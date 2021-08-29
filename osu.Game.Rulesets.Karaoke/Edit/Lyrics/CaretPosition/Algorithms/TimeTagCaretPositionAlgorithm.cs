// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class TimeTagCaretPositionAlgorithm : CaretPositionAlgorithm<TimeTagCaretPosition>
    {
        public MovingTimeTagCaretMode Mode { get; set; }

        public TimeTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(TimeTagCaretPosition position)
        {
            return timeTagMovable(position.TimeTag);
        }

        public override TimeTagCaretPosition MoveUp(TimeTagCaretPosition currentPosition)
        {
            var currentTimeTag = currentPosition.TimeTag;

            // need to check is lyric in time-tag is valid.
            var currentLyric = timeTagInLyric(currentTimeTag);
            if (currentLyric != currentPosition.Lyric)
                throw new ArgumentException(nameof(currentPosition.Lyric));

            var upTimeTag = Lyrics.GetPreviousMatch(currentLyric, l => l.TimeTags?.Any() ?? false)
                                  ?.TimeTags.FirstOrDefault(x => x.Index >= currentTimeTag.Index && timeTagMovable(x));
            return timeTagToPosition(upTimeTag);
        }

        public override TimeTagCaretPosition MoveDown(TimeTagCaretPosition currentPosition)
        {
            var currentTimeTag = currentPosition.TimeTag;

            // need to check is lyric in time-tag is valid.
            var currentLyric = timeTagInLyric(currentTimeTag);
            if (currentLyric != currentPosition.Lyric)
                throw new ArgumentException(nameof(currentPosition.Lyric));

            var downTimeTag = Lyrics.GetNextMatch(currentLyric, l => l.TimeTags?.Any() ?? false)
                                    ?.TimeTags?.FirstOrDefault(x => x.Index >= currentTimeTag.Index && timeTagMovable(x));
            return timeTagToPosition(downTimeTag);
        }

        public override TimeTagCaretPosition MoveLeft(TimeTagCaretPosition currentPosition)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags ?? Array.Empty<TimeTag>()).ToArray();
            var previousTimeTag = timeTags.GetPreviousMatch(currentPosition.TimeTag, timeTagMovable);
            return timeTagToPosition(previousTimeTag);
        }

        public override TimeTagCaretPosition MoveRight(TimeTagCaretPosition currentPosition)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags ?? Array.Empty<TimeTag>()).ToArray();
            var nextTimeTag = timeTags.GetNextMatch(currentPosition.TimeTag, timeTagMovable);
            return timeTagToPosition(nextTimeTag);
        }

        public override TimeTagCaretPosition MoveToFirst()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags ?? Array.Empty<TimeTag>()).ToArray();
            var firstTimeTag = timeTags.FirstOrDefault(timeTagMovable);
            return timeTagToPosition(firstTimeTag);
        }

        public override TimeTagCaretPosition MoveToLast()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags ?? Array.Empty<TimeTag>()).ToArray();
            var lastTag = timeTags.LastOrDefault(timeTagMovable);
            return timeTagToPosition(lastTag);
        }

        public override TimeTagCaretPosition MoveToTarget(Lyric lyric)
        {
            var targetTimeTag = lyric.TimeTags?.FirstOrDefault(timeTagMovable);
            return new TimeTagCaretPosition(lyric, targetTimeTag);
        }

        private TimeTagCaretPosition timeTagToPosition(TimeTag timeTag)
        {
            if (timeTag == null)
                return null;

            return new TimeTagCaretPosition(timeTagInLyric(timeTag), timeTag);
        }

        private Lyric timeTagInLyric(TimeTag timeTag)
        {
            if (timeTag == null)
                return null;

            return Lyrics.FirstOrDefault(x => x.TimeTags?.Contains(timeTag) ?? false);
        }

        private bool timeTagMovable(TimeTag timeTag)
        {
            if (timeTag == null)
                return false;

            return Mode switch
            {
                MovingTimeTagCaretMode.None => true,
                MovingTimeTagCaretMode.OnlyStartTag => timeTag.Index.State == TextIndex.IndexState.Start,
                MovingTimeTagCaretMode.OnlyEndTag => timeTag.Index.State == TextIndex.IndexState.End,
                _ => throw new InvalidOperationException(nameof(MovingTimeTagCaretMode))
            };
        }
    }
}
