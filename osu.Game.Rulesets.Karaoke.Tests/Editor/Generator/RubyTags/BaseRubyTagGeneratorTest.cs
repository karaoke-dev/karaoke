// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.RubyTags
{
    public abstract class BaseRubyTagGeneratorTest<TRubyTagGenerator, TConfig> : BaseGeneratorTest<TRubyTagGenerator, RubyTag[], TConfig>
        where TRubyTagGenerator : RubyTagGenerator<TConfig> where TConfig : RubyTagGeneratorConfig, new()
    {
        protected static void CheckCanGenerate(string text, bool canGenerate, TConfig config)
        {
            var lyric = new Lyric { Text = text };
            CheckCanGenerate(lyric, canGenerate, config);
        }

        protected void CheckGenerateResult(string text, string[] expectedRubies, TConfig config)
        {
            var expected = TestCaseTagHelper.ParseRubyTags(expectedRubies);
            var lyric = new Lyric { Text = text };
            CheckGenerateResult(lyric, expected, config);
        }

        protected override void AssertEqual(RubyTag[] expected, RubyTag[] actual)
        {
            TextTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
