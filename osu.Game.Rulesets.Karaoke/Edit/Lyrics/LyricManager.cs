// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        public Bindable<Lyric> BindableSplitLyric { get; } = new Bindable<Lyric>();
        public Bindable<int> BindableSplitPosition { get; } = new Bindable<int>();

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

        public void UpdateSplitCursorPosition(Lyric lyric, int index)
        {
            BindableSplitLyric.Value = lyric;
            BindableSplitPosition.Value = index;
        }

        public void ClearSplitCursorPosition()
        {
            BindableSplitLyric.Value = null;
        }

        public void SplitLyric(Lyric lyric, int index)
        {
            // todo : implement split.
        }
    }
}
