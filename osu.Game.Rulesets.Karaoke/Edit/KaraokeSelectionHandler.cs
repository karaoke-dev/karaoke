// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit.Compose;
using osu.Game.Screens.Edit.Compose.Components;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeSelectionHandler : SelectionHandler
    {
        [Resolved]
        private IPositionCalculator calculator { get; set; }

        [Resolved]
        private IPlacementHandler placementHandler { get; set; }

        [Resolved]
        private HitObjectComposer composer { get; set; }

        protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint> selection)
        {
            if (selection.All(x => x is LyricSelectionBlueprint))
            {
                return new[]
                {
                    createLayoutMenuItem(),
                    createFontMenuItem()
                };
            }

            if (EditorBeatmap.SelectedHitObjects.All(x => x is Note)
                && EditorBeatmap.SelectedHitObjects.Count > 1)
            {
                var menu = new List<MenuItem>();
                var selectedObject = EditorBeatmap.SelectedHitObjects.Cast<Note>().OrderBy(x => x.StartTime);

                // Set multi note display property
                menu.Add(createMultiNoteDisplayPropertyMenuItem(selectedObject));

                // Combine multi note if they have the same start and end index.
                var firstObject = selectedObject.FirstOrDefault();
                if (firstObject != null && selectedObject.All(x => x.StartIndex == firstObject.StartIndex && x.EndIndex == firstObject.EndIndex))
                    menu.Add(createCombineNoteMenuItem(selectedObject));

                return menu;
            }

            return new List<MenuItem>();
        }

        private MenuItem createMultiNoteDisplayPropertyMenuItem(IEnumerable<Note> selectedObject)
        {
            var display = selectedObject.Count(x => x.Display) >= selectedObject.Count(x => !x.Display);
            var displayText = display ? "Hide" : "Show";
            return new OsuMenuItem($"{displayText} {selectedObject.Count()} notes.", display ? MenuItemType.Destructive : MenuItemType.Standard,
                () => { SelectedBlueprints.OfType<NoteSelectionBlueprint>().ForEach(x => x.ChangeDisplay(!display)); });
        }

        private MenuItem createCombineNoteMenuItem(IEnumerable<Note> selectedObject)
        {
            return new OsuMenuItem("Combine", MenuItemType.Standard, () =>
            {
                // Select at least two objects.
                if (selectedObject.Count() < 2)
                    return;

                ChangeHandler.BeginChange();

                // Recover end time
                var firstObject = selectedObject.FirstOrDefault();
                if (firstObject != null)
                    firstObject.Duration = selectedObject.Sum(x => x.Duration);

                ChangeHandler.EndChange();

                // Delete objects
                var deleteObjects = selectedObject.Skip(1).ToList();

                foreach (var deleteObject in deleteObjects)
                {
                    placementHandler.Delete(deleteObject);
                }
            });
        }

        private MenuItem createLayoutMenuItem()
        {
            var layoutDictionary = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            return new OsuMenuItem("Layout")
            {
                Items = layoutDictionary.Select(x => new TernaryStateMenuItem(x.Value, MenuItemType.Standard, state =>
                {
                    ChangeHandler.BeginChange();

                    if (state == TernaryState.True)
                        EditorBeatmap.SelectedHitObjects.Cast<Lyric>().ForEach(l => l.LayoutIndex = x.Key);

                    ChangeHandler.EndChange();
                })).ToArray()
            };
        }

        private MenuItem createFontMenuItem()
        {
            var fontDictionary = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Style)?.Value;
            return new OsuMenuItem("Font")
            {
                Items = fontDictionary.Select(x => new TernaryStateMenuItem(x.Value, MenuItemType.Standard, state =>
                {
                    ChangeHandler.BeginChange();

                    // todo : SingerUtils not used in here, and this logic should be combined into singer manager.
                    if (state == TernaryState.True)
                        EditorBeatmap.SelectedHitObjects.Cast<Lyric>().ForEach(l => l.Singers = SingerUtils.GetSingersIndex(x.Key));

                    ChangeHandler.EndChange();
                })).ToArray()
            };
        }

        private ISkinSource source;

        [BackgroundDependencyLoader]
        private void load(ISkinSource source)
        {
            this.source = source;
        }

        public override bool HandleMovement(MoveSelectionEvent moveEvent)
        {
            if (!(moveEvent.Blueprint is NoteSelectionBlueprint noteSelectionBlueprint))
                return true;

            var lastTone = noteSelectionBlueprint.DrawableObject.HitObject.Tone;
            performColumnMovement(lastTone, moveEvent);

            return true;
        }

        private void performColumnMovement(Tone lastTone, MoveSelectionEvent moveEvent)
        {
            if (!(moveEvent.Blueprint is NoteSelectionBlueprint))
                return;

            var karaokePlayfield = ((KaraokeHitObjectComposer)composer).Playfield;

            // top position
            var dragHeight = karaokePlayfield.NotePlayfield.ToLocalSpace(moveEvent.ScreenSpacePosition).Y;
            var lastHeight = convertToneToHeight(lastTone);
            var moveHeight = dragHeight - lastHeight;

            var deltaTone = new Tone();
            const float trigger_height = NotePlayfield.COLUMN_SPACING + DefaultColumnBackground.COLUMN_HEIGHT;

            if (moveHeight > trigger_height)
                deltaTone = -new Tone { Half = true };
            else if (moveHeight < 0)
                deltaTone = new Tone { Half = true };

            if (deltaTone == 0)
                return;

            foreach (var note in EditorBeatmap.SelectedHitObjects.OfType<Note>())
            {
                if (note.Tone >= calculator.MaxTone() && deltaTone > 0)
                    continue;
                if (note.Tone <= calculator.MinTone() && deltaTone < 0)
                    continue;

                note.Tone += deltaTone;

                //Change all notes to visible
                note.Display = true;
            }
        }

        private float convertToneToHeight(Tone tone)
        {
            var maxTone = calculator.MaxTone();
            return calculator.YPositionAt(tone - maxTone);
        }
    }
}
