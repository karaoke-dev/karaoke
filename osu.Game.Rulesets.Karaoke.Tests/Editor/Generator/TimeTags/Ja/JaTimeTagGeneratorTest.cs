// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.TimeTags.Ja
{
    [TestFixture]
    public class JaTimeTagGeneratorTest : BaseTimeTagGeneratorTest<JaTimeTagGenerator, JaTimeTagGeneratorConfig>
    {
        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckLineEnd(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckLineEnd) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("か", new[] { "[0,start]:" }, false)]
        [TestCase("か", new[] { "[0,start]:", "[0,end]:" }, true)]
        public void TestLyricWithCheckLineEndKeyUp(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckLineEndKeyUp) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckBlankLine(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckBlankLine) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("     ", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:" }, false)]
        [TestCase("     ", new[] { "[0,start]:" }, true)]
        public void TestLyricWithCheckWhiteSpace(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("か ", new[] { "[0,start]:", "[1,start]:" }, false)]
        [TestCase("か ", new[] { "[0,start]:", "[0,end]:" }, true)]
        public void TestLyricWithCheckWhiteSpaceKeyUp(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("a　b　c　d　e", new[] { "[0,start]:", "[2,start]:", "[4,start]:", "[6,start]:", "[8,start]:" }, false)]
        [TestCase("a　b　c　d　e", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:", "[5,start]:", "[6,start]:", "[7,start]:", "[8,start]:" }, true)]
        [TestCase("Ａ　Ｂ　Ｃ　Ｄ　Ｅ", new[] { "[0,start]:", "[2,start]:", "[4,start]:", "[6,start]:", "[8,start]:" }, false)]
        [TestCase("Ａ　Ｂ　Ｃ　Ｄ　Ｅ", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:", "[5,start]:", "[6,start]:", "[7,start]:", "[8,start]:" }, true)]
        public void TestLyricWithCheckWhiteSpaceAlphabet(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAlphabet) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("0　1　2　3　4", new[] { "[0,start]:", "[2,start]:", "[4,start]:", "[6,start]:", "[8,start]:" }, false)]
        [TestCase("0　1　2　3　4", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:", "[5,start]:", "[6,start]:", "[7,start]:", "[8,start]:" }, true)]
        [TestCase("０　１　２　３　４", new[] { "[0,start]:", "[2,start]:", "[4,start]:", "[6,start]:", "[8,start]:" }, false)]
        [TestCase("０　１　２　３　４", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:", "[5,start]:", "[6,start]:", "[7,start]:", "[8,start]:" }, true)]
        public void TestLyricWithCheckWhiteSpaceDigit(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceDigit) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("!　!　!　!　！", new[] { "[0,start]:", "[2,start]:", "[4,start]:", "[6,start]:", "[8,start]:" }, false)]
        [TestCase("!　!　!　!　！", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:", "[5,start]:", "[6,start]:", "[7,start]:", "[8,start]:" }, true)]
        public void TestLyricWitCheckWhiteSpaceAsciiSymbol(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAsciiSymbol) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("がんばって", new[] { "[0,start]:", "[2,start]:", "[4,start]:" }, false)]
        [TestCase("がんばって", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[4,start]:" }, true)]
        public void TestLyricWithCheckWhiteCheckん(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkん) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [TestCase("買って", new[] { "[0,start]:", "[2,start]:" }, false)]
        [TestCase("買って", new[] { "[0,start]:", "[1,start]:", "[2,start]:" }, true)]
        public void TestLyricWithCheckっ(string lyric, string[] expectedTimeTags, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkっ) : null);
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        [Test]
        public void TestTagWithRubyLyric()
        {
            var config = GeneratorConfig(null);
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

            string[] actualTimeTags =
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
            RunTimeTagCheckTest(lyric, actualTimeTags, config);
        }
    }
}
