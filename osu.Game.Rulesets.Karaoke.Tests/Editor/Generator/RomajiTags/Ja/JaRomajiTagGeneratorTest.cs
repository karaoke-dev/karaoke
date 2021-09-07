// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.RomajiTags.Ja
{
    public class JaRomajiTagGeneratorTest
    {
        [TestCase("花火大会", new[] { "[0,2]:hanabi", "[2,4]:taikai" })]
        [TestCase("はなび", new[] { "[0,3]:hanabi" })]
        [TestCase("枯れた世界に輝く", new[] { "[0,3]:kareta", "[3,6]:sekaini", "[6,8]:kagayaku" })]
        public void TestCreateRomajiTags(string text, string[] actualRomaji)
        {
            var config = generatorConfig(null);
            runRomajiCheckTest(text, actualRomaji, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:HANABI", "[2,4]:TAIKAI" })]
        [TestCase("はなび", new[] { "[0,3]:HANABI" })]
        public void TestCreateRomajiTagsWithUppercase(string text, string[] actualRomaji)
        {
            var config = generatorConfig(nameof(JaRomajiTagGeneratorConfig.Uppercase));
            runRomajiCheckTest(text, actualRomaji, config);
        }

        #region test helper

        private static void runRomajiCheckTest(string text, string[] actualRomaji, JaRomajiTagGeneratorConfig config)
        {
            var generator = new JaRomajiTagGenerator(config);

            var lyric = new Lyric { Text = text };
            var romajiTags = generator.CreateRomajiTags(lyric);
            var actualRomajiTags = TestCaseTagHelper.ParseRomajiTags(actualRomaji);

            TextTagAssert.ArePropertyEqual(romajiTags, actualRomajiTags);
        }

        private static JaRomajiTagGeneratorConfig generatorConfig(params string[] properties)
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
