// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Languages
{
    public class LanguageDetector
    {
        private readonly LanguageDetection.LanguageDetector detector = new LanguageDetection.LanguageDetector();

        public LanguageDetector(LanguageDetectorConfig config)
        {
            var targetLanguages = config?.AcceptLanguage?.Where(x => x != null).ToList() ?? new List<CultureInfo>();
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
            var languageCode = result.FirstOrDefault()?.Language;

            if (languageCode == null)
                return null;

            // make some language conversion here
            switch (languageCode)
            {
                // todo : need to think about if this is needed?
                case "en":
                    return new CultureInfo("en-US");
                default:
                    return new CultureInfo(languageCode);
            }
        }
    }
}
