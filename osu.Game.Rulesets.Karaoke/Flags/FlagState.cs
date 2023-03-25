// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Flags;

public class FlagState<TFlag> where TFlag : struct, Enum
{
    private int value;

    public void Invalidate(TFlag flags)
    {
        value &= ~Convert.ToInt32(flags);
    }

    public void InvalidateAll()
    {
        value = 0;
    }

    public void Validate(TFlag flags)
    {
        value |= Convert.ToInt32(flags);
    }

    public void ValidateAll()
    {
        InvalidateAll();

        foreach (int flag in Enum.GetValues(typeof(TFlag)))
        {
            value |= flag;
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
}
