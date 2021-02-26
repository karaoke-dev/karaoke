// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public class TypingCaretPositionAlgorithm : CaretPositionAlgorithm<TextCaretPosition>
    {
        public TypingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(TextCaretPosition position)
        {
            return InRange(position.Index, position?.Lyric);
        }

        public override TextCaretPosition MoveUp(TextCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return new TextCaretPosition(lyric, index);
        }

        public override TextCaretPosition MoveDown(TextCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return new TextCaretPosition(lyric, index);
        }

        public override TextCaretPosition MoveLeft(TextCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var previousIndex = currentPosition.Index - 1;

            if (!InRange(previousIndex, lyric))
                return MoveUp(new TextCaretPosition(currentPosition.Lyric, int.MaxValue));

            return new TextCaretPosition(currentPosition.Lyric, previousIndex);
        }

        public override TextCaretPosition MoveRight(TextCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var nextIndex = currentPosition.Index + 1;

            if (!InRange(nextIndex, lyric))
                return MoveDown(new TextCaretPosition(currentPosition.Lyric, int.MinValue));

            return new TextCaretPosition(currentPosition.Lyric, nextIndex);
        }

        public override TextCaretPosition MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            return new TextCaretPosition(lyric, 0);
        }

        public override TextCaretPosition MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            var textLength = lyric?.Text.Length ?? 0;
            var index = textLength - 1;
            return new TextCaretPosition(lyric, index);
        }

        protected bool InRange(int index, Lyric lyric)
        {
            var text = lyric.Text;
            if (string.IsNullOrEmpty(text))
                return false;

            return index >= GetMinIndex(text) && index <= GetMaxIndex(text);
        }

        protected virtual int GetMinIndex(string text) => 0;

        protected virtual int GetMaxIndex(string text) => text.Length;
    }
}
