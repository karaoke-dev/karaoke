﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
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

        public IEnumerable<Singer> Singers => (beatmap.PlayableBeatmap as KaraokeBeatmap)?.Singers;

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

        #region Layout

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

        #region Split/combine lyric

        public void SplitLyric(Lyric lyric, int index)
        {
            // todo : make sure split wotks with order and other property.
            var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, index);

            changeHandler?.BeginChange();

            beatmap.Add(firstLyric);
            beatmap.Add(secondLyric);
            beatmap.Remove(lyric);

            changeHandler?.EndChange();
        }

        #endregion

        #region Create/delete lyric

        public void CreateLyric()
        {
            changeHandler?.BeginChange();

            var mexOrder = IHasOrdersUtils.GetMaxOrderNumber(Lyrics.ToArray());
            var createLyric = new Lyric
            {
                Text = "New lyric",
                Order = mexOrder + 1,
            };
            beatmap.Add(createLyric);

            changeHandler?.EndChange();
        }

        public void DeleteLyric(Lyric lyric)
        {
            changeHandler?.BeginChange();

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

        #region Singer

        public void AddSingerToLyric(Singer singer, Lyric lyric) => AddSingersToLyrics(new List<Singer> { singer }, new List<Lyric> { lyric });

        public void AddSingersToLyrics(List<Singer> singers, List<Lyric> lyrics)
        {
            if (!(singers?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(singers)} cannot be numm or empty.");

            if (!(lyrics?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(lyrics)} cannot be numm or empty.");

            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                foreach (var singer in singers)
                {
                    addSingerToLyric(singer, lyric);
                }
            }

            changeHandler?.EndChange();

            void addSingerToLyric(Singer singer, Lyric lyric)
            {
                if (SingerInLyric(singer, lyric))
                    return;

                var existSingerList = lyric.Singers?.ToList() ?? new List<int>();
                existSingerList.Add(singer.ID);
                lyric.Singers = existSingerList.ToArray();
            }
        }

        public void RemoveSingerToLyric(Singer singer, Lyric lyric) => RemoveSingersToLyrics(new List<Singer> { singer }, new List<Lyric> { lyric });

        public void RemoveSingersToLyrics(List<Singer> singers, List<Lyric> lyrics)
        {
            if (!(singers?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(singers)} cannot be numm or empty.");

            if (!(lyrics?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(lyrics)} cannot be numm or empty.");

            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                foreach (var singer in singers)
                {
                    removeSingerToLyric(singer, lyric);
                }
            }

            changeHandler?.EndChange();

            void removeSingerToLyric(Singer singer, Lyric lyric)
            {
                if (!SingerInLyric(singer, lyric))
                    return;

                lyric.Singers = lyric.Singers?.Where(x => x != singer.ID).ToArray();
            }
        }

        public bool SingerInLyric(Singer singer, Lyric lyric)
        {
            // lyric belongs to default singer if no any singer in lyric.
            if (lyric.Singers == null || !lyric.Singers.Any())
                return singer.ID == 0;

            return (bool)lyric.Singers?.Contains(singer.ID);
        }

        #endregion
    }
}
