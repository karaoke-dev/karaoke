// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Carets
{
    public class DrawableTimeTagEditCaret : CompositeDrawable, IDrawableCaret, IHasCaretPosition
    {
        private const float triangle_width = 8;

        [Resolved]
        private OsuColour colours { get; set; }

        private readonly RightTriangle drawableTimeTag;

        public DrawableTimeTagEditCaret()
        {
            AutoSizeAxes = Axes.Both;

            InternalChild = drawableTimeTag = new RightTriangle
            {
                Name = "Time tag triangle",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(triangle_width),
            };
        }

        private CursorPosition position;

        public CursorPosition CaretPosition
        {
            get => position;
            set
            {
                position = value;
                drawableTimeTag.Scale = new Vector2(position.Index.State == TextIndex.IndexState.Start ? 1 : -1, 1);

                // todo : color is by has time-tag here?
                // drawableTimeTag.Colour = position.Time.HasValue ? colours.YellowDarker : colours.Gray3;
            }
        }

        private bool preview;

        public bool Preview
        {
            get => preview;
            set
            {
                preview = value;
                drawableTimeTag.Alpha = preview ? 0.5f : 1;
            }
        }
    }
}
