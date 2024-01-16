// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romanization;

public abstract class BaseRomanizationGeneratorTest<TRomanizationGenerator, TConfig> : BaseLyricGeneratorTest<TRomanizationGenerator, IReadOnlyDictionary<TimeTag, RomanizationGenerateResult>, TConfig>
    where TRomanizationGenerator : RomanizationGenerator<TConfig> where TConfig : RomanizationGeneratorConfig, new()
{
    protected void CheckGenerateResult(Lyric lyric, string[] expectedRubies, TConfig config)
    {
        var expected = RomanizationGenerateResultHelper.ParseRomanizationGenerateResults(lyric.TimeTags, expectedRubies);
        CheckGenerateResult(lyric, expected, config);
    }

    protected override void AssertEqual(IReadOnlyDictionary<TimeTag, RomanizationGenerateResult> expected, IReadOnlyDictionary<TimeTag, RomanizationGenerateResult> actual)
    {
        TimeTagAssert.ArePropertyEqual(expected.Select(x => x.Key).ToArray(), actual.Select(x => x.Key).ToArray());
        Assert.AreEqual(expected.Select(x => x.Value), actual.Select(x => x.Value));
    }
}
