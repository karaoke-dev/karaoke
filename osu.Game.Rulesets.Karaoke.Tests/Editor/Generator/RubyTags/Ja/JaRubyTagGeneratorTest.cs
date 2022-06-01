// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.RubyTags.Ja
{
    [TestFixture]
    public class JaRubyTagGeneratorTest
    {
        [TestCase("花火大会", true)]
        [TestCase("", false)] // will not able to generate the ruby if lyric is empty.
        [TestCase("   ", false)]
        [TestCase(null, false)]
        public void TestCanGenerate(string text, bool canGenerate)
        {
            var config = generatorConfig(null);
            runCanGenerateRubyCheckTest(text, canGenerate, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:はなび", "[2,4]:たいかい" })]
        [TestCase("はなび", new string[] { })]
        public void TestGenerate(string text, string[] expectedRubies)
        {
            var config = generatorConfig(null);
            runRubyCheckTest(text, expectedRubies, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:ハナビ", "[2,4]:タイカイ" })]
        [TestCase("ハナビ", new string[] { })]
        public void TestGenerateWithRubyAsKatakana(string text, string[] expectedRubies)
        {
            var config = generatorConfig(nameof(JaRubyTagGeneratorConfig.RubyAsKatakana));
            runRubyCheckTest(text, expectedRubies, config);
        }

        [TestCase("はなび", new[] { "[0,2]:はな", "[2,3]:び" })]
        [TestCase("ハナビ", new[] { "[0,3]:はなび" })]
        public void TestGenerateWithEnableDuplicatedRuby(string text, string[] expectedRubies)
        {
            var config = generatorConfig(nameof(JaRubyTagGeneratorConfig.EnableDuplicatedRuby));
            runRubyCheckTest(text, expectedRubies, config);
        }

        #region test helper

        private static void runCanGenerateRubyCheckTest(string text, bool canGenerate, JaRubyTagGeneratorConfig config)
        {
            var generator = new JaRubyTagGenerator(config);
            var lyric = new Lyric { Text = text };

            bool actual = generator.CanGenerate(lyric);
            Assert.AreEqual(canGenerate, actual);
        }

        private static void runRubyCheckTest(string text, IEnumerable<string> expectedRubies, JaRubyTagGeneratorConfig config)
        {
            var generator = new JaRubyTagGenerator(config);
            var lyric = new Lyric { Text = text };

            var expected = TestCaseTagHelper.ParseRubyTags(expectedRubies);
            var actual = generator.Generate(lyric);
            TextTagAssert.ArePropertyEqual(expected, actual);
        }

        private static JaRubyTagGeneratorConfig generatorConfig(params string[] properties)
        {
            var config = new JaRubyTagGeneratorConfig();
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

        #endregion
    }
}
