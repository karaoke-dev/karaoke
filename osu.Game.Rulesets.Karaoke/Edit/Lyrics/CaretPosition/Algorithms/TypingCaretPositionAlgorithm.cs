// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class TypingCaretPositionAlgorithm : CaretPositionAlgorithm<TextCaretPosition>
    {
        public TypingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(TextCaretPosition position)
        {
            return indexInTextRange(position.Index, position.Lyric);
        }

        public override TextCaretPosition? MoveUp(TextCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return new TextCaretPosition(lyric, index);
        }

        public override TextCaretPosition? MoveDown(TextCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return new TextCaretPosition(lyric, index);
        }

        public override TextCaretPosition? MoveLeft(TextCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int previousIndex = currentPosition.Index - 1;

            if (!indexInTextRange(previousIndex, lyric))
                return MoveUp(new TextCaretPosition(currentPosition.Lyric, int.MaxValue));

            return new TextCaretPosition(currentPosition.Lyric, previousIndex);
        }

        public override TextCaretPosition? MoveRight(TextCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int nextIndex = currentPosition.Index + 1;

            if (!indexInTextRange(nextIndex, lyric))
                return MoveDown(new TextCaretPosition(currentPosition.Lyric, int.MinValue));

            return new TextCaretPosition(currentPosition.Lyric, nextIndex);
        }

        public override TextCaretPosition? MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return new TextCaretPosition(lyric, GetMinIndex(lyric.Text));
        }

        public override TextCaretPosition? MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return new TextCaretPosition(lyric, GetMaxIndex(lyric.Text));
        }

        public override TextCaretPosition MoveToTarget(Lyric lyric) => new(lyric, GetMinIndex(lyric.Text));

        private bool lyricMovable(Lyric lyric)
        {
            int minIndex = GetMinIndex(lyric.Text);
            return indexInTextRange(minIndex, lyric);
        }

        private bool indexInTextRange(int index, Lyric lyric)
        {
            string text = lyric.Text;
            if (string.IsNullOrEmpty(text))
                return index == 0;

            return index >= GetMinIndex(text) && index <= GetMaxIndex(text);
        }

        protected virtual int GetMinIndex(string text) => 0;

        protected virtual int GetMaxIndex(string text) => text.Length;
    }
}
