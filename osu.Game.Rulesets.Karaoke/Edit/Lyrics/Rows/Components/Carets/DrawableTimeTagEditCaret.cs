// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public class DrawableTimeTagEditCaret : DrawableCaret, IApplicableCaretPosition<TimeTagIndexCaretPosition>
    {
        private const float triangle_width = 8;

        [Resolved]
        private OsuColour colours { get; set; }

        [Resolved]
        private EditorLyricPiece lyricPiece { get; set; }

        private readonly RightTriangle drawableTimeTag;

        public DrawableTimeTagEditCaret(bool preview)
            : base(preview)
        {
            AutoSizeAxes = Axes.Both;

            InternalChild = drawableTimeTag = new RightTriangle
            {
                Name = "Time tag triangle",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(triangle_width),
                Alpha = preview ? 0.5f : 1
            };
        }

        public void Apply(TimeTagIndexCaretPosition caret)
        {
            Position = lyricPiece.GetTextIndexPosition(caret.Index);
            drawableTimeTag.Scale = new Vector2(caret.Index.State == TextIndex.IndexState.Start ? 1 : -1, 1);
        }
    }
}
