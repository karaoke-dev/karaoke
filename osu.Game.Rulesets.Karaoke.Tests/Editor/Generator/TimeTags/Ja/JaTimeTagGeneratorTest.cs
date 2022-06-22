// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.TimeTags.Ja
{
    [TestFixture]
    public class JaTimeTagGeneratorTest : BaseTimeTagGeneratorTest<JaTimeTagGenerator, JaTimeTagGeneratorConfig>
    {
        [TestCase("花火大会", true)]
        [TestCase("！", true)]
        [TestCase("   ", true)]
        [TestCase("", false)] // will not able to generate the romaji if lyric is empty.
        [TestCase(null, false)]
        public void TestCanGenerate(string text, bool canGenerate)
        {
            var config = GeneratorConfig();
            CheckCanGenerate(text, canGenerate, config);
        }

        [TestCase("か", new[] { "[0,start]:" }, false)]
        [TestCase("か", new[] { "[0,start]:", "[0,end]:" }, true)]
        public void TestGenerateWithCheckLineEndKeyUp(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckLineEndKeyUp) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase(" ", new string[] { }, false)]
        [TestCase(" ", new[] { "[0,start]:" }, true)]
        public void TestGenerateWithCheckBlankLine(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckBlankLine) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase("か     ", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:", "[5,start]:" }, false)]
        [TestCase("か     ", new[] { "[0,start]:", "[1,start]:" }, true)]
        public void TestGenerateWithCheckWhiteSpace(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase("か ", new[] { "[0,start]:", "[1,start]:" }, false)]
        [TestCase("か ", new[] { "[0,start]:", "[0,end]:" }, true)]
        public void TestGenerateWithCheckWhiteSpaceKeyUp(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase("a　b　c", new[] { "[0,start]:", "[2,start]:", "[4,start]:" }, false, false)]
        [TestCase("a　b　c", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:" }, true, false)]
        [TestCase("a　b　c", new[] { "[0,start]:", "[0,end]:", "[2,start]:", "[2,end]:", "[4,start]:" }, true, true)]
        [TestCase("Ａ　Ｂ　Ｃ", new[] { "[0,start]:", "[2,start]:", "[4,start]:" }, false, false)]
        [TestCase("Ａ　Ｂ　Ｃ", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:" }, true, false)]
        [TestCase("Ａ　Ｂ　Ｃ", new[] { "[0,start]:", "[0,end]:", "[2,start]:", "[2,end]:", "[4,start]:" }, true, true)]
        public void TestGenerateWithCheckWhiteSpaceAlphabet(string lyric, string[] expectedTimeTags, bool applyConfig, bool keyUp)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAlphabet) : null,
                keyUp ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase("0　1　2", new[] { "[0,start]:", "[2,start]:", "[4,start]:" }, false, false)]
        [TestCase("0　1　2", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:" }, true, false)]
        [TestCase("0　1　2", new[] { "[0,start]:", "[0,end]:", "[2,start]:", "[2,end]:", "[4,start]:" }, true, true)]
        [TestCase("０　１　２", new[] { "[0,start]:", "[2,start]:", "[4,start]:" }, false, false)]
        [TestCase("０　１　２", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:" }, true, false)]
        [TestCase("０　１　２", new[] { "[0,start]:", "[0,end]:", "[2,start]:", "[2,end]:", "[4,start]:" }, true, true)]
        public void TestGenerateWithCheckWhiteSpaceDigit(string lyric, string[] expectedTimeTags, bool applyConfig, bool keyUp)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceDigit) : null,
                keyUp ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase("!　!　!", new[] { "[0,start]:", "[2,start]:", "[4,start]:" }, false, false)]
        [TestCase("!　!　!", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:" }, true, false)]
        [TestCase("!　!　!", new[] { "[0,start]:", "[0,end]:", "[2,start]:", "[2,end]:", "[4,start]:" }, true, true)]
        public void TestGenerateWitCheckWhiteSpaceAsciiSymbol(string lyric, string[] expectedTimeTags, bool applyConfig, bool keyUp)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAsciiSymbol) : null,
                keyUp ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase("がんばって", new[] { "[0,start]:", "[2,start]:", "[4,start]:" }, false)]
        [TestCase("がんばって", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[4,start]:" }, true)]
        public void TestGenerateWithCheckWhiteCheckん(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkん) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [TestCase("買って", new[] { "[0,start]:", "[2,start]:" }, false)]
        [TestCase("買って", new[] { "[0,start]:", "[1,start]:", "[2,start]:" }, true)]
        public void TestGenerateWithCheckっ(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkっ) : null);
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }

        [Test]
        public void TestGenerateWithRubyLyric()
        {
            var config = GeneratorConfig();
            var lyric = new Lyric
            {
                Text = "明日いっしょに遊びましょう！",
                RubyTags = new[]
                {
                    new RubyTag
                    {
                        StartIndex = 0,
                        EndIndex = 2,
                        Text = "あした"
                    },
                    new RubyTag
                    {
                        StartIndex = 7,
                        EndIndex = 8,
                        Text = "あそ"
                    }
                }
            };

            string[] expectedTimeTags =
            {
                "[0,start]:",
                "[0,start]:",
                "[0,start]:",
                "[2,start]:",
                "[4,start]:",
                "[6,start]:",
                "[7,start]:",
                "[7,start]:",
                "[8,start]:",
                "[9,start]:",
                "[10,start]:",
                "[12,start]:",
                "[13,start]:"
            };
            CheckGenerateResult(lyric, expectedTimeTags, config);
        }
    }
}
