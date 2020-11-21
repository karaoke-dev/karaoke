// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.LyricEditor.Generator.RubyTags.Ja
{
    [TestFixture]
    public class JaRubyTagGeneratorTest
    {
        [TestCase("花火大会", new string[] { "はなび", "たいかい" }, false)]
        [TestCase("花火大会", new string[] { "ハナビ", "タイカイ" }, true)]
        public void TestLyricWithCheckLineEndKeyUpWithRubyAsKatakana(string text, string[] ruby, bool applyConfig)
        {
            var config = generatorConfig(applyConfig ? nameof(JaRubyTagGeneratorConfig.RubyAsKatakana) : null);
            var generator = new JaRubyTagGenerator(config);

            var lyric = new Lyric { Text = text };
            var rubyTags = generator.CreateRubyTags(lyric);

            foreach (var rubyTag in rubyTags)
            {
                Assert.IsTrue(ruby.Contains(rubyTag.Text));
            }
        }

        #region test helper

        private Lyric generateLyric(string text)
            => new Lyric { Text = text };

        private JaRubyTagGeneratorConfig generatorConfig(params string[] properties)
        {
            var config = new JaRubyTagGeneratorConfig();

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
