// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class TimeTagIndexCaretPositionAlgorithm : CaretPositionAlgorithm<TimeTagIndexCaretPosition>
    {
        public TimeTagIndexCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(TimeTagIndexCaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (TextIndexUtils.OutOfRange(position.Index, position.Lyric.Text))
                return false;

            return true;
        }

        public override TimeTagIndexCaretPosition MoveUp(TimeTagIndexCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = currentPosition.Index.State;

            return new TimeTagIndexCaretPosition(lyric, new TextIndex(index, state));
        }

        public override TimeTagIndexCaretPosition MoveDown(TimeTagIndexCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = currentPosition.Index.State;

            return new TimeTagIndexCaretPosition(lyric, new TextIndex(index, state));
        }

        public override TimeTagIndexCaretPosition MoveLeft(TimeTagIndexCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var previousIndex = GetPreviousIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(previousIndex, lyric?.Text))
                return MoveUp(new TimeTagIndexCaretPosition(currentPosition.Lyric, new TextIndex(int.MaxValue, TextIndex.IndexState.End)));

            return new TimeTagIndexCaretPosition(currentPosition.Lyric, previousIndex);
        }

        public override TimeTagIndexCaretPosition MoveRight(TimeTagIndexCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var nextIndex = GetNextIndex(currentPosition.Index);

            if (TextIndexUtils.OutOfRange(nextIndex, lyric?.Text))
                return MoveDown(new TimeTagIndexCaretPosition(currentPosition.Lyric, new TextIndex(int.MinValue)));

            return new TimeTagIndexCaretPosition(currentPosition.Lyric, nextIndex);
        }

        public override TimeTagIndexCaretPosition MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault(l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var index = new TextIndex();
            return new TimeTagIndexCaretPosition(lyric, index);
        }

        public override TimeTagIndexCaretPosition MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault(l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var textLength = lyric?.Text.Length ?? 0;
            var index = new TextIndex(textLength - 1, TextIndex.IndexState.End);
            return new TimeTagIndexCaretPosition(lyric, index);
        }

        public override TimeTagIndexCaretPosition MoveToTarget(Lyric lyric)
        {
            var index = new TextIndex();
            return new TimeTagIndexCaretPosition(lyric, index);
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
