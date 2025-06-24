// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public abstract class HitObjectWorkingPropertyValidatorTest<THitObject, TFlag>
    where TFlag : struct, Enum
    where THitObject : KaraokeHitObject, IHasWorkingProperty<TFlag>, new()
{
    [Test]
    public void TestInitialState([Values] TFlag flag)
    {
        // should be valid on the first load.
        AssetInitialStateIsValid(new THitObject(), flag);
    }

    [Test]
    public void TestAllInvalidateTest([Values] TFlag flag)
    {
        // run this test case just make sure that all working property are checked.
        Assert.That(() => new THitObject().InvalidateWorkingProperty(flag), Throws.Nothing);
    }

    protected void AssetInitialStateIsValid(THitObject hitObject, TFlag flag)
    {
        bool isValid = IsInitialStateValid(flag);
        AssetIsValid(hitObject, flag, isValid);
    }

    protected void AssetIsValid(THitObject hitObject, TFlag flag, bool isValid)
    {
        Assert.That(!hitObject.GetAllInvalidWorkingProperties().Contains(flag), Is.EqualTo(isValid));
    }

    protected abstract bool IsInitialStateValid(TFlag flag);
}
