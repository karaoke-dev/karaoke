// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints
{
    public abstract class TagSelectionBlueprint<T> : SelectionBlueprint<T>
    {
        protected TagSelectionBlueprint(T item)
            : base(item)
        {
            RelativeSizeAxes = Axes.None;
        }

        protected void UpdatePositionAndSize(RectangleF position)
        {
            X = position.X;
            Y = position.Y;
            Width = position.Width;
            Height = position.Height;
        }

        public override Vector2 ScreenSpaceSelectionPoint => ScreenSpaceDrawQuad.TopLeft;
    }
}
