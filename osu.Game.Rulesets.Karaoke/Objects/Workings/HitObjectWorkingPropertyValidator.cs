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
        invalidateCannotCheckSyncProperties();
    }

    /// <summary>
    /// We need to invalidate the properties that can't check working property sync.
    /// For able to apply the value in the <see cref="KaraokeBeatmapProcessor"/>
    /// </summary>
    private void invalidateCannotCheckSyncProperties()
    {
        foreach (TFlag flag in Enum.GetValues(typeof(TFlag)))
        {
            if (HasDataProperty(flag))
                continue;

            Invalidate(flag);
        }
    }

    /// <summary>
    /// This method is called after change the data property.
    /// We should make sure that the working property is same as data property.
    /// Note that this property should only called inside the <typeparamref name="THitObject"/>
    /// </summary>
    /// <param name="flag"></param>
    public bool UpdateStateByDataProperty(TFlag flag)
    {
        if (!CanInvalidate(flag))
        {
            // will caused if data property become same as working property again.
            Validate(flag);
        }

        return Invalidate(flag);
    }

    /// <summary>
    /// This method is called after assign the working property changed in the <typeparamref name="THitObject"/> by <see cref="KaraokeBeatmapProcessor"/>.
    /// We should make sure that the working property is same as data property.
    /// Note that this property should only called inside the <typeparamref name="THitObject"/>
    /// </summary>
    /// <param name="flag"></param>
    public bool UpdateStateByWorkingProperty(TFlag flag)
    {
        if (!CanValidate(flag))
            throw new InvalidWorkingPropertyAssignException();

        return Validate(flag);
    }

    protected sealed override bool CanInvalidate(TFlag flags)
        => !HasDataProperty(flags) || !IsWorkingPropertySynced(hitObject, flags);

    protected sealed override bool CanValidate(TFlag flags)
        => !HasDataProperty(flags) || IsWorkingPropertySynced(hitObject, flags);

    protected abstract bool HasDataProperty(TFlag flags);

    protected abstract bool IsWorkingPropertySynced(THitObject hitObject, TFlag flags);
}
