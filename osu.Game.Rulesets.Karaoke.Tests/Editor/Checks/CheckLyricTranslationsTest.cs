// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricTranslations;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckLyricTranslationsTest : HitObjectCheckTest<Lyric, CheckLyricTranslations>
{
    [TestCase("translation")]
    [TestCase("k")] // not limit min size for now.
    [TestCase("翻譯")] // not limit language.
    public void TestCheck(string text)
    {
        var lyric = new Lyric
        {
            Translations = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo("Ja-jp"), text },
            },
        };

        AssertOk(lyric);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")] // but should not be empty or white space.
    [TestCase("　")] // but should not be empty or white space.
    public void TestCheckEmptyText(string text)
    {
        var lyric = new Lyric
        {
            Translations = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo("Ja-jp"), text },
            },
        };

        AssertNotOk<LyricIssue, IssueTemplateEmptyText>(lyric);
    }
}
