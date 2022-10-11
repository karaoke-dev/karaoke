// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class TypingCaretPositionAlgorithm : CaretPositionAlgorithm<TypingCaretPosition>
    {
        public TypingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(TypingCaretPosition position)
        {
            return indexInTextRange(position.Index, position.Lyric);
        }

        public override TypingCaretPosition? MoveUp(TypingCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return new TypingCaretPosition(lyric, index);
        }

        public override TypingCaretPosition? MoveDown(TypingCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return new TypingCaretPosition(lyric, index);
        }

        public override TypingCaretPosition? MoveLeft(TypingCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int previousIndex = currentPosition.Index - 1;

            if (!indexInTextRange(previousIndex, lyric))
                return MoveUp(new TypingCaretPosition(currentPosition.Lyric, int.MaxValue));

            return new TypingCaretPosition(currentPosition.Lyric, previousIndex);
        }

        public override TypingCaretPosition? MoveRight(TypingCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int nextIndex = currentPosition.Index + 1;

            if (!indexInTextRange(nextIndex, lyric))
                return MoveDown(new TypingCaretPosition(currentPosition.Lyric, int.MinValue));

            return new TypingCaretPosition(currentPosition.Lyric, nextIndex);
        }

        public override TypingCaretPosition? MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return new TypingCaretPosition(lyric, GetMinIndex(lyric.Text));
        }

        public override TypingCaretPosition? MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return new TypingCaretPosition(lyric, GetMaxIndex(lyric.Text));
        }

        public override TypingCaretPosition MoveToTarget(Lyric lyric) => new(lyric, GetMinIndex(lyric.Text), CaretGenerateType.TargetLyric);

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
