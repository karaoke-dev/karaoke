// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using System.Globalization;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Languages
{
    public class LanguageDetector
    {
        public CultureInfo DetectLanguage(Lyric lyric)
        {
            var detector = new LanguageDetection.LanguageDetector();
            detector.AddAllLanguages();
            var result = detector.DetectAll(lyric.Text);
            var languageCode = result.FirstOrDefault()?.Language;

            switch (languageCode)
            {
                case "zho":
                    return new CultureInfo("zh-CN");
                case "eng":
                    return new CultureInfo("en-US");
                default:
                    return null;
            }
        }
    }
}
