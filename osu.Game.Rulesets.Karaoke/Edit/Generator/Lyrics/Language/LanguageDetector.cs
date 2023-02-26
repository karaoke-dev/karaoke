// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language
{
    public class LanguageDetector : LyricPropertyDetector<CultureInfo?, LanguageDetectorConfig>
    {
        private readonly LanguageDetection.LanguageDetector detector = new();

        public LanguageDetector(LanguageDetectorConfig config)
            : base(config)
        {
            var targetLanguages = config.AcceptLanguages.ToList();

            if (targetLanguages.Any())
            {
                detector.AddLanguages(targetLanguages.Select(x => x.Name).ToArray());
            }
            else
            {
                detector.AddAllLanguages();
            }
        }

        protected override LocalisableString? GetInvalidMessageFromItem(Lyric item)
        {
            if (string.IsNullOrWhiteSpace(item.Text))
                return "Lyric should not be empty.";

            return null;
        }

        protected override CultureInfo? DetectFromItem(Lyric item)
        {
            var result = detector.DetectAll(item.Text);
            string? languageCode = result.FirstOrDefault()?.Language;

            return languageCode == null ? null : new CultureInfo(languageCode);
        }
    }
}
