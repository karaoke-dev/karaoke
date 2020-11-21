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
        public void TestLyricWithCheckLineEnd(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckLineEnd));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 0.5 })]
        // [TestCase("カラオケ", new double[] { 0, 0.5 })]
        public void TestLyricWithCheckLineEndKeyUp(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckLineEndKeyUp));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWithCheckBlankLine(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckBlankLine));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWithCheckWhiteSpace(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWithCheckWhiteSpaceKeyUp(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWithCheckWhiteSpaceAlphabet(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAlphabet));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWithCheckWhiteSpaceDigit(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceDigit));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWitCheckWhiteSpaceAsciiSymbol(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAsciiSymbol));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWithCheckWhiteCheckん(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.Checkん));
            RunTimeTagCheckText(lyric, index, config);
        }

        [TestCase("か", new double[] { 0, 1 })]
        public void TestLyricWithCheckっ(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(JaTimeTagGeneratorConfig.Checkっ));
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

        private JaTimeTagGeneratorConfig generatorConfig(string applyProperty)
        {
            var config = new JaTimeTagGeneratorConfig();
            var theMethod = config.GetType().GetProperty(applyProperty);
            if (theMethod == null)
                throw new MissingMethodException("Config is not exist.");

            theMethod.SetValue(config, true);

            return config;
        }

        private Lyric generateLyric(string text)
            => new Lyric { Text = text };

        #endregion
    }
}
