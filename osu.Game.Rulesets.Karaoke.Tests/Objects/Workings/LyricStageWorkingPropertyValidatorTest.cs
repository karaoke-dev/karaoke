// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public class LyricStageWorkingPropertyValidatorTest : HitObjectWorkingPropertyValidatorTest<Lyric, LyricStageWorkingProperty, StageInfo>
{
    [Test]
    public void TestStartTime()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.StartTime = 1000);
        AssetIsValid(lyric, LyricStageWorkingProperty.StartTime, true);
    }

    [Test]
    public void TestDuration()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.Duration = 1000);
        AssetIsValid(lyric, LyricStageWorkingProperty.Duration, true);
    }

    [Test]
    public void TestTiming()
    {
        var lyric = new Lyric();

        // state is still invalid because duration is not assign.
        Assert.DoesNotThrow(() => lyric.StartTime = 1000);
        AssetIsValid(lyric, LyricStageWorkingProperty.Timing, false);

        // ok, should be valid now.
        Assert.DoesNotThrow(() => lyric.Duration = 1000);
        AssetIsValid(lyric, LyricStageWorkingProperty.Timing, true);
    }

    [Test]
    public void TestEffectApplier()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.EffectApplier = new LyricClassicStageEffectApplier(Array.Empty<StageElement>(), new ClassicStageDefinition()));
        AssetIsValid(lyric, LyricStageWorkingProperty.EffectApplier, true);
    }

    protected override bool IsInitialStateValid(LyricStageWorkingProperty flag)
    {
        return new LyricStageWorkingPropertyValidator(new Lyric()).IsValid(flag);
    }
}
