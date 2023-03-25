// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Flags;

public class FlagState<TFlag> where TFlag : struct, Enum
{
    #region Public interface

    private int value;

    public bool Invalidate(TFlag flags)
    {
        if (!CanInvalidate(flags))
            return false;

        value &= ~Convert.ToInt32(flags);

        return true;
    }

    public void InvalidateAll()
    {
        foreach (TFlag flag in Enum.GetValues(typeof(TFlag)))
        {
            Invalidate(flag);
        }
    }

    public bool Validate(TFlag flags)
    {
        if (!CanValidate(flags))
            return false;

        value |= Convert.ToInt32(flags);

        return true;
    }

    public void ValidateAll()
    {
        foreach (TFlag flag in Enum.GetValues(typeof(TFlag)))
        {
            Validate(flag);
        }
    }

    public bool IsValid(TFlag flags)
    {
        return (value & Convert.ToInt32(flags)) == Convert.ToInt32(flags);
    }

    public TFlag[] GetAllValidFlags()
        => Enum.GetValues<TFlag>().Where(IsValid).ToArray();

    public TFlag[] GetAllInvalidFlags()
        => Enum.GetValues<TFlag>().Where(x => !IsValid(x)).ToArray();

    #endregion

    protected virtual bool CanInvalidate(TFlag flags) => true;

    protected virtual bool CanValidate(TFlag flags) => true;
}
