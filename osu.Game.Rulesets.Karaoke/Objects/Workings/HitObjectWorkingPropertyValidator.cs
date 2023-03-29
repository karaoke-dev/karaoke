// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Flags;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

/// <summary>
/// This class is used to check the working property is same as data property in the <typeparamref name="THitObject"/>.
/// Should mark as invalid when data property is changed.
/// Should mark as valid when working property is synced with data property
/// </summary>
/// <typeparam name="THitObject"></typeparam>
/// <typeparam name="TFlag"></typeparam>
public abstract class HitObjectWorkingPropertyValidator<THitObject, TFlag> : FlagState<TFlag>
    where TFlag : struct, Enum
{
    private readonly THitObject hitObject;

    protected HitObjectWorkingPropertyValidator(THitObject hitObject)
    {
        this.hitObject = hitObject;
        ValidateAll();
    }

    /// <summary>
    /// This method is called after assign the working property changed in the <typeparamref name="THitObject"/> by <see cref="KaraokeBeatmapProcessor"/>.
    /// We should make sure that the working property is same as data property.
    /// </summary>
    /// <param name="flag"></param>
    public new bool Validate(TFlag flag)
    {
        if (!CanValidate(flag))
            throw new InvalidWorkingPropertyAssignException();

        return base.Validate(flag);
    }

    protected sealed override bool CanInvalidate(TFlag flags)
        => !CanCheckWorkingPropertySync(hitObject, flags) || NeedToSyncWorkingProperty(hitObject, flags);

    protected sealed override bool CanValidate(TFlag flags)
        => !CanCheckWorkingPropertySync(hitObject, flags) || !NeedToSyncWorkingProperty(hitObject, flags);

    protected abstract bool CanCheckWorkingPropertySync(THitObject hitObject, TFlag flags);

    protected abstract bool NeedToSyncWorkingProperty(THitObject hitObject, TFlag flags);
}
