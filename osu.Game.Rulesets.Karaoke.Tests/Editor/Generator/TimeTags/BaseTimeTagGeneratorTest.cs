// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.TimeTags
{
    public abstract class BaseTimeTagGeneratorTest<TTimeTagGenerator, TConfig>
        where TTimeTagGenerator : TimeTagGenerator<TConfig> where TConfig : TimeTagGeneratorConfig, new()
    {
        protected void RunTimeTagCheckTest(string text, string[] actualTimeTags, TConfig config)
        {
            var lyric = new Lyric { Text = text };
            RunTimeTagCheckTest(lyric, actualTimeTags, config);
        }

        protected void RunTimeTagCheckTest(Lyric lyric, string[] actualTimeTags, TConfig config)
        {
            var generator = Activator.CreateInstance(typeof(TTimeTagGenerator), config) as TTimeTagGenerator;

            // create time tag and actually time tag.
            var timeTags = generator?.CreateTimeTags(lyric);
            var actualIndexed = TestCaseTagHelper.ParseTimeTags(actualTimeTags);

            // check should be equal
            TimeTagAssert.ArePropertyEqual(timeTags, actualIndexed);
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
