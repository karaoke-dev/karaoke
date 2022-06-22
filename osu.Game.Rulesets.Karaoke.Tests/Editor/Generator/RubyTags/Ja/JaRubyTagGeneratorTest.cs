// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.RubyTags.Ja
{
    [TestFixture]
    public class JaRubyTagGeneratorTest : BaseRubyTagGeneratorTest<JaRubyTagGenerator, JaRubyTagGeneratorConfig>
    {
        [TestCase("花火大会", true)]
        [TestCase("", false)] // will not able to generate the ruby if lyric is empty.
        [TestCase("   ", false)]
        [TestCase(null, false)]
        public void TestCanGenerate(string text, bool canGenerate)
        {
            var config = GeneratorConfig();
            CheckCanGenerate(text, canGenerate, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:はなび", "[2,4]:たいかい" })]
        [TestCase("はなび", new string[] { })]
        public void TestGenerate(string text, string[] expectedRubies)
        {
            var config = GeneratorConfig();
            CheckGenerateResult(text, expectedRubies, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:ハナビ", "[2,4]:タイカイ" })]
        [TestCase("ハナビ", new string[] { })]
        public void TestGenerateWithRubyAsKatakana(string text, string[] expectedRubies)
        {
            var config = GeneratorConfig(nameof(JaRubyTagGeneratorConfig.RubyAsKatakana));
            CheckGenerateResult(text, expectedRubies, config);
        }

        [TestCase("はなび", new[] { "[0,2]:はな", "[2,3]:び" })]
        [TestCase("ハナビ", new[] { "[0,3]:はなび" })]
        public void TestGenerateWithEnableDuplicatedRuby(string text, string[] expectedRubies)
        {
            var config = GeneratorConfig(nameof(JaRubyTagGeneratorConfig.EnableDuplicatedRuby));
            CheckGenerateResult(text, expectedRubies, config);
        }
    }
}
