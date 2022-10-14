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
    public class TimeTagIndexCaretPositionAlgorithm : IndexCaretPositionAlgorithm<TimeTagIndexCaretPosition>
    {
        public MovingTimeTagCaretMode Mode { get; set; }

        public TimeTagIndexCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override bool PositionMovable(TimeTagIndexCaretPosition position)
        {
            if (TextIndexUtils.OutOfRange(position.Index, position.Lyric.Text))
                return false;

            var textIndex = position.Index;
            return textIndexMovable(textIndex);
        }

        protected override TimeTagIndexCaretPosition? MoveToPreviousLyric(TimeTagIndexCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            int lyricTextLength = lyric.Text.Length;
            int index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = suitableState(currentPosition.Index);

            return new TimeTagIndexCaretPosition(lyric, new TextIndex(index, state));
        }

        protected override TimeTagIndexCaretPosition? MoveToNextLyric(TimeTagIndexCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            int lyricTextLength = lyric.Text.Length;
            int index = Math.Clamp(currentPosition.Index.Index, 0, lyricTextLength - 1);
            var state = suitableState(currentPosition.Index);

            return new TimeTagIndexCaretPosition(lyric, new TextIndex(index, state));
        }

        protected override TimeTagIndexCaretPosition? MoveToFirstLyric()
        {
            var lyric = Lyrics.FirstOrDefault(l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            var index = new TextIndex(0, suitableState(TextIndex.IndexState.Start));
            return new TimeTagIndexCaretPosition(lyric, index);
        }

        protected override TimeTagIndexCaretPosition? MoveToLastLyric()
        {
            var lyric = Lyrics.LastOrDefault(l => !string.IsNullOrEmpty(l.Text));
            if (lyric == null)
                return null;

            int textLength = lyric.Text.Length;
            var index = new TextIndex(textLength - 1, suitableState(TextIndex.IndexState.End));
            return new TimeTagIndexCaretPosition(lyric, index);
        }

        protected override TimeTagIndexCaretPosition? MoveToTargetLyric(Lyric lyric)
        {
            var index = new TextIndex(0, suitableState(TextIndex.IndexState.Start));
            return new TimeTagIndexCaretPosition(lyric, index, CaretGenerateType.TargetLyric);
        }

        protected override TimeTagIndexCaretPosition? MoveToPreviousIndex(TimeTagIndexCaretPosition currentPosition)
        {
            // get previous caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var index = TextIndexUtils.GetPreviousIndex(currentPosition.Index);

            if (!textIndexMovable(index))
                return MoveToPreviousIndex(new TimeTagIndexCaretPosition(lyric, index));

            if (TextIndexUtils.OutOfRange(index, lyric.Text))
                return MoveToPreviousLyric(new TimeTagIndexCaretPosition(lyric, new TextIndex(int.MaxValue, index.State)));

            return new TimeTagIndexCaretPosition(lyric, index);
        }

        protected override TimeTagIndexCaretPosition? MoveToNextIndex(TimeTagIndexCaretPosition currentPosition)
        {
            // get next caret and make a check is need to change line.
            var lyric = currentPosition.Lyric;
            var index = TextIndexUtils.GetNextIndex(currentPosition.Index);

            if (!textIndexMovable(index))
                return MoveToNextIndex(new TimeTagIndexCaretPosition(lyric, index));

            if (TextIndexUtils.OutOfRange(index, lyric.Text))
                return MoveToNextLyric(new TimeTagIndexCaretPosition(lyric, new TextIndex(int.MinValue, index.State)));

            return new TimeTagIndexCaretPosition(lyric, index);
        }

        private bool textIndexMovable(TextIndex textIndex)
            => suitableState(textIndex) == textIndex.State;

        private TextIndex.IndexState suitableState(TextIndex textIndex)
            => suitableState(textIndex.State);

        private TextIndex.IndexState suitableState(TextIndex.IndexState state) =>
            Mode switch
            {
                MovingTimeTagCaretMode.None => state,
                MovingTimeTagCaretMode.OnlyStartTag => TextIndex.IndexState.Start,
                MovingTimeTagCaretMode.OnlyEndTag => TextIndex.IndexState.End,
                _ => throw new InvalidOperationException(nameof(MovingTimeTagCaretMode))
            };
    }
}
