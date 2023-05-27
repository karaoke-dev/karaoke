// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.RubyTags.Ja;

[TestFixture]
public class JaRubyTagGeneratorTest : BaseRubyTagGeneratorTest<JaRubyTagGenerator, JaRubyTagGeneratorConfig>
{
    [TestCase("花火大会", true)]
    [TestCase("", false)] // will not able to generate the ruby if lyric is empty.
    [TestCase("   ", false)]
    [TestCase(null, false)]
    public void TestCanGenerate(string text, bool canGenerate)
    {
        var config = GeneratorEmptyConfig();
        CheckCanGenerate(text, canGenerate, config);
    }

    [TestCase("花火大会", new[] { "[0,1]:はなび", "[2,3]:たいかい" })]
    [TestCase("はなび", new string[] { })]
    public void TestGenerate(string text, string[] expectedRubies)
    {
        var config = GeneratorEmptyConfig();
        CheckGenerateResult(text, expectedRubies, config);
    }

    [TestCase("花火大会", new[] { "[0,1]:ハナビ", "[2,3]:タイカイ" })]
    [TestCase("ハナビ", new string[] { })]
    public void TestGenerateWithRubyAsKatakana(string text, string[] expectedRubies)
    {
        var config = GeneratorEmptyConfig(x => x.RubyAsKatakana.Value = true);
        CheckGenerateResult(text, expectedRubies, config);
    }

    [TestCase("はなび", new[] { "[0,1]:はな", "[2]:び" })]
    [TestCase("ハナビ", new[] { "[0,2]:はなび" })]
    public void TestGenerateWithEnableDuplicatedRuby(string text, string[] expectedRubies)
    {
        var config = GeneratorEmptyConfig(x => x.EnableDuplicatedRuby.Value = true);
        CheckGenerateResult(text, expectedRubies, config);
    }
}
