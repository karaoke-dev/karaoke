﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
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
        public MovingTimeTagCaretMode Mode { get; set; }

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

            var textIndex = position.Index;
            return textIndexMovable(textIndex);
        }

        public override TimeTagIndexCaretPosition MoveUp(TimeTagIndexCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = suitableState(currentPosition.Index);

            return new TimeTagIndexCaretPosition(lyric, new TextIndex(index, state));
        }

        public override TimeTagIndexCaretPosition MoveDown(TimeTagIndexCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = suitableState(currentPosition.Index);

            return new TimeTagIndexCaretPosition(lyric, new TextIndex(index, state));
        }

        public override TimeTagIndexCaretPosition MoveLeft(TimeTagIndexCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var index = getPreviousIndex(currentPosition.Index);

            if (!textIndexMovable(index))
                return MoveLeft(new TimeTagIndexCaretPosition(currentPosition.Lyric, index));

            if (TextIndexUtils.OutOfRange(index, lyric?.Text))
                return MoveUp(new TimeTagIndexCaretPosition(currentPosition.Lyric, new TextIndex(int.MaxValue, index.State)));

            return new TimeTagIndexCaretPosition(currentPosition.Lyric, index);

            static TextIndex getPreviousIndex(TextIndex currentIndex)
            {
                var previousIndex = TextIndexUtils.ToStringIndex(currentIndex) - 1;
                var previousState = TextIndexUtils.ReverseState(currentIndex.State);
                return new TextIndex(previousIndex, previousState);
            }
        }

        public override TimeTagIndexCaretPosition MoveRight(TimeTagIndexCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var index = getNextIndex(currentPosition.Index);

            if (!textIndexMovable(index))
                return MoveRight(new TimeTagIndexCaretPosition(currentPosition.Lyric, index));

            if (TextIndexUtils.OutOfRange(index, lyric?.Text))
                return MoveDown(new TimeTagIndexCaretPosition(currentPosition.Lyric, new TextIndex(int.MinValue, index.State)));

            return new TimeTagIndexCaretPosition(currentPosition.Lyric, index);

            static TextIndex getNextIndex(TextIndex currentIndex)
            {
                var nextIndex = TextIndexUtils.ToStringIndex(currentIndex);
                var nextState = TextIndexUtils.ReverseState(currentIndex.State);
                return new TextIndex(nextIndex, nextState);
            }
        }

        public override TimeTagIndexCaretPosition MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault(l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var index = new TextIndex(0, suitableState(TextIndex.IndexState.Start));
            return new TimeTagIndexCaretPosition(lyric, index);
        }

        public override TimeTagIndexCaretPosition MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault(l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var textLength = lyric?.Text.Length ?? 0;
            var index = new TextIndex(textLength - 1, suitableState(TextIndex.IndexState.End));
            return new TimeTagIndexCaretPosition(lyric, index);
        }

        public override TimeTagIndexCaretPosition MoveToTarget(Lyric lyric)
        {
            var index = new TextIndex(0, suitableState(TextIndex.IndexState.Start));
            return new TimeTagIndexCaretPosition(lyric, index);
        }

        private bool textIndexMovable(TextIndex textIndex)
            => suitableState(textIndex) == textIndex.State;

        private TextIndex.IndexState suitableState(TextIndex textIndex)
            => suitableState(textIndex.State);

        private TextIndex.IndexState suitableState(TextIndex.IndexState state)
        {
            switch (Mode)
            {
                case MovingTimeTagCaretMode.None:
                    return state;

                case MovingTimeTagCaretMode.OnlyStartTag:
                    return TextIndex.IndexState.Start;

                case MovingTimeTagCaretMode.OnlyEndTag:
                    return TextIndex.IndexState.End;

                default:
                    throw new InvalidOperationException(nameof(MovingTimeTagCaretMode));
            }
        }
    }
}
