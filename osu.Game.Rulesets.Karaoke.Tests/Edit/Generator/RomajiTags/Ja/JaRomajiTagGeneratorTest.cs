// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.RomajiTags.Ja
{
    public class JaRomajiTagGeneratorTest
    {
        [TestCase("花火大会", new[] { "hanabi", "taikei" })]
        [TestCase("はなび", new string[] { })]
        public void TestCreateRomajiTags(string text, string[] romaji)
        {
            var config = generatorConfig(null);
            RunRomajiCheckTest(text, romaji, config);
        }

        #region test helper

        protected void RunRomajiCheckTest(string text, string[] romaji, JaRomajiTagGeneratorConfig config)
        {
            var generator = new JaRomajiTagGenerator(config);

            var lyric = new Lyric { Text = text };
            var romajiTags = generator.CreateRomajiTags(lyric);

            Assert.AreEqual(romajiTags.Length, romajiTags.Length);

            foreach (var romajiTag in romajiTags)
            {
                Assert.IsTrue(romaji.Contains(romajiTag.Text));
            }
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
