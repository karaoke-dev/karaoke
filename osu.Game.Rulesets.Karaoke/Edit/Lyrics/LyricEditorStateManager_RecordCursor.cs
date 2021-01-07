// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager
    {
        public Bindable<TimeTag> BindableHoverRecordCursorPosition { get; } = new Bindable<TimeTag>();

        public Bindable<TimeTag> BindableRecordCursorPosition { get; } = new Bindable<TimeTag>();

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

        private bool moveRecordCursor(CursorAction action)
        {
            var currentTimeTag = BindableRecordCursorPosition.Value;

            TimeTag nextTimeTag = null;

            switch (action)
            {
                case CursorAction.MoveUp:
                    nextTimeTag = getPreviousLyricTimeTag(currentTimeTag);
                    break;

                case CursorAction.MoveDown:
                    nextTimeTag = getNextLyricTimeTag(currentTimeTag);
                    break;

                case CursorAction.MoveLeft:
                    nextTimeTag = getPreviousTimeTag(currentTimeTag);
                    break;

                case CursorAction.MoveRight:
                    nextTimeTag = getNextTimeTag(currentTimeTag);
                    break;

                case CursorAction.First:
                    nextTimeTag = getFirstTimeTag();
                    break;

                case CursorAction.Last:
                    nextTimeTag = getLastTimeTag();
                    break;
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
