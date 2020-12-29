// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
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

        /// <summary>
        /// Will auto-detect each <see cref="Lyric"/> 's <see cref="Lyric.Language"/> and apply on them.
        /// </summary>
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

        public bool DeleteLyricText(CursorPosition position)
        {
            var lyric = position.Lyric;
            var index = TimeTagIndexUtils.ToLyricIndex(position.Index);
            if (index <= 0)
                return false;

            changeHandler?.BeginChange();

            LyricUtils.RemoveText(lyric, index - 1);

            changeHandler?.EndChange();

            return true;
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
    }
}
