// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.RomajiTags.Ja;

public class JaRomajiTagGeneratorTest : BaseRomajiTagGeneratorTest<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>
{
    [TestCase("花火大会", true)]
    [TestCase("", false)] // will not able to generate the romaji if lyric is empty.
    [TestCase("   ", false)]
    [TestCase(null, false)]
    public void TestCanGenerate(string text, bool canGenerate)
    {
        var config = GeneratorEmptyConfig();
        CheckCanGenerate(text, canGenerate, config);
    }

    [TestCase("花火大会", new[] { "[0,1]:hanabi", "[2,3]:taikai" })]
    [TestCase("はなび", new[] { "[0,2]:hanabi" })]
    [TestCase("枯れた世界に輝く", new[] { "[0,2]:kareta", "[3,5]:sekaini", "[6,7]:kagayaku" })]
    public void TestGenerate(string text, string[] expectedRomajies)
    {
        var config = GeneratorEmptyConfig();
        CheckGenerateResult(text, expectedRomajies, config);
    }

    [TestCase("花火大会", new[] { "[0,1]:HANABI", "[2,3]:TAIKAI" })]
    [TestCase("はなび", new[] { "[0,2]:HANABI" })]
    public void TestGenerateWithUppercase(string text, string[] expectedRomajies)
    {
        var config = GeneratorEmptyConfig(x => x.Uppercase.Value = true);
        CheckGenerateResult(text, expectedRomajies, config);
    }
}
