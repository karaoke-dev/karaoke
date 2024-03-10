// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.RubyTags;

public class RubyTagGeneratorSelectorTest : BaseLyricGeneratorSelectorTest<RubyTagGeneratorSelector, RubyTag[]>
{
    [TestCase(17, "花火大会", true)]
    [TestCase(17, "我是中文", true)] // only change the language code to decide should be able to generate or not.
    [TestCase(17, "", false)] // will not able to generate the romanisation if lyric is empty.
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

    [TestCase(17, "花火大会", new[] { "[0,1]:はなび", "[2,3]:たいかい" })] // Japanese
    [TestCase(1041, "はなび", new string[] { })] // Japanese
    public void TestGenerate(int lcid, string text, string[] expectedRubies)
    {
        var selector = CreateSelector();
        var lyric = new Lyric
        {
            Language = new CultureInfo(lcid),
            Text = text,
        };

        var expected = TestCaseTagHelper.ParseRubyTags(expectedRubies);
        CheckGenerateResult(lyric, expected, selector);
    }

    protected override void AssertEqual(RubyTag[] expected, RubyTag[] actual)
    {
        TextTagAssert.ArePropertyEqual(expected, actual);
    }
}
