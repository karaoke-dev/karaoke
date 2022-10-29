// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricLanguage;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class CheckLyricLanguageTest : HitObjectCheckTest<Lyric, CheckLyricLanguage>
    {
        [TestCase("Ja-jp")]
        [TestCase("")] // should not have issue if CultureInfo accept it.
        public void TestCheck(string language)
        {
            var lyric = new Lyric
            {
                Language = new CultureInfo(language),
            };

            AssertOk(lyric);
        }

        [TestCase(null)]
        public void TestCheckNotFillLanguage(string? language)
        {
            var lyric = new Lyric
            {
                Language = language != null ? new CultureInfo(language) : null,
            };

            AssertNotOk<IssueTemplateLyricNotFillLanguage>(lyric);
        }
    }
}
