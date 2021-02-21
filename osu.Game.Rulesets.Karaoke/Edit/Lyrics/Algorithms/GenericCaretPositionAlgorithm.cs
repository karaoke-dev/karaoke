// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public class GenericCaretPositionAlgorithm : CaretPositionAlgorithm, ICaretPositionAlgorithm
    {
        public GenericCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public virtual bool PositionMovable(CaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (TextIndexUtils.OutOfRange(position.Index, position.Lyric.Text))
                return false;

            return true;
        }

        public CaretPosition? MoveUp(CaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = currentPosition.Index.State;

            return new CaretPosition(lyric, new TextIndex(index, state));
        }

        public CaretPosition? MoveDown(CaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = currentPosition.Index.State;

            return new CaretPosition(lyric, new TextIndex(index, state));
        }

        public CaretPosition? MoveLeft(CaretPosition currentPosition)
        {
            // get previous cursor and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var previousIndex = GetPreviousIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(previousIndex, lyric?.Text))
                return MoveUp(new CaretPosition(currentPosition.Lyric, new TextIndex(int.MaxValue)));

            return new CaretPosition(currentPosition.Lyric, previousIndex);
        }

        public CaretPosition? MoveRight(CaretPosition currentPosition)
        {
            // get next cursor and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var nextIndex = GetNextIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(nextIndex, lyric?.Text))
                return MoveDown(new CaretPosition(currentPosition.Lyric, new TextIndex(int.MinValue)));

            return new CaretPosition(currentPosition.Lyric, nextIndex);
        }

        public CaretPosition? MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            var index = new TextIndex();
            return new CaretPosition(lyric, index);
        }

        public CaretPosition? MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            var textLength = lyric?.Text.Length ?? 0;
            var index = new TextIndex(textLength - 1, TextIndex.IndexState.End);
            return new CaretPosition(lyric, index);
        }

        protected virtual TextIndex GetPreviousIndex(TextIndex currentIndex)
        {
            var nextIndex = TextIndexUtils.ToStringIndex(currentIndex) - 1;
            var nextState = TextIndexUtils.ReverseState(currentIndex.State);
            return new TextIndex(nextIndex, nextState);
        }

        protected virtual TextIndex GetNextIndex(TextIndex currentIndex)
        {
            var nextIndex = TextIndexUtils.ToStringIndex(currentIndex);
            var nextState = TextIndexUtils.ReverseState(currentIndex.State);
            return new TextIndex(nextIndex, nextState);
        }
    }
}
