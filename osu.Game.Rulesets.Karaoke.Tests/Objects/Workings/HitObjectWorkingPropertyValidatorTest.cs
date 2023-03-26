// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Workings;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public abstract class HitObjectWorkingPropertyValidatorTest<THitObject, TFlag>
    where TFlag : struct, Enum
    where THitObject : KaraokeHitObject, new()
{
    protected abstract HitObjectWorkingPropertyValidator<THitObject, TFlag> GetValidatorFromHitObject(THitObject hitObject);

    [Test]
    public void RunAllInvalidateTest([Values] TFlag flag)
    {
        // run this test case just make sure that all working property are checked.
        var validator = GetValidatorFromHitObject(new THitObject());
        Assert.DoesNotThrow(() => validator.Invalidate(flag));
    }

    [Test]
    public void RunAllValidateTest([Values] TFlag flag)
    {
        // run this test case just make sure that all working property are checked.
        var validator = GetValidatorFromHitObject(new THitObject());
        Assert.DoesNotThrow(() => validator.Validate(flag));
    }

    protected void AssetIsValid(THitObject hitObject, TFlag flag, bool isValid)
    {
        var validator = GetValidatorFromHitObject(hitObject);
        Assert.AreEqual(isValid, validator.IsValid(flag));
    }
}
