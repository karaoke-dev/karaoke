// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes
{
    public class NoteSelectionBlueprint : KaraokeSelectionBlueprint<Note>
    {
        public new DrawableNote DrawableObject => (DrawableNote)base.DrawableObject;

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        [Resolved]
        protected EditorBeatmap EditorBeatmap { get; private set; }

        public override MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem(HitObject.Display ? "Hide" : "Show", HitObject.Display ? MenuItemType.Destructive : MenuItemType.Standard, () => ChangeDisplay(!HitObject.Display)),
            new OsuMenuItem("Split", MenuItemType.Destructive, splitNote),
        };

        private void splitNote()
        {
            // TODO : percentage should be entered by dialog
            var splittedNote = HitObject.CopyByPercentage(0.5);
            EditorBeatmap?.Add(splittedNote);
            // Change object's duration
            HitObject.Duration = HitObject.Duration - splittedNote.Duration;
        }

        public void ChangeDisplay(bool display)
        {
            changeHandler.BeginChange();

            HitObject.Display = display;

            // Move to center if note is not displayed
            if (!HitObject.Display)
                HitObject.Tone = new Tone();

            changeHandler.EndChange();
        }

        public NoteSelectionBlueprint(DrawableNote note)
            : base(note)
        {
            AddInternal(new EditBodyPiece
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        protected override void Update()
        {
            base.Update();

            Size = DrawableObject.DrawSize;
            Position = Parent.ToLocalSpace(DrawableObject.ToScreenSpace(Vector2.Zero));
        }
    }
}
