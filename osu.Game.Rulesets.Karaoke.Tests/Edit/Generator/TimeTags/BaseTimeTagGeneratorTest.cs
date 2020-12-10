// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.TimeTags
{
    public abstract class BaseTimeTagGeneratorTest<TTimeTagGenerator, TConfig>
        where TTimeTagGenerator : TimeTagGenerator<TConfig> where TConfig : TimeTagGeneratorConfig, new()
    {
        protected void RunTimeTagCheckTest(string lyricText, double[] index, TConfig config)
        {
            var lyric = generateLyric(lyricText);
            RunTimeTagCheckTest(lyric, index, config);
        }

        protected void RunTimeTagCheckTest(Lyric lyric, double[] index, TConfig config)
        {
            var generator = Activator.CreateInstance(typeof(TTimeTagGenerator), config) as TTimeTagGenerator;

            // create time tag and actually time tag.
            var timeTags = getTimeTagIndex(generator.CreateTimeTags(lyric));
            var actualIndexed = getTimeTagIndexByArray(index);

            // check should be equal
            Assert.AreEqual(timeTags, actualIndexed);
        }

        protected TConfig GeneratorConfig(params string[] properties)
        {
            var config = new TConfig();
            if (properties == null)
                return config;

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

        #region test helper

        private TimeTagIndex[] getTimeTagIndex(TimeTag[] timeTags)
            => timeTags.Select((v, i) => v.Item1).ToArray();

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
