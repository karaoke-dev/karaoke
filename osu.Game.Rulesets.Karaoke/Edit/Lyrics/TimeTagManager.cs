// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    /// <summary>
    /// Handle view or edit time-tag in lyrics.
    /// Notice that <see cref="TimeTagManager"/> is not strictly needed, <see cref="LyricEditor"/> just not showing time-tag if not inject this manager.
    /// </summary>
    public class TimeTagManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(canBeNull: true)]
        private IEditorChangeHandler changeHandler { get; set; }

        [Resolved(canBeNull: true)]
        private IFrameBasedClock framedClock { get; set; }

        /// <summary>
        /// Will auto-detect each <see cref="Lyric"/> 's <see cref="Lyric.TimeTags"/> and apply on them.
        /// </summary>
        public void AutoGenerateTimeTags()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            if (!lyrics.Any())
                return;

            changeHandler?.BeginChange();

            var selector = new TimeTagGeneratorSelector();

            foreach (var lyric in lyrics)
            {
                var timeTags = selector.GenerateTimeTags(lyric);
                lyric.TimeTags = timeTags;
            }

            changeHandler?.EndChange();
        }

        public bool SetTimeTagTime(TimeTag timeTag)
        {
            if (framedClock == null)
                return false;

            var currentLyric = timeTagInLyric(timeTag);
            if (currentLyric == null)
                return false;

            changeHandler?.BeginChange();

            timeTag.Time = framedClock.CurrentTime;
            refreshTimeTag(currentLyric);

            changeHandler?.EndChange();

            currentLyric.TimeTagsBindable.TriggerChange();
            return true;
        }

        public bool ClearTimeTagTime(TimeTag timeTag)
        {
            if (framedClock == null)
                return false;

            var currentLyric = timeTagInLyric(timeTag);
            if (currentLyric == null)
                return false;

            changeHandler?.BeginChange();

            timeTag.Time = null;
            refreshTimeTag(currentLyric);

            changeHandler?.EndChange();

            return true;
        }

        public TimeTag AddTimeTag(TimeTag timeTag)
        {
            var currentLyric = timeTagInLyric(timeTag);
            if (currentLyric == null)
                return null;

            var timeTags = currentLyric.TimeTags.ToList();
            var targetIndex = timeTags.IndexOf(timeTag);
            if (targetIndex < 0)
                return null;

            var newTimeTag = new TimeTag(timeTag.Index);
            timeTags.Insert(targetIndex, newTimeTag);

            changeHandler?.BeginChange();

            currentLyric.TimeTags = timeTags.ToArray();
            sortingTimeTag(currentLyric);

            changeHandler?.EndChange();

            return newTimeTag;
        }

        public bool RemoveTimeTag(TimeTag timeTag)
        {
            var currentLyric = timeTagInLyric(timeTag);
            if (currentLyric == null)
                return false;

            changeHandler?.BeginChange();

            // delete time tag from list
            currentLyric.TimeTags = currentLyric.TimeTags.Where(x => x != timeTag).ToArray();

            changeHandler?.EndChange();

            return true;
        }

        public bool AddTimeTagByPosition(CursorPosition position)
        {
            var lyric = position.Lyric;
            var index = position.Index;
            if (!beatmap.HitObjects.Contains(lyric))
                return false;

            var timeTags = lyric.TimeTags.ToList();
            var targetTimeTag = timeTags.FirstOrDefault(x => x.Index >= index) ?? timeTags.LastOrDefault();
            if (targetTimeTag == null)
                return false;

            changeHandler?.BeginChange();

            var insertIndex = timeTags.IndexOf(targetTimeTag);
            timeTags.Insert(insertIndex, new TimeTag(index));
            lyric.TimeTags = timeTags.ToArray();

            changeHandler?.EndChange();

            return false;
        }

        public bool RemoveTimeTagByPosition(CursorPosition position)
        {
            var lyric = position.Lyric;
            var index = position.Index;
            if (!beatmap.HitObjects.Contains(lyric))
                return false;

            var timeTags = lyric.TimeTags.ToList();
            var targetTimeTag = timeTags.FirstOrDefault(x => x.Index == index);
            if (targetTimeTag == null)
                return false;

            changeHandler?.BeginChange();

            timeTags.Remove(targetTimeTag);
            lyric.TimeTags = timeTags.ToArray();

            changeHandler?.EndChange();

            return false;
        }

        public bool HasTimedTimeTags()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            return LyricsUtils.HasTimedTimeTags(lyrics);
        }

        private void refreshTimeTag(Lyric lyric)
            => lyric.TimeTags = lyric.TimeTags.ToArray();

        private void sortingTimeTag(Lyric lyric)
            => lyric.TimeTags = TimeTagsUtils.Sort(lyric.TimeTags);

        private Lyric timeTagInLyric(TimeTag timeTag)
        {
            if (timeTag == null)
                return null;

            return beatmap.HitObjects.OfType<Lyric>().FirstOrDefault(x => x.TimeTags?.Contains(timeTag) ?? false);
        }
    }

    public enum MovingCursorAction
    {
        Up,

        Down,

        Left,

        Right,

        // notice that this action only available in recording cursor mode.
        Next,

        First,

        Last,
    }
}
