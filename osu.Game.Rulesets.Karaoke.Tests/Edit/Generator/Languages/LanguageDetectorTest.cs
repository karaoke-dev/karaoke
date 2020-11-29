// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.Languages
{
    [TestFixture]
    public class LanguageDetectorTest
    {
        [TestCase("花火大会", "zh-CN")]
        [TestCase("花火大會", "zh-TW")]
        [TestCase("Testing", "en")]
        [TestCase("ハナビ", "ja")]
        [TestCase("はなび", "ja")]
        public void TestDetectLanguage(string text, string language)
        {
            var detector = new LanguageDetector(generageConfig());
            var result = detector.DetectLanguage(new Lyric { Text = text });
            Assert.AreEqual(result, new CultureInfo(language));
        }

        private LanguageDetectorConfig generageConfig()
        {
            return new LanguageDetectorConfig();
        }
    }
}
