// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager
    {
        public Bindable<RecordingMovingCursorMode> BindableRecordingMovingCursorMode { get; } = new Bindable<RecordingMovingCursorMode>();

        public RecordingMovingCursorMode RecordingMovingCursorMode => BindableRecordingMovingCursorMode.Value;

        public Bindable<TimeTag> BindableHoverRecordCursorPosition { get; } = new Bindable<TimeTag>();

        public Bindable<TimeTag> BindableRecordCursorPosition { get; } = new Bindable<TimeTag>();

        public void SetRecordingMovingCursorMode(RecordingMovingCursorMode mode)
        {
            BindableRecordingMovingCursorMode.Value = mode;

            // todo : might move cursor to valid position.
        }

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
            BindableHoverRecordCursorPosition.Value = null;
        }

        private bool moveRecordCursor(MovingCursorAction action)
        {
            var currentTimeTag = BindableRecordCursorPosition.Value;

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
            return Lyrics.GetPrevious(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= timeTag.Index);
        }

        private TimeTag getNextLyricTimeTag(TimeTag timeTag)
        {
            var currentLyric = timeTagInLyric(timeTag);
            return Lyrics.GetNext(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= timeTag.Index);
        }

        private TimeTag getPreviousTimeTag(TimeTag timeTag)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetPrevious(timeTag);
        }

        private TimeTag getNextTimeTag(TimeTag timeTag)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetNext(timeTag);
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

            BindableHoverRecordCursorPosition.Value = null;
            BindableRecordCursorPosition.Value = timeTag;
        }

        private void moveHoverCursorTo(TimeTag timeTag)
        {
            if (timeTag == null)
                return;

            BindableHoverRecordCursorPosition.Value = timeTag;
        }
    }
}
