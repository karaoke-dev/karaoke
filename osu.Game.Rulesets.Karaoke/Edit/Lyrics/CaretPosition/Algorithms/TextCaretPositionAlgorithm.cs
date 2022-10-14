// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public abstract class TextCaretPositionAlgorithm<TCaretPosition> : IndexCaretPositionAlgorithm<TCaretPosition> where TCaretPosition : struct, ITextCaretPosition
    {
        protected TextCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override void Validate(TCaretPosition input)
        {
            Debug.Assert(indexInTextRange(input.Index, input.Lyric));
        }

        protected override bool PositionMovable(TCaretPosition position)
        {
            return indexInTextRange(position.Index, position.Lyric);
        }

        protected override TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return CreateCaretPosition(lyric, index);
        }

        protected override TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return CreateCaretPosition(lyric, index);
        }

        protected override TCaretPosition? MoveToFirstLyric()
        {
            var lyric = Lyrics.FirstOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return CreateCaretPosition(lyric, GetMinIndex(lyric.Text));
        }

        protected override TCaretPosition? MoveToLastLyric()
        {
            var lyric = Lyrics.LastOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return CreateCaretPosition(lyric, GetMaxIndex(lyric.Text));
        }

        protected override TCaretPosition? MoveToTargetLyric(Lyric lyric) => CreateCaretPosition(lyric, GetMinIndex(lyric.Text), CaretGenerateType.TargetLyric);

        protected override TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int previousIndex = currentPosition.Index - 1;

            if (!indexInTextRange(previousIndex, lyric))
                return MoveToPreviousLyric(CreateCaretPosition(lyric, int.MaxValue));

            return CreateCaretPosition(lyric, previousIndex);
        }

        protected override TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int nextIndex = currentPosition.Index + 1;

            if (!indexInTextRange(nextIndex, lyric))
                return MoveToNextLyric(CreateCaretPosition(lyric, int.MinValue));

            return CreateCaretPosition(lyric, nextIndex);
        }

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

        protected abstract TCaretPosition CreateCaretPosition(Lyric lyric, int index, CaretGenerateType generateType = CaretGenerateType.Action);

        protected abstract int GetMinIndex(string text);

        protected abstract int GetMaxIndex(string text);
    }
}
