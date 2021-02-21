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
    public class TypingCaretPositionAlgorithm : CaretPositionAlgorithm, ICaretPositionAlgorithm<TextCaretPosition>
    {
        public TypingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public virtual bool PositionMovable(TextCaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (TextIndexUtils.OutOfRange(new TextIndex(position.Index), position.Lyric.Text))
                return false;

            return true;
        }

        public TextCaretPosition MoveUp(TextCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index, 0, lyricTextLength - 1);

            return new TextCaretPosition(lyric, index);
        }

        public TextCaretPosition MoveDown(TextCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index, 0, lyricTextLength - 1);

            return new TextCaretPosition(lyric, index);
        }

        public TextCaretPosition MoveLeft(TextCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var previousIndex = GetPreviousIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(new TextIndex(previousIndex), lyric?.Text))
                return MoveUp(new TextCaretPosition(currentPosition.Lyric, int.MaxValue));

            return new TextCaretPosition(currentPosition.Lyric, previousIndex);
        }

        public TextCaretPosition MoveRight(TextCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var nextIndex = GetNextIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(new TextIndex(nextIndex), lyric?.Text))
                return MoveDown(new TextCaretPosition(currentPosition.Lyric, int.MinValue));

            return new TextCaretPosition(currentPosition.Lyric, nextIndex);
        }

        public TextCaretPosition MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            return new TextCaretPosition(lyric, 0);
        }

        public TextCaretPosition MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            var textLength = lyric?.Text.Length ?? 0;
            var index = textLength - 1;
            return new TextCaretPosition(lyric, index);
        }

        protected virtual int GetPreviousIndex(int currentIndex)
        {
            return currentIndex - 1;
        }

        protected virtual int GetNextIndex(int currentIndex)
        {
            return currentIndex + 1;
        }
    }
}
