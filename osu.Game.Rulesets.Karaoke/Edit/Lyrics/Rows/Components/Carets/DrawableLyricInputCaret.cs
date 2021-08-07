// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public class DrawableLyricInputCaret : DrawableLyricTextCaret
    {
        private const float caret_move_time = 60;
        private const float caret_width = 3;

        [Resolved]
        private OsuColour colours { get; set; }

        [Resolved]
        private EditorLyricPiece lyricPiece { get; set; }

        public DrawableLyricInputCaret(bool preview)
            : base(preview)
        {
            Width = caret_width;
            Colour = Color4.Transparent;
            Masking = true;
            CornerRadius = 1;

            InternalChild = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.White,
                Alpha = preview ? 0.5f : 1
            };
        }

        public override void Hide() => this.FadeOut(200);

        protected override void Apply(TextCaretPosition caret)
        {
            Height = lyricPiece.LineBaseHeight;
            var position = GetPosition(caret);

            var displayAnimation = Alpha > 0;
            var time = displayAnimation ? 60 : 0;

            this.MoveTo(new Vector2(position.X - caret_width / 2, position.Y), time, Easing.Out);
            this.ResizeWidthTo(caret_width, caret_move_time, Easing.Out);
            this
                .FadeColour(Color4.White, 200, Easing.Out)
                .Loop(c => c.FadeTo(0.7f).FadeTo(0.4f, 500, Easing.InOutSine));
        }
    }
}
