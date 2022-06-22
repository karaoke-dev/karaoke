// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.TimeTags
{
    public abstract class BaseTimeTagGeneratorTest<TTimeTagGenerator, TConfig> : BaseGeneratorTest<TTimeTagGenerator, TimeTag[], TConfig>
        where TTimeTagGenerator : TimeTagGenerator<TConfig> where TConfig : TimeTagGeneratorConfig, new()
    {
        protected void CheckGenerateResult(string text, string[] expectedTimeTags, TConfig config)
        {
            var expected = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            CheckGenerateResult(text, expected, config);
        }

        protected void CheckGenerateResult(Lyric lyric, string[] expectedTimeTags, TConfig config)
        {
            var expected = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            CheckGenerateResult(lyric, expected, config);
        }

        protected override void AssertEqual(TimeTag[] expected, TimeTag[] actual)
        {
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
