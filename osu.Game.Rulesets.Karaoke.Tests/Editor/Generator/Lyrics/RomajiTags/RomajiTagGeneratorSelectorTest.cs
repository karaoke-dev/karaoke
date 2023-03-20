// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.RomajiTags;

public class RomajiTagGeneratorSelectorTest : BaseLyricGeneratorSelectorTest<RomajiTagGeneratorSelector, RomajiTag[]>
{
    [TestCase(17, "花火大会", true)]
    [TestCase(17, "我是中文", true)] // only change the language code to decide should be able to generate or not.
    [TestCase(17, "", false)] // will not able to generate the romaji if lyric is empty.
    [TestCase(17, "   ", false)]
    [TestCase(17, null, false)]
    [TestCase(1028, "はなび", false)] // Should not be able to generate if language is not supported.
    public void TestCanGenerate(int lcid, string text, bool canGenerate)
    {
        var selector = CreateSelector();
        var lyric = new Lyric
        {
            Language = new CultureInfo(lcid),
            Text = text,
        };

        CheckCanGenerate(lyric, canGenerate, selector);
    }

    [TestCase(17, "花火大会", new[] { "[0,2]:hanabi", "[2,4]:taikai" })] // Japanese
    [TestCase(1041, "はなび", new[] { "[0,3]:hanabi" })] // Japanese
    public void TestGenerate(int lcid, string text, string[] expectedRomajies)
    {
        var selector = CreateSelector();
        var lyric = new Lyric
        {
            Language = new CultureInfo(lcid),
            Text = text,
        };

        var expected = TestCaseTagHelper.ParseRomajiTags(expectedRomajies);
        CheckGenerateResult(lyric, expected, selector);
    }

    protected override void AssertEqual(RomajiTag[] expected, RomajiTag[] actual)
    {
        TextTagAssert.ArePropertyEqual(expected, actual);
    }
}
