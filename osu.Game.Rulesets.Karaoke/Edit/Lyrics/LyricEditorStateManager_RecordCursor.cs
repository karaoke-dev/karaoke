// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager
    {
        public bool MoveRecordCursorToTargetPosition(TimeTag timeTag)
        {
            if (timeTagInLyric(timeTag) == null)
                return false;

            moveCursorTo(timeTag);
            return true;
        }

        public bool MoveHoverRecordCursorToTargetPosition(TimeTag timeTag)
        {
            if (timeTagInLyric(timeTag) == null)
                return false;

            moveHoverCursorTo(timeTag);
            return true;
        }

        public void ClearHoverRecordCursorPosition()
        {
            BindableHoverCursorPosition.Value = new CursorPosition();
        }

        public bool RecordingCursorMovable(TimeTag timeTag)
        {
            switch (RecordingMovingCursorMode)
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

        private bool moveRecordCursor(MovingCursorAction action)
        {
            var currentTimeTag = BindableCursorPosition.Value.TimeTag;

            TimeTag nextTimeTag;

            switch (action)
            {
                case MovingCursorAction.Up:
                    nextTimeTag = getPreviousLyricTimeTag(currentTimeTag);
                    break;

                case MovingCursorAction.Down:
                    nextTimeTag = getNextLyricTimeTag(currentTimeTag);
                    break;

                case MovingCursorAction.Left:
                    nextTimeTag = getPreviousTimeTag(currentTimeTag);
                    break;

                case MovingCursorAction.Right:
                    nextTimeTag = getNextTimeTag(currentTimeTag);
                    break;

                case MovingCursorAction.First:
                    nextTimeTag = getFirstTimeTag();
                    break;

                case MovingCursorAction.Last:
                    nextTimeTag = getLastTimeTag();
                    break;

                default:
                    throw new InvalidOperationException(nameof(action));
            }

            if (nextTimeTag == null)
                return false;

            moveCursorTo(nextTimeTag);
            return true;
        }

        private Lyric timeTagInLyric(TimeTag timeTag)
        {
            if (timeTag == null)
                return null;

            return Lyrics.FirstOrDefault(x => x.TimeTags?.Contains(timeTag) ?? false);
        }

        private TimeTag getPreviousLyricTimeTag(TimeTag timeTag)
        {
            var currentLyric = timeTagInLyric(timeTag);
            return Lyrics.GetPrevious(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= timeTag.Index && RecordingCursorMovable(x));
        }

        private TimeTag getNextLyricTimeTag(TimeTag timeTag)
        {
            var currentLyric = timeTagInLyric(timeTag);
            return Lyrics.GetNext(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= timeTag.Index && RecordingCursorMovable(x));
        }

        private TimeTag getPreviousTimeTag(TimeTag timeTag)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetPreviousMatch(timeTag, RecordingCursorMovable);
        }

        private TimeTag getNextTimeTag(TimeTag timeTag)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetNextMatch(timeTag, RecordingCursorMovable);
        }

        private TimeTag getFirstTimeTag()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            return timeTags.FirstOrDefault();
        }

        private TimeTag getLastTimeTag()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            return timeTags.LastOrDefault();
        }

        private void moveCursorTo(TimeTag timeTag)
        {
            if (timeTag == null)
                return;

            var currentLyric = timeTagInLyric(timeTag);
            BindableHoverCursorPosition.Value = new CursorPosition();
            BindableCursorPosition.Value = new CursorPosition(currentLyric, timeTag);
        }

        private void moveHoverCursorTo(TimeTag timeTag)
        {
            if (timeTag == null)
                return;

            var currentLyric = timeTagInLyric(timeTag);
            BindableHoverCursorPosition.Value = new CursorPosition(currentLyric, timeTag);
        }
    }
}
