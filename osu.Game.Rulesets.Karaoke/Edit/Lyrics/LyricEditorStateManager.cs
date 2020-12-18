// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorStateManager
    {
        private EditorBeatmap beatmap { get; set; }

        public Bindable<Lyric> BindableSplitLyric { get; } = new Bindable<Lyric>();
        public Bindable<TimeTagIndex> BindableSplitPosition { get; } = new Bindable<TimeTagIndex>();

        public Bindable<Mode> BindableMode { get; } = new Bindable<Mode>();

        public Bindable<LyricFastEditMode> BindableFastEditMode { get; } = new Bindable<LyricFastEditMode>();

        public Mode Mode => BindableMode.Value;

        public LyricFastEditMode FastEditMode => BindableFastEditMode.Value;

        public Bindable<TimeTag> BindableCursorPosition { get; } = new Bindable<TimeTag>();

        public LyricEditorStateManager(EditorBeatmap beatmap)
        {
            this.beatmap = beatmap;
        }

        #region Cursor position by time-tag

        public void UpdateSplitCursorPosition(Lyric lyric, TimeTagIndex index)
        {
            BindableSplitLyric.Value = lyric;
            BindableSplitPosition.Value = index;
        }

        public void ClearSplitCursorPosition()
        {
            BindableSplitLyric.Value = null;
        }

        #endregion

        #region Cursor position by lyric

        public bool MoveCursor(CursorAction action)
        {
            var currentTimeTag = BindableCursorPosition.Value;

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
                    nextTimeTag = getFirstTimeTag(currentTimeTag);
                    break;

                case CursorAction.Last:
                    nextTimeTag = getLastTimeTag(currentTimeTag);
                    break;
            }

            if (nextTimeTag == null)
                return false;

            moveCursorTo(nextTimeTag);
            return true;
        }

        public bool MoveCursorToTargetPosition(TimeTag timeTag)
        {
            if (timeTagInLyric(timeTag) == null)
                return false;

            moveCursorTo(timeTag);
            return true;
        }

        private Lyric timeTagInLyric(TimeTag timeTag)
        {
            if (timeTag == null)
                return null;

            return beatmap.HitObjects.OfType<Lyric>().FirstOrDefault(x => x.TimeTags?.Contains(timeTag) ?? false);
        }

        private TimeTag getPreviousLyricTimeTag(TimeTag timeTag)
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            var currentLyric = timeTagInLyric(timeTag);
            return lyrics.GetPrevious(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= timeTag.Index);
        }

        private TimeTag getNextLyricTimeTag(TimeTag timeTag)
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            var currentLyric = timeTagInLyric(timeTag);
            return lyrics.GetNext(currentLyric)?.TimeTags?.FirstOrDefault(x => x.Index >= timeTag.Index);
        }

        private TimeTag getPreviousTimeTag(TimeTag timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetPrevious(timeTag);
        }

        private TimeTag getNextTimeTag(TimeTag timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetNext(timeTag);
        }

        private TimeTag getFirstTimeTag(TimeTag timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.FirstOrDefault();
        }

        private TimeTag getLastTimeTag(TimeTag timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.LastOrDefault();
        }

        private void moveCursorTo(TimeTag timeTag)
        {
            if (timeTag == null)
                return;

            BindableCursorPosition.Value = timeTag;
        }

        #endregion

        #region Mode

        public void SetMode(Mode mode)
        {
            BindableMode.Value = mode;
        }

        public void SetFastEditMode(LyricFastEditMode fastEditMode)
        {
            BindableFastEditMode.Value = fastEditMode;
        }

        #endregion
    }

    public enum Mode
    {
        /// <summary>
        /// Cannot edit anything except each lyric's left-side part.
        /// </summary>
        ViewMode,

        /// <summary>
        /// Can create/delete/mode/split/combine lyric.
        /// </summary>
        EditMode,

        /// <summary>
        /// Click white-space to set current time into time-tag.
        /// </summary>
        RecordMode,

        /// <summary>
        /// Enable to create/delete and reset time tag.
        /// </summary>
        TimeTagEditMode
    }

    public enum LyricFastEditMode
    {
        /// <summary>
        /// User can only see start and end time.
        /// </summary>
        None,

        /// <summary>
        /// Can edit each lyric's layout.
        /// </summary>
        Layout,

        /// <summary>
        /// Can edit each lyric's singer.
        /// </summary>
        Singer,

        /// <summary>
        /// Can edit each lyric's language.
        /// </summary>
        Language,
    }
}
