// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Language;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Language
{
    [TestFixture]
    public class LanguageDetectorTest : BaseDetectorTest<LanguageDetector, CultureInfo?, LanguageDetectorConfig>
    {
        [TestCase("花火大会", true)]
        [TestCase("", false)] // will not able to detect the language if lyric is empty.
        [TestCase("   ", false)]
        [TestCase(null, false)]
        public void TestCanDetect(string text, bool canDetect)
        {
            var lyric = new Lyric { Text = text };
            var config = GeneratorConfig();
            CheckCanDetect(lyric, canDetect, config);
        }

        [TestCase("花火大会", "zh-CN")]
        [TestCase("花火大會", "zh-TW")]
        [TestCase("Testing", "en")]
        [TestCase("ハナビ", "ja")]
        [TestCase("はなび", "ja")]
        public void TestDetect(string text, string language)
        {
            var lyric = new Lyric { Text = text };
            var config = GeneratorConfig();
            var expected = new CultureInfo(language);
            CheckDetectResult(lyric, expected, config);
        }

        protected override void AssertEqual(CultureInfo? expected, CultureInfo? actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}
