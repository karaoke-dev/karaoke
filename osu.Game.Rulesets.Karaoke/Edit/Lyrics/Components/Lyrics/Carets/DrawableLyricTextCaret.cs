// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Carets
{
    public abstract class DrawableLyricTextCaret<TCaretPosition> : DrawableCaret<TCaretPosition> where TCaretPosition : class, ITextCaretPosition
    {
        [Resolved]
        private InteractableKaraokeSpriteText karaokeSpriteText { get; set; }

        protected DrawableLyricTextCaret(bool preview)
            : base(preview)
        {
        }

        protected Vector2 GetPosition(TCaretPosition caret)
        {
            float textHeight = karaokeSpriteText.LineBaseHeight;
            bool end = caret.Index == caret.Lyric.Text.Length;
            var originPosition = karaokeSpriteText.GetTextIndexPosition(TextIndexUtils.FromStringIndex(caret.Index, end));
            return new Vector2(originPosition.X, originPosition.Y - textHeight);
        }
    }
}
