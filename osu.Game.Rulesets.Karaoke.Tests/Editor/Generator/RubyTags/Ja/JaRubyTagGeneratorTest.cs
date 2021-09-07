// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
        [TestCase("花火大会", new[] { "[0,2]:はなび", "[2,4]:たいかい" })]
        [TestCase("はなび", new string[] { })]
        public void TestCreateRubyTags(string text, string[] actualRuby)
        {
            var config = generatorConfig(null);
            runRubyCheckTest(text, actualRuby, config);
        }

        [TestCase("花火大会", new[] { "[0,2]:ハナビ", "[2,4]:タイカイ" })]
        [TestCase("ハナビ", new string[] { })]
        public void TestCreateRubyTagsWithRubyAsKatakana(string text, string[] actualRuby)
        {
            var config = generatorConfig(nameof(JaRubyTagGeneratorConfig.RubyAsKatakana));
            runRubyCheckTest(text, actualRuby, config);
        }

        [TestCase("はなび", new[] { "[0,2]:はな", "[2,3]:び" })]
        [TestCase("ハナビ", new[] { "[0,3]:はなび" })]
        public void TestCreateRubyTagsWithEnableDuplicatedRuby(string text, string[] actualRuby)
        {
            var config = generatorConfig(nameof(JaRubyTagGeneratorConfig.EnableDuplicatedRuby));
            runRubyCheckTest(text, actualRuby, config);
        }

        #region test helper

        private static void runRubyCheckTest(string text, string[] actualRuby, JaRubyTagGeneratorConfig config)
        {
            var generator = new JaRubyTagGenerator(config);

            var lyric = new Lyric { Text = text };
            var rubyTags = generator.CreateRubyTags(lyric);
            var actualRubyTags = TestCaseTagHelper.ParseRubyTags(actualRuby);

            TextTagAssert.ArePropertyEqual(rubyTags, actualRubyTags);
        }

        private static JaRubyTagGeneratorConfig generatorConfig(params string[] properties)
        {
            var config = new JaRubyTagGeneratorConfig();
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
