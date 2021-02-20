// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public class RecordingCursorPositionAlgorithm : CursorPositionAlgorithm, ICursorPositionAlgorithm
    {
        private readonly RecordingMovingCursorMode mode;

        public RecordingCursorPositionAlgorithm(Lyric[] lyrics, RecordingMovingCursorMode mode)
            : base(lyrics)
        {
            this.mode = mode;
        }

        public bool PositionMovable(CursorPosition position)
        {
            return timeTagMovable(position.TimeTag);
        }

        public CursorPosition? MoveUp(CursorPosition currentPosition)
        {
            var currentTimeTag = currentPosition.TimeTag;

            // need to check is lyric in time-tag is valid.
            var currentLyric = timeTagInLyric(currentTimeTag);
            if (currentLyric != currentPosition.Lyric)
                throw new ArgumentException(nameof(currentPosition.Lyric));

            var upTimeTag = Lyrics.GetPrevious(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= currentTimeTag.Index && timeTagMovable(x));
            return timeTagToPosition(upTimeTag);
        }

        public CursorPosition? MoveDown(CursorPosition currentPosition)
        {
            var currentTimeTag = currentPosition.TimeTag;

            // need to check is lyric in time-tag is valid.
            var currentLyric = timeTagInLyric(currentTimeTag);
            if (currentLyric != currentPosition.Lyric)
                throw new ArgumentException(nameof(currentPosition.Lyric));

            var downTimeTag = Lyrics.GetNext(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= currentTimeTag.Index && timeTagMovable(x));
            return timeTagToPosition(downTimeTag);
        }

        public CursorPosition? MoveLeft(CursorPosition currentPosition)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var previousTimeTag = timeTags.GetPreviousMatch(currentPosition.TimeTag, timeTagMovable);
            return timeTagToPosition(previousTimeTag);
        }

        public CursorPosition? MoveRight(CursorPosition currentPosition)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var nextTimeTag = timeTags.GetNextMatch(currentPosition.TimeTag, timeTagMovable);
            return timeTagToPosition(nextTimeTag);
        }

        public CursorPosition? MoveToFirst()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var firstTimeTag = timeTags.FirstOrDefault();
            return timeTagToPosition(firstTimeTag);
        }

        public CursorPosition? MoveToLast()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var lastTag = timeTags.LastOrDefault();
            return timeTagToPosition(lastTag);
        }

        private CursorPosition? timeTagToPosition(TimeTag timeTag)
        {
            if (timeTag == null)
                return null;

            return new CursorPosition(timeTagInLyric(timeTag), timeTag);
        }

        private Lyric timeTagInLyric(TimeTag timeTag)
        {
            if (timeTag == null)
                return null;

            return Lyrics.FirstOrDefault(x => x.TimeTags?.Contains(timeTag) ?? false);
        }

        private bool timeTagMovable(TimeTag timeTag)
        {
            switch (mode)
            {
                case RecordingMovingCursorMode.None:
                    return true;

                case RecordingMovingCursorMode.OnlyStartTag:
                    return timeTag.Index.State == TextIndex.IndexState.Start;

                case RecordingMovingCursorMode.OnlyEndTag:
                    return timeTag.Index.State == TextIndex.IndexState.End;

                default:
                    throw new InvalidOperationException(nameof(RecordingMovingCursorMode));
            }
        }
    }
}
