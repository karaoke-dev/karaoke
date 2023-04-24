// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public partial class LyricLanguageChangeHandler : LyricPropertyChangeHandler, ILyricLanguageChangeHandler
    {
        #region Auto-Generate

        public bool CanGenerate()
        {
            var detector = GetDetector<CultureInfo, LanguageDetectorConfig>();
            return CanDetect(detector);
        }

        public IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics()
        {
            var detector = GetDetector<CultureInfo, LanguageDetectorConfig>();
            return GetInvalidMessageFromDetector(detector);
        }

        public void AutoGenerate()
        {
            var detector = GetDetector<CultureInfo, LanguageDetectorConfig>();

            PerformOnSelection(lyric =>
            {
                var detectedLanguage = detector.Detect(lyric);
                lyric.Language = detectedLanguage;
            });
        }

        #endregion

        public void SetLanguage(CultureInfo? language)
        {
            PerformOnSelection(lyric =>
            {
                if (EqualityComparer<CultureInfo?>.Default.Equals(language, lyric.Language))
                    return;

                lyric.Language = language;
            });
        }

        protected override bool IsWritePropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.Language));
    }
}
