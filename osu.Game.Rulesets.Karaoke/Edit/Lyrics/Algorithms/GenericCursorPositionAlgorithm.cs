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
    public class GenericCursorPositionAlgorithm : CursorPositionAlgorithm, ICursorPositionAlgorithm
    {
        public GenericCursorPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public virtual bool PositionMovable(CursorPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (TextIndexUtils.OutOfRange(position.Index, position.Lyric.Text))
                return false;

            return true;
        }

        public CursorPosition? MoveUp(CursorPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = currentPosition.Index.State;

            return new CursorPosition(lyric, new TextIndex(index, state));
        }

        public CursorPosition? MoveDown(CursorPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = currentPosition.Index.State;

            return new CursorPosition(lyric, new TextIndex(index, state));
        }

        public CursorPosition? MoveLeft(CursorPosition currentPosition)
        {
            // get previous cursor and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var previousIndex = GetPreviousIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(previousIndex, lyric?.Text))
                return MoveUp(new CursorPosition(currentPosition.Lyric, new TextIndex(int.MaxValue)));

            return new CursorPosition(currentPosition.Lyric, previousIndex);
        }

        public CursorPosition? MoveRight(CursorPosition currentPosition)
        {
            // get next cursor and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var nextIndex = GetNextIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(nextIndex, lyric?.Text))
                return MoveDown(new CursorPosition(currentPosition.Lyric, new TextIndex(int.MinValue)));

            return new CursorPosition(currentPosition.Lyric, nextIndex);
        }

        public CursorPosition? MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            var index = new TextIndex();
            return new CursorPosition(lyric, index);
        }

        public CursorPosition? MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            var textLength = lyric?.Text.Length ?? 0;
            var index = new TextIndex(textLength - 1, TextIndex.IndexState.End);
            return new CursorPosition(lyric, index);
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
