// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose;

namespace osu.Game.Rulesets.Karaoke.Edit.Blueprints
{
    public class NoteSelectionBlueprint : KaraokeSelectionBlueprint
    {
        public new DrawableNote DrawableObject => (DrawableNote)base.DrawableObject;

        public new Note HitObject => DrawableObject.HitObject;

        [Resolved(CanBeNull = false)]
        private IPlacementHandler placementHandler { get; set; }

        [Resolved]
        protected EditorBeatmap EditorBeatmap { get; private set; }

        public override MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem(HitObject.Display ? "Hide" : "Show", HitObject.Display ? MenuItemType.Destructive : MenuItemType.Standard, () => ChangeDisplay(!HitObject.Display)),
            new OsuMenuItem("Split", MenuItemType.Destructive, splitNote),
        };

        private void splitNote()
        {
            // TODO : percentage should be enter by dialog
            var splittedNote = HitObject.CopyByPercentage(0.5);
            EditorBeatmap?.Add(splittedNote);
            // Change object's duration
            HitObject.Duration = HitObject.Duration - splittedNote.Duration;
        }

        public void ChangeDisplay(bool display)
        {
            HitObject.Display = display;

            // Move to center if note is not display
            if (!HitObject.Display)
                HitObject.Tone = new Tone();
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
        }
    }
}
