// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.RomajiTags.Ja
{
    public class JaRomajiTagGeneratorTest : BaseRomajiTagGeneratorTest<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>
    {
        [TestCase("花火大会", true)]
        [TestCase("", false)] // will not able to generate the romaji if lyric is empty.
        [TestCase("   ", false)]
        [TestCase(null, false)]
        public void TestCanGenerate(string text, bool canGenerate)
        {
            var config = GeneratorConfig();
            CheckCanGenerate(text, canGenerate, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:hanabi", "[2,4]:taikai" })]
        [TestCase("はなび", new[] { "[0,3]:hanabi" })]
        [TestCase("枯れた世界に輝く", new[] { "[0,3]:kareta", "[3,6]:sekaini", "[6,8]:kagayaku" })]
        public void TestGenerate(string text, string[] expectedRomajies)
        {
            var config = GeneratorConfig();
            CheckGenerateResult(text, expectedRomajies, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:HANABI", "[2,4]:TAIKAI" })]
        [TestCase("はなび", new[] { "[0,3]:HANABI" })]
        public void TestGenerateWithUppercase(string text, string[] expectedRomajies)
        {
            var config = GeneratorConfig(nameof(JaRomajiTagGeneratorConfig.Uppercase));
            CheckGenerateResult(text, expectedRomajies, config);
        }
    }
}
