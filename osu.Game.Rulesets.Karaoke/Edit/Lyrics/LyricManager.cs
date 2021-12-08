﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
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

        protected IEnumerable<Lyric> Lyrics => beatmap.HitObjects.OfType<Lyric>().OrderBy(x => x.Order);

        public IEnumerable<Singer> Singers => (beatmap.PlayableBeatmap as KaraokeBeatmap)?.Singers;

        #region Language

        public void AutoDetectLyricLanguage()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToArray();

            AutoDetectLyricLanguage(lyrics);
        }

        public void AutoDetectLyricLanguage(Lyric[] lyrics)
        {
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
            if (EqualityComparer<CultureInfo>.Default.Equals(language, lyric.Language))
                return false;

            changeHandler?.BeginChange();

            lyric.Language = language;

            changeHandler?.EndChange();

            return true;
        }

        #endregion

        #region Layout

        public void ChangeLayout(List<Lyric> lyrics, int layout)
        {
            changeHandler?.BeginChange();

            lyrics.ForEach(l => l.LayoutIndex = layout);

            changeHandler?.EndChange();
        }

        #endregion

        #region Text

        public bool DeleteLyricText(Lyric lyric, int index)
        {
            if (index <= 0)
                return false;

            changeHandler?.BeginChange();

            LyricUtils.RemoveText(lyric, index - 1);

            if (string.IsNullOrEmpty(lyric.Text))
            {
                OrderUtils.ShiftingOrder(Lyrics.Where(x => x.Order > lyric.Order).ToArray(), -1);
                beatmap.Remove(lyric);
            }

            changeHandler?.EndChange();

            return true;
        }

        #endregion

        #region Order

        public void ChangeLyricOrder(Lyric lyric, int newIndex)
        {
            var oldOrder = lyric.Order;
            var newOrder = newIndex + 1; // order is start from 1
            OrderUtils.ChangeOrder(Lyrics.ToArray(), oldOrder, newOrder, (switchSinger, oldOrder, newOrder) =>
            {
                // todo : not really sure should call update?
            });
        }

        #endregion

        #region TimeTag

        public void AutoGenerateTimeTags()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToArray();
            AutoGenerateTimeTags(lyrics);
        }

        public void AutoGenerateTimeTags(Lyric[] lyrics)
        {
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

        public bool AddTimeTagByPosition(Lyric lyric, TextIndex index)
        {
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

        public bool RemoveTimeTagByPosition(Lyric lyric, TextIndex index)
        {
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

        #region Singer

        public void AddSingerToLyric(Singer singer, Lyric lyric) => AddSingersToLyrics(new List<Singer> { singer }, new List<Lyric> { lyric });

        public void AddSingersToLyrics(List<Singer> singers, List<Lyric> lyrics)
        {
            if (!(singers?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(singers)} cannot be null or empty.");

            if (!(lyrics?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(lyrics)} cannot be null or empty.");

            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                foreach (var singer in singers)
                {
                    LyricUtils.AddSinger(lyric, singer);
                }
            }

            changeHandler?.EndChange();
        }

        public void RemoveSingerToLyric(Singer singer, Lyric lyric) => RemoveSingersToLyrics(new List<Singer> { singer }, new List<Lyric> { lyric });

        public void RemoveSingersToLyrics(List<Singer> singers, List<Lyric> lyrics)
        {
            if (!(singers?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(singers)} cannot be null or empty.");

            if (!(lyrics?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(lyrics)} cannot be null or empty.");

            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                foreach (var singer in singers)
                {
                    LyricUtils.RemoveSinger(lyric, singer);
                }
            }

            changeHandler?.EndChange();
        }

        public void ClearAllSingersFromLyric(Lyric lyric)
        {
            if (!lyric.Singers.Any())
                return;

            var matchedSingers = Singers.Where(x => lyric.Singers.Contains(x.ID)).ToList();
            RemoveSingersToLyrics(matchedSingers, new List<Lyric> { lyric });
        }

        #endregion

        #region Lock

        public virtual void LockLyric(Lyric lyric, LockState lockState)
            => LockLyrics(new List<Lyric> { lyric }, lockState);

        public void LockLyrics(List<Lyric> lyrics, LockState lockState)
        {
            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                lyric.Lock = lockState;
            }

            changeHandler?.EndChange();
        }

        public void UnlockLyric(Lyric lyric)
            => UnlockLyrics(new List<Lyric> { lyric });

        public void UnlockLyrics(List<Lyric> lyrics)
            => LockLyrics(lyrics, LockState.None);

        #endregion
    }
}
