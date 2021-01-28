// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        protected IEnumerable<Lyric> Lyrics => beatmap.HitObjects.OfType<Lyric>();

        #region Language

        public void AutoDetectLyricLanguage()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            if (!lyrics.Any())
                return;

            // todo : should get the config from setting.
            var config = new LanguageDetectorConfig();
            var detector = new LanguageDetector(config);

            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                var detectedLanguage = detector.DetectLanguage(lyric);
                lyric.Language = detectedLanguage;
            }

            changeHandler?.EndChange();
        }

        public bool SetLanguage(Lyric lyric, CultureInfo language)
        {
            if (lyric.Language.Equals(language))
                return false;

            changeHandler?.BeginChange();

            lyric.Language = language;

            changeHandler?.EndChange();

            return true;
        }

        #endregion

        #region Lyout

        public void ChangeLayout(List<Lyric> lyrics, int layout)
        {
            changeHandler?.BeginChange();

            lyrics.ForEach(l => l.LayoutIndex = layout);

            changeHandler?.EndChange();
        }

        #endregion

        #region Text

        public bool DeleteLyricText(CursorPosition position)
        {
            var lyric = position.Lyric;
            var index = TextIndexUtils.ToLyricIndex(position.Index);
            if (index <= 0)
                return false;

            changeHandler?.BeginChange();

            LyricUtils.RemoveText(lyric, index - 1);

            changeHandler?.EndChange();

            return true;
        }

        #endregion

        #region Split/combine

        public void SplitLyric(Lyric lyric, int index)
        {
            // todo: need to got reason why cause null object issue.
            return;

            // todo : implement split.
            var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, index);

            changeHandler?.BeginChange();

            beatmap.Add(firstLyric);
            beatmap.Add(secondLyric);
            beatmap.Remove(lyric);

            changeHandler?.EndChange();
        }

        #endregion

        #region Order

        public void ChangeLyricOrder(Lyric lyric, int newIndex)
        {
            var oldOrder = lyric.Order;
            var newOrder = newIndex + 1; // order is start from 1
            IHasOrdersUtils.ChangeOrder(Lyrics.ToArray(), oldOrder, newOrder, (switchSinger, oldOrder, newOrder) =>
            {
                // todo : not really sure should call update?
            });
        }

        #endregion

        #region TimeTag

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

        public bool SetTimeTagTime(TimeTag timeTag, double time)
        {
            var currentLyric = timeTagInLyric(timeTag);
            if (currentLyric == null)
                return false;

            changeHandler?.BeginChange();

            timeTag.Time = time;
            refreshTimeTag(currentLyric);

            changeHandler?.EndChange();

            currentLyric.TimeTagsBindable.TriggerChange();
            return true;
        }

        public bool ClearTimeTagTime(TimeTag timeTag)
        {
            if (timeTag.Time == null)
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

        #endregion
    }
}
