// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public class DrawableLyricInputCaret : Caret, IDrawableCaret
    {
        private const float caret_move_time = 60;
        private const float caret_width = 3;

        [Resolved]
        private OsuColour colours { get; set; }

        public DrawableLyricInputCaret()
        {
            Width = caret_width;

            Colour = Color4.Transparent;

            Masking = true;
            CornerRadius = 1;

            InternalChild = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.White,
            };
        }

        public override void Hide() => this.FadeOut(200);

        public override void DisplayAt(Vector2 position, float? selectionWidth)
        {
            var displayAnimation = Alpha > 0;
            var time = displayAnimation ? 60 : 0;

            if (selectionWidth != null)
            {
                var selectionColour = colours.Yellow;

                this.MoveTo(new Vector2(position.X, position.Y), time, Easing.Out);
                this.ResizeWidthTo(selectionWidth.Value + caret_width / 2, caret_move_time, Easing.Out);
                this
                    .FadeTo(0.5f, 200, Easing.Out)
                    .FadeColour(selectionColour, 200, Easing.Out);
            }
            else
            {
                this.MoveTo(new Vector2(position.X - caret_width / 2, position.Y), time, Easing.Out);
                this.ResizeWidthTo(caret_width, caret_move_time, Easing.Out);
                this
                    .FadeColour(Color4.White, 200, Easing.Out)
                    .Loop(c => c.FadeTo(0.7f).FadeTo(0.4f, 500, Easing.InOutSine));
            }
        }

        private bool preview;

        public bool Preview
        {
            get => preview;
            set
            {
                preview = value;
                InternalChild.Alpha = preview ? 0.5f : 1;
            }
        }
    }
}
