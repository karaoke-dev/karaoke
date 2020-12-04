// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.TimeTags.Ja
{
    [TestFixture]
    public class JaTimeTagGeneratorTest : BaseTimeTagGeneratorTest<JaTimeTagGenerator, JaTimeTagGeneratorConfig>
    {
        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckLineEnd(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckLineEnd) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [TestCase("か", new double[] { 0 }, false)]
        [TestCase("か", new[] { 0, 0.5 }, true)]
        public void TestLyricWithCheckLineEndKeyUp(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckLineEndKeyUp) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckBlankLine(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckBlankLine) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [TestCase("     ", new double[] { 0, 1, 2, 3, 4 }, false)]
        [TestCase("     ", new double[] { 0 }, true)]
        public void TestLyricWithCheckWhiteSpace(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckWhiteSpaceKeyUp(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [TestCase("a　b　c　d　e", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("a　b　c　d　e", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        [TestCase("Ａ　Ｂ　Ｃ　Ｄ　Ｅ", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("Ａ　Ｂ　Ｃ　Ｄ　Ｅ", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        public void TestLyricWithCheckWhiteSpaceAlphabet(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAlphabet) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [TestCase("0　1　2　3　4", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("0　1　2　3　4", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        [TestCase("０　１　２　３　４", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("０　１　２　３　４", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        public void TestLyricWithCheckWhiteSpaceDigit(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceDigit) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [TestCase("!　!　!　!　！", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("!　!　!　!　！", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        public void TestLyricWitCheckWhiteSpaceAsciiSymbol(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAsciiSymbol) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [TestCase("がんばって", new double[] { 0, 2, 4 }, false)]
        [TestCase("がんばって", new double[] { 0, 1, 2, 4 }, true)]
        public void TestLyricWithCheckWhiteCheckん(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkん) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [TestCase("買って", new double[] { 0, 2 }, false)]
        [TestCase("買って", new double[] { 0, 1, 2 }, true)]
        public void TestLyricWithCheckっ(string lyric, double[] index, bool applyConfig)
        {
            var config = GeneratorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkっ) : null);
            RunTimeTagCheckTest(lyric, index, config);
        }

        [Test]
        public void TestTagWithRubyLyric()
        {
            var config = GeneratorConfig(null);
            var lyric = new Lyric
            {
                Text = "明日いっしょに遊びましょう！",
                RubyTags = new []
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

            var result = new double[] { 0, 0, 0, 2, 4, 6, 7, 7, 8, 9, 10, 12, 13 };
            RunTimeTagCheckTest(lyric, result, config);
        }
    }
}
