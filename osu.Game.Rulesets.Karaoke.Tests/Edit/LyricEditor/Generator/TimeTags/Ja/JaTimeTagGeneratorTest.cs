// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.LyricEditor.Generator.TimeTags.Ja
{
    [TestFixture]
    public class JaTimeTagGeneratorTest
    {
        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckLineEnd(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckLineEnd) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0 }, false)]
        [TestCase("か", new double[] { 0, 0.5 }, true)]
        public void TestLyricWithCheckLineEndKeyUp(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckLineEndKeyUp) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckBlankLine(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckBlankLine) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("     ", new double[] { 0, 1, 2 ,3, 4 }, false)]
        [TestCase("     ", new double[] { 0 }, true)]
        public void TestLyricWithCheckWhiteSpace(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [Ignore("This feature has not been implemented")]
        public void TestLyricWithCheckWhiteSpaceKeyUp(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("a　b　c　d　e", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("a　b　c　d　e", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        [TestCase("Ａ　Ｂ　Ｃ　Ｄ　Ｅ", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("Ａ　Ｂ　Ｃ　Ｄ　Ｅ", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        public void TestLyricWithCheckWhiteSpaceAlphabet(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAlphabet) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("0　1　2　3　4", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("0　1　2　3　4", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        [TestCase("０　１　２　３　４", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("０　１　２　３　４", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        public void TestLyricWithCheckWhiteSpaceDigit(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceDigit) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("!　!　!　!　！", new double[] { 0, 2, 4, 6, 8 }, false)]
        [TestCase("!　!　!　!　！", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, true)]
        public void TestLyricWitCheckWhiteSpaceAsciiSymbol(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace),
                applyConfig ? nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAsciiSymbol) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("がんばって", new double[] { 0, 2, 4 }, false)]
        [TestCase("がんばって", new double[] { 0, 1, 2, 4 }, true)]
        public void TestLyricWithCheckWhiteCheckん(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkん) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("買って", new double[] { 0, 2 }, false)]
        [TestCase("買って", new double[] { 0, 1, 2 }, true)]
        public void TestLyricWithCheckっ(string lyric, double[] index, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaTimeTagGeneratorConfig.Checkっ) : null);
            RunTimeTagCheckText(lyric, index, config);
        }

        #region test helper

        protected void RunTimeTagCheckText(string lyricText, double[] index, JaTimeTagGeneratorConfig config)
        {
            var generator = new JaTimeTagGenerator(config);
            var lyric = generateLyric(lyricText);

            // create time tag and aceually time tag.
            var timeTags = getTimeTagIndex(generator.CreateTimeTag(lyric));
            var actualIndexed = getTimeTagIndexByArray(index);

            // chekc should be equal
            Assert.AreEqual(timeTags, actualIndexed);
        }

        private TimeTagIndex[] getTimeTagIndex(Tuple<TimeTagIndex, double?>[] timeTags)
            => timeTags.Select((v, i) => v.Item1).ToArray();

        private TimeTagIndex[] getTimeTagIndexByArray(double[] timeTags)
            => timeTags.Select(timeTag =>
            {
                var state = Math.Abs(timeTag) % 1 == 0.5 ? TimeTagIndex.IndexState.End : TimeTagIndex.IndexState.Start;
                var index = (int)timeTag;
                return new TimeTagIndex(index, state);
            }).ToArray();

        private JaTimeTagGeneratorConfig generatorConfig(params string[] properties)
        {
            var config = new JaTimeTagGeneratorConfig();

            foreach (var propertyName in properties)
            {
                if (propertyName == null)
                    continue;

                var theMethod = config.GetType().GetProperty(propertyName);
                if (theMethod == null)
                    throw new MissingMethodException("Config is not exist.");

                theMethod.SetValue(config, true);
            }

            return config;
        }

        private Lyric generateLyric(string text)
            => new Lyric { Text = text };

        #endregion
    }
}
