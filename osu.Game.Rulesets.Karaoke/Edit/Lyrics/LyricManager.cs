// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
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
