// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    /// <summary>
    /// Handle view or edit time-tag in lyrics.
    /// Notice that <see cref="TimeTagManager"/> is not strictly needed, <see cref="LyricEditor"/> just not showing time-tag if not regist this manager.
    /// </summary>
    public class TimeTagManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(canBeNull: true)]
        private IEditorChangeHandler changeHandler { get; set; }

        [Resolved(canBeNull: true)]
        private IFrameBasedClock framedClock { get; set; }

        public Bindable<TimeTag> BindableCursorPosition { get; } = new Bindable<TimeTag>();

        #region Edit Time Tag

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

        private void refreshTimeTag(Lyric lyric)
            => lyric.TimeTags = lyric.TimeTags.ToArray();

        private void sortingTimeTag(Lyric lyric)
            => lyric.TimeTags = TimeTagsUtils.Sort(lyric.TimeTags);

        #endregion

        #region Time Tag cursor

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

        public class TimeTagGeneratorSelector
        {
            private readonly Lazy<JaTimeTagGenerator> jaTimeTagGenerator;
            private readonly Lazy<ZhTimeTagGenerator> zhTimeTagGenerator;

            public TimeTagGeneratorSelector()
            {
                jaTimeTagGenerator = new Lazy<JaTimeTagGenerator>(() =>
                {
                    // todo : get config from setting.
                    var config = new JaTimeTagGeneratorConfig();
                    return new JaTimeTagGenerator(config);
                });
                zhTimeTagGenerator = new Lazy<ZhTimeTagGenerator>(() =>
                {
                    // todo : get config from setting.
                    var config = new ZhTimeTagGeneratorConfig();
                    return new ZhTimeTagGenerator(config);
                });
            }

            public TimeTag[] GenerateTimeTags(Lyric lyric)
            {
                // lazy to generate language detector and apply it's setting
                switch (lyric.Language?.LCID)
                {
                    case 17:
                    case 1041:
                        return jaTimeTagGenerator.Value.CreateTimeTags(lyric);

                    case 1028:
                        return zhTimeTagGenerator.Value.CreateTimeTags(lyric);

                    default:
                        return null;
                }
            }
        }

        #endregion
    }

    public enum CursorAction
    {
        MoveUp,

        MoveDown,

        MoveLeft,

        MoveRight,

        First,

        Last,
    }
}
