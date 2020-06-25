// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Blueprints
{
    public class KaraokeSelectionBlueprint : OverlaySelectionBlueprint
    {
        public Vector2 ScreenSpaceDragPosition { get; private set; }
        public Vector2 DragPosition { get; private set; }

        public new DrawableKaraokeScrollingHitObject DrawableObject => (DrawableKaraokeScrollingHitObject)base.DrawableObject;

        public KaraokeSelectionBlueprint(DrawableHitObject drawableObject)
            : base(drawableObject)
        {
            RelativeSizeAxes = Axes.None;
        }

        protected override void Update()
        {
            base.Update();

            Position = Parent.ToLocalSpace(DrawableObject.ToScreenSpace(Vector2.Zero));
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            ScreenSpaceDragPosition = e.ScreenSpaceMousePosition;
            DragPosition = DrawableObject.ToLocalSpace(e.ScreenSpaceMousePosition);

            return base.OnMouseDown(e);
        }

        protected override void OnDrag(DragEvent e)
        {
            base.OnDrag(e);

            ScreenSpaceDragPosition = e.ScreenSpaceMousePosition;
            DragPosition = DrawableObject.ToLocalSpace(e.ScreenSpaceMousePosition);
        }

        public override void Show()
        {
            DrawableObject.AlwaysAlive = true;
            base.Show();
        }

        public override void Hide()
        {
            DrawableObject.AlwaysAlive = false;
            base.Hide();
        }
    }
}
