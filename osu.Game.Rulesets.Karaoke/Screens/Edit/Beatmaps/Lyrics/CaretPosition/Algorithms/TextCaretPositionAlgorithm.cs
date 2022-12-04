// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms
{
    public abstract class TextCaretPositionAlgorithm<TCaretPosition> : IndexCaretPositionAlgorithm<TCaretPosition> where TCaretPosition : struct, ITextCaretPosition
    {
        protected TextCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected sealed override void Validate(TCaretPosition input)
        {
            Debug.Assert(indexInTextRange(input.Index, input.Lyric));
        }

        protected sealed override bool PositionMovable(TCaretPosition position)
        {
            return indexInTextRange(position.Index, position.Lyric);
        }

        protected sealed override TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return CreateCaretPosition(lyric, index);
        }

        protected sealed override TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, lyricMovable);
            if (lyric == null)
                return null;

            int index = Math.Clamp(currentPosition.Index, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

            return CreateCaretPosition(lyric, index);
        }

        protected sealed override TCaretPosition? MoveToFirstLyric()
        {
            var lyric = Lyrics.FirstOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return CreateCaretPosition(lyric, GetMinIndex(lyric.Text));
        }

        protected sealed override TCaretPosition? MoveToLastLyric()
        {
            var lyric = Lyrics.LastOrDefault(lyricMovable);
            if (lyric == null)
                return null;

            return CreateCaretPosition(lyric, GetMaxIndex(lyric.Text));
        }

        protected sealed override TCaretPosition? MoveToTargetLyric(Lyric lyric)
            => MoveToTargetLyric(lyric, GetMinIndex(lyric.Text));

        protected sealed override TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int previousIndex = currentPosition.Index - 1;

            if (!indexInTextRange(previousIndex, lyric))
                return null;

            return CreateCaretPosition(lyric, previousIndex);
        }

        protected sealed override TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            int nextIndex = currentPosition.Index + 1;

            if (!indexInTextRange(nextIndex, lyric))
                return null;

            return CreateCaretPosition(lyric, nextIndex);
        }

        protected sealed override TCaretPosition? MoveToFirstIndex(Lyric lyric)
        {
            int index = GetMinIndex(lyric.Text);

            return CreateCaretPosition(lyric, index);
        }

        protected sealed override TCaretPosition? MoveToLastIndex(Lyric lyric)
        {
            int index = GetMaxIndex(lyric.Text);

            return CreateCaretPosition(lyric, index);
        }

        protected override TCaretPosition? MoveToTargetLyric<TIndex>(Lyric lyric, TIndex? index) where TIndex : default
        {
            if (index is not int value)
                throw new InvalidCastException();

            return CreateCaretPosition(lyric, value, CaretGenerateType.TargetLyric);
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
