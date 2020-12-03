// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.TimeTags.Zh
{
    [TestFixture]
    public class ZhTimeTagGeneratorTest
    {
        [TestCase("測試一些歌詞", new double[] { 0, 1, 2, 3, 4, 5, 5.5 })]
        [TestCase("拉拉拉~~~", new double[] { 0, 1, 2, 5.5 })]
        [TestCase("拉~拉~拉~", new double[] { 0, 2, 4, 5.5 })]
        public void TestLyricWithCheckLineEndKeyUp(string lyric, double[] index)
        {
            var config = generatorConfig(nameof(ZhTimeTagGeneratorConfig.CheckLineEndKeyUp));
            RunTimeTagCheckTest(lyric, index, config);
        }

        #region test helper

        protected void RunTimeTagCheckTest(string lyricText, double[] index, ZhTimeTagGeneratorConfig config)
        {
            var generator = new ZhTimeTagGenerator(config);
            var lyric = generateLyric(lyricText);

            // create time tag and actually time tag.
            var timeTags = getTimeTagIndex(generator.CreateTimeTags(lyric));
            var actualIndexed = getTimeTagIndexByArray(index);

            // check should be equal
            Assert.AreEqual(timeTags, actualIndexed);
        }

        private TimeTagIndex[] getTimeTagIndex(Tuple<TimeTagIndex, double?>[] timeTags)
            => timeTags.Select((v, i) => v.Item1).ToArray();

        private ZhTimeTagGeneratorConfig generatorConfig(params string[] properties)
        {
            var config = new ZhTimeTagGeneratorConfig();

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

        private TimeTagIndex[] getTimeTagIndexByArray(double[] timeTags)
            => timeTags.Select(timeTag =>
            {
                var state = Math.Abs(timeTag) % 1 == 0.5 ? TimeTagIndex.IndexState.End : TimeTagIndex.IndexState.Start;
                var index = (int)timeTag;
                return new TimeTagIndex(index, state);
            }).ToArray();

        private Lyric generateLyric(string text)
            => new Lyric { Text = text };

        #endregion
    }
}
