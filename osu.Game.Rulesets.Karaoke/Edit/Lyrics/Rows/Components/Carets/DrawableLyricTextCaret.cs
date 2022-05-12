// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public abstract class DrawableLyricTextCaret : DrawableCaret<TextCaretPosition>
    {
        [Resolved]
        private EditorKaraokeSpriteText karaokeSpriteText { get; set; }

        protected DrawableLyricTextCaret(bool preview)
            : base(preview)
        {
        }

        protected Vector2 GetPosition(TextCaretPosition caret)
        {
            float textHeight = karaokeSpriteText.LineBaseHeight;
            bool end = caret.Index == caret.Lyric?.Text?.Length;
            var originPosition = karaokeSpriteText.GetTextIndexPosition(TextIndexUtils.FromStringIndex(caret.Index, end));
            return new Vector2(originPosition.X, originPosition.Y - textHeight);
        }
    }
}
