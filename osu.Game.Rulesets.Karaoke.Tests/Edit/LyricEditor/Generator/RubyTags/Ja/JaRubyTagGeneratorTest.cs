// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.LyricEditor.Generator.RubyTags.Ja
{
    [TestFixture]
    public class JaRubyTagGeneratorTest
    {
        [TestCase("花火大会", new string[] { "ハナビ", "タイカイ" })]
        public void TestLyricWithCheckLineEndKeyUp(string text, string[] ruby)
        {
            var lyric = new Lyric { Text = text };
            var generator = new JaRubyTagGenerator();
            var rubyTags = generator.CreateRubyTags(lyric);

            foreach (var rubyTag in rubyTags)
            {
                Assert.IsTrue(ruby.Contains(rubyTag.Text));
            }
        }

        #region test helper

        private Lyric generateLyric(string text)
            => new Lyric { Text = text };

        #endregion
    }
}
