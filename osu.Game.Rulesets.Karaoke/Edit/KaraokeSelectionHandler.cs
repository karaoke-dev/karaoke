// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
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

        public override MenuItem[] ContextMenuItems
        {
            get
            {
                var menu = base.ContextMenuItems;
                menu = menu.ToList().Where(x => x.Text.Value != "Sound" && x.Text.Value != "Delete").ToArray();

                if (SelectedHitObjects.All(x => x is LyricLine))
                {
                    menu = menu.Append(createLayoutMenuItem()).Append(createFontMenuItem()).ToArray();
                }

                // If selected more then two notes
                if (SelectedHitObjects.All(x => x is Note)
                    && SelectedHitObjects.Count() > 1)
                {
                    var selectedObject = SelectedHitObjects.Cast<Note>().OrderBy(x => x.StartTime);

                    // Set multi note display property
                    menu = menu.Append(createMultiNoteDisplayPropertyMenuItem(selectedObject)).ToArray();

                    // Combine multi note if they has same start and end index.
                    var firstObject = selectedObject.FirstOrDefault();
                    if (firstObject != null && selectedObject.All(x => x.StartIndex == firstObject.StartIndex && x.EndIndex == firstObject.EndIndex))
                        menu = menu.Append(createCombineNoteMenuItem(selectedObject)).ToArray();
                }

                return menu;
            }
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
                var endTime = selectedObject.LastOrDefault()?.EndTime;
                if (endTime == null)
                    return;

                // Recover end time
                var firstObject = selectedObject.FirstOrDefault();
                if (firstObject != null)
                    firstObject.EndTime = endTime.Value;

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
            var layoutDictionary = source.GetConfig<KaraokeIndexLookup, Dictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            return new OsuMenuItem("Layout")
            {
                Items = layoutDictionary.Select(x => new TernaryStateMenuItem(x.Value, MenuItemType.Standard, state =>
                {
                    if (state == TernaryState.True)
                        SelectedHitObjects.Cast<LyricLine>().ForEach(l => l.LayoutIndex = x.Key);
                })).ToArray()
            };
        }

        private MenuItem createFontMenuItem()
        {
            var fontDictionary = source.GetConfig<KaraokeIndexLookup, Dictionary<int, string>>(KaraokeIndexLookup.Style)?.Value;
            return new OsuMenuItem("Font")
            {
                Items = fontDictionary.Select(x => new TernaryStateMenuItem(x.Value, MenuItemType.Standard, state =>
                {
                    if (state == TernaryState.True)
                        SelectedHitObjects.Cast<LyricLine>().ForEach(l => l.FontIndex = x.Key);
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
            if (!(moveEvent.Blueprint is NoteSelectionBlueprint noteBlueprint))
                return;

            // top position
            var dragHeight = noteBlueprint.DrawableObject.Parent.ToLocalSpace(moveEvent.ScreenSpacePosition).Y;
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

            foreach (var note in SelectedHitObjects.OfType<Note>())
            {
                if (note.Tone >= calculator.MaxTone() && deltaTone > 0)
                    continue;
                if (note.Tone <= calculator.MinTone() && deltaTone < 0)
                    continue;

                note.Tone += deltaTone;

                //Change all note to visible
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
