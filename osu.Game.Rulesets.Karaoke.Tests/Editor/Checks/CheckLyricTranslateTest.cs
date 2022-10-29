// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using J2N.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricTranslate;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class CheckLyricTranslateTest : HitObjectCheckTest<Lyric, CheckLyricTranslate>
    {
        [TestCase("translate")]
        [TestCase("k")] // not limit min size for now.
        [TestCase("翻譯")] // not limit language.
        public void TestCheck(string text)
        {
            var lyric = new Lyric
            {
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo("Ja-jp"), text }
                }
            };

            AssertOk(lyric);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")] // but should not be empty or white space.
        [TestCase("　")] // but should not be empty or white space.
        public void TestCheckTranslationNoText(string text)
        {
            var lyric = new Lyric
            {
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo("Ja-jp"), text }
                }
            };

            AssertNotOk<IssueTemplateLyricTranslationNoText>(lyric);
        }
    }
}
