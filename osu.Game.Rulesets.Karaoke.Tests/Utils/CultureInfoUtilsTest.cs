// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [Ignore("Cannot run those test cases right now because will got different results in the different platform...")]
    public class CultureInfoUtilsTest
    {
        [Test]
        public void TestGetAvailableLanguages()
        {
            // seems there are 276 languages in the world.
            const int expected = 276;
            int actual = CultureInfoUtils.GetAvailableLanguages().Length;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGetAvailableLanguagesWithUniqueLcid()
        {
            var languages = CultureInfoUtils.GetAvailableLanguages();

            int uniqueLcidAmount = languages.Select(x => x.LCID).Where(x => x > 0).Distinct().Count();
            int uniqueTwoLetterIsoLanguageNameAmount = languages.Select(x => x.TwoLetterISOLanguageName).Where(x => !string.IsNullOrEmpty(x)).Distinct().Count();
            int uniqueThreeLetterIsoLanguageNameAmount = languages.Select(x => x.ThreeLetterISOLanguageName).Where(x => !string.IsNullOrEmpty(x)).Distinct().Count();

            // todo: we should make sure that all the language code is not duplicated.
            Assert.AreEqual(160, uniqueLcidAmount);
            Assert.AreEqual(244, uniqueTwoLetterIsoLanguageNameAmount);
            Assert.AreEqual(244, uniqueThreeLetterIsoLanguageNameAmount);
        }

        [TestCase("zh-Hans", true)] // 中文（简体）, 4
        [TestCase("zh-Hant", true)] // 中文（繁體）, 31748
        [TestCase("zh", true)] // 中文, 30724
        [TestCase("zh-TW", false)] // 中文（台灣）, 1028
        [TestCase("zh-Hant-TW", false)] // 中文（繁體，台灣）, 4096
        [TestCase("zh-Hans-HK", false)] // 中文（简体，香港特别行政区）, 4096
        public void TesIsLanguage(string name, bool isLanguage)
        {
            var cultureInfo = new CultureInfo(name);
            bool actual = CultureInfoUtils.IsLanguage(cultureInfo);
            Assert.AreEqual(isLanguage, actual);
        }

        [TestCase(4, true)] // 中文（简体）
        [TestCase(31748, true)] // 中文（繁體）
        [TestCase(30724, true)] // 中文
        [TestCase(1028, false)] // 中文（台灣）
        public void TesIsLanguage(int lcid, bool isLanguage)
        {
            var cultureInfo = new CultureInfo(lcid);
            bool actual = CultureInfoUtils.IsLanguage(cultureInfo);
            Assert.AreEqual(isLanguage, actual);
        }

        [TestCase(4, "中文（简体）")]
        [TestCase(31748, "中文（繁體）")]
        [TestCase(30724, "中文")]
        [TestCase(1028, "中文（台灣）")]
        public void TestGetLanguageDisplayText(int lcid, string displayText)
        {
            var cultureInfo = new CultureInfo(lcid);
            string actual = CultureInfoUtils.GetLanguageDisplayText(cultureInfo);
            Assert.AreEqual(displayText, actual);
        }
    }
}
