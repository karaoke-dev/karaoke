// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Languages
{
    public class LanguageDetector
    {
        private readonly LanguageDetection.LanguageDetector detector = new();

        public LanguageDetector(LanguageDetectorConfig config)
        {
            var targetLanguages = config?.AcceptLanguages?.Where(x => x != null).ToList() ?? new List<CultureInfo>();

            if (targetLanguages.Any())
            {
                detector.AddLanguages(targetLanguages.Select(x => x.Name).ToArray());
            }
            else
            {
                detector.AddAllLanguages();
            }
        }

        public CultureInfo DetectLanguage(Lyric lyric)
        {
            var result = detector.DetectAll(lyric.Text);
            string languageCode = result.FirstOrDefault()?.Language;

            return languageCode == null ? null : new CultureInfo(languageCode);
        }
    }
}
