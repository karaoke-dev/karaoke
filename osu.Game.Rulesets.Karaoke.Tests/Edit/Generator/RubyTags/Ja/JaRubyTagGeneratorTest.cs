// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.RubyTags.Ja
{
    [TestFixture]
    public class JaRubyTagGeneratorTest
    {
        [TestCase("花火大会", new[] { "はなび", "たいかい" })]
        [TestCase("はなび", new string[] { })]
        public void TestCreateRubyTags(string text, string[] ruby)
        {
            var config = generatorConfig(null);
            RunRubyCheckTest(text, ruby, config);
        }

        [TestCase("花火大会", new[] { "ハナビ", "タイカイ" })]
        [TestCase("ハナビ", new string[] { })]
        public void TestCreateRubyTagsWithRubyAsKatakana(string text, string[] ruby)
        {
            var config = generatorConfig(nameof(JaRubyTagGeneratorConfig.RubyAsKatakana));
            RunRubyCheckTest(text, ruby, config);
        }

        [TestCase("はなび", new string[] { "はな", "び" })]
        [TestCase("ハナビ", new string[] { "はなび" })]
        public void TestCreateRubyTagsWithEnableDuplicatedRuby(string text, string[] ruby)
        {
            var config = generatorConfig(nameof(JaRubyTagGeneratorConfig.EnableDuplicatedRuby));
            RunRubyCheckTest(text, ruby, config);
        }

        #region test helper

        protected void RunRubyCheckTest(string text, string[] ruby, JaRubyTagGeneratorConfig config)
        {
            var generator = new JaRubyTagGenerator(config);

            var lyric = new Lyric { Text = text };
            var rubyTags = generator.CreateRubyTags(lyric);

            Assert.AreEqual(rubyTags.Length, ruby.Length);
            foreach (var rubyTag in rubyTags)
            {
                Assert.IsTrue(ruby.Contains(rubyTag.Text));
            }
        }

        private JaRubyTagGeneratorConfig generatorConfig(params string[] properties)
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
