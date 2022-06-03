// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.TimeTags
{
    public abstract class BaseTimeTagGeneratorTest<TTimeTagGenerator, TConfig>
        where TTimeTagGenerator : TimeTagGenerator<TConfig> where TConfig : TimeTagGeneratorConfig, new()
    {
        protected static void RunCanGenerateTimeTagCheckTest(string text, bool canGenerate, TConfig config)
        {
            var generator = Activator.CreateInstance(typeof(TTimeTagGenerator), config) as TTimeTagGenerator;
            var lyric = new Lyric { Text = text };

            bool? actual = generator?.GetInvalidMessage(lyric) == null;
            Assert.AreEqual(canGenerate, actual);
        }

        protected static void RunTimeTagCheckTest(string text, string[] expectedTimeTags, TConfig config)
        {
            var lyric = new Lyric { Text = text };
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }

        protected static void RunTimeTagCheckTest(Lyric lyric, string[] expectedTimeTags, TConfig config)
        {
            var generator = Activator.CreateInstance(typeof(TTimeTagGenerator), config) as TTimeTagGenerator;

            // create time tag and actually time tag.
            var expected = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            var actual = generator?.Generate(lyric);
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        protected TConfig GeneratorConfig(params string[] properties)
        {
            var config = new TConfig();
            if (properties == null)
                return config;

            foreach (string propertyName in properties)
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
    }
}
