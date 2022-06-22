// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeSelectionHandler : EditorSelectionHandler
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        [Resolved]
        private ISkinSource source { get; set; }

        [Resolved]
        private HitObjectComposer composer { get; set; }

        [Resolved]
        private INotesChangeHandler notesChangeHandler { get; set; }

        [Resolved]
        private ILyricSingerChangeHandler lyricSingerChangeHandler { get; set; }

        protected ScrollingNotePlayfield NotePlayfield => ((KaraokeHitObjectComposer)composer).Playfield.NotePlayfield;

        protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<HitObject>> selection)
        {
            if (selection.All(x => x is LyricSelectionBlueprint))
            {
                return new[]
                {
                    createSingerMenuItem()
                };
            }

            if (EditorBeatmap.SelectedHitObjects.All(x => x is Note)
                && EditorBeatmap.SelectedHitObjects.Count > 1)
            {
                var menu = new List<MenuItem>();
                var selectedObject = EditorBeatmap.SelectedHitObjects.Cast<Note>().OrderBy(x => x.StartTime).ToArray();

                // Set multi note display property
                menu.Add(createMultiNoteDisplayPropertyMenuItem(selectedObject));

                // Combine multi note if they has same start and end index.
                var firstObject = selectedObject.FirstOrDefault();
                if (firstObject != null && selectedObject.All(x => x.StartIndex == firstObject.StartIndex && x.EndIndex == firstObject.EndIndex))
                    menu.Add(createCombineNoteMenuItem());

                return menu;
            }

            return new List<MenuItem>();
        }

        private MenuItem createMultiNoteDisplayPropertyMenuItem(IReadOnlyCollection<Note> selectedObject)
        {
            bool display = selectedObject.Count(x => x.Display) >= selectedObject.Count(x => !x.Display);
            string displayText = display ? "Hide" : "Show";
            return new OsuMenuItem($"{displayText} {selectedObject.Count} notes.", display ? MenuItemType.Destructive : MenuItemType.Standard,
                () =>
                {
                    notesChangeHandler.ChangeDisplayState(!display);
                });
        }

        private MenuItem createCombineNoteMenuItem()
        {
            return new OsuMenuItem("Combine", MenuItemType.Standard, () =>
            {
                notesChangeHandler.Combine();
            });
        }

        private MenuItem createSingerMenuItem()
        {
            return new SingerContextMenu(beatmap, lyricSingerChangeHandler, "Singer");
        }

        public override bool HandleMovement(MoveSelectionEvent<HitObject> moveEvent)
        {
            // Only note can be moved.
            if (moveEvent.Blueprint is not NoteSelectionBlueprint noteSelectionBlueprint)
                return false;

            var lastTone = noteSelectionBlueprint.HitObject.Tone;
            performColumnMovement(lastTone, moveEvent);

            return true;
        }

        private void performColumnMovement(Tone lastTone, MoveSelectionEvent<HitObject> moveEvent)
        {
            if (moveEvent.Blueprint is not NoteSelectionBlueprint)
                return;

            var calculator = notePositionInfo.Calculator;

            // get center position
            var screenSpacePosition = moveEvent.Blueprint.ScreenSpaceSelectionPoint + moveEvent.ScreenSpaceDelta;
            var position = NotePlayfield.ToLocalSpace(screenSpacePosition);
            var centerPosition = new Vector2(position.X, position.Y - NotePlayfield.Height / 2);

            // get delta position
            float lastCenterPosition = calculator.YPositionAt(lastTone);
            float delta = centerPosition.Y - lastCenterPosition;

            // get delta tone.
            const float trigger_height = ScrollingNotePlayfield.COLUMN_SPACING + DefaultColumnBackground.COLUMN_HEIGHT;
            var deltaTone = delta switch
            {
                > trigger_height => -new Tone { Half = true },
                < 0 => new Tone { Half = true },
                _ => default
            };

            if (deltaTone == default(Tone))
                return;

            foreach (var note in EditorBeatmap.SelectedHitObjects.OfType<Note>())
            {
                if (note.Tone >= calculator.MaxTone && deltaTone > 0)
                    continue;
                if (note.Tone <= calculator.MinTone && deltaTone < 0)
                    continue;

                note.Tone += deltaTone;

                //Change all note to visible
                note.Display = true;
            }
        }
    }
}
