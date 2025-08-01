﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romanisation;

public abstract class BaseRomanisationGeneratorTest<TRomanisationGenerator, TConfig> : BaseLyricGeneratorTest<TRomanisationGenerator, IReadOnlyDictionary<TimeTag, RomanisationGenerateResult>, TConfig>
    where TRomanisationGenerator : RomanisationGenerator<TConfig> where TConfig : RomanisationGeneratorConfig, new()
{
    protected void CheckGenerateResult(Lyric lyric, string[] expectedRubies, TConfig config)
    {
        var expected = RomanisationGenerateResultHelper.ParseRomanisationGenerateResults(lyric.TimeTags, expectedRubies);
        CheckGenerateResult(lyric, expected, config);
    }

    protected override void AssertEqual(IReadOnlyDictionary<TimeTag, RomanisationGenerateResult> expected, IReadOnlyDictionary<TimeTag, RomanisationGenerateResult> actual)
    {
        TimeTagAssert.ArePropertyEqual(expected.Select(x => x.Key).ToArray(), actual.Select(x => x.Key).ToArray());
        Assert.That(actual.Select(x => x.Value), Is.EqualTo(expected.Select(x => x.Value)));
    }
}
