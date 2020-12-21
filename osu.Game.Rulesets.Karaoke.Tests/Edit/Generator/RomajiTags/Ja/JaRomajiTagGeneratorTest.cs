// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.RomajiTags.Ja
{
    public class JaRomajiTagGeneratorTest
    {
        [TestCase("花火大会", new[] { "[0,2]:hanabi", "[2,4]:taikai" })]
        [TestCase("はなび", new string[] { "[0,3]:hanabi" })]
        public void TestCreateRomajiTags(string text, string[] actualRomaji)
        {
            var config = generatorConfig(null);
            RunRomajiCheckTest(text, actualRomaji, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:HANABI", "[2,4]:TAIKAI" })]
        [TestCase("はなび", new string[] { "[0,3]:HANABI" })]
        public void TestCreateRomajiTagsWithUppercase(string text, string[] actualRomaji)
        {
            var config = generatorConfig(nameof(JaRomajiTagGeneratorConfig.Uppercase));
            RunRomajiCheckTest(text, actualRomaji, config);
        }

        #region test helper

        protected void RunRomajiCheckTest(string text, string[] actualRomaji, JaRomajiTagGeneratorConfig config)
        {
            var generator = new JaRomajiTagGenerator(config);

            var lyric = new Lyric { Text = text };
            var romajiTags = generator.CreateRomajiTags(lyric);
            var actualRomajiTags = TestCaseTagHelper.ParseRomajiTags(actualRomaji);

            Assert.AreEqual(romajiTags, actualRomajiTags);
        }

        private JaRomajiTagGeneratorConfig generatorConfig(params string[] properties)
        {
            var config = new JaRomajiTagGeneratorConfig();
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

        #endregion
    }
}
