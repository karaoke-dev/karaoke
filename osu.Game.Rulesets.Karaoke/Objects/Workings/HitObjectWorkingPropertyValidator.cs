// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

public class HitObjectWorkingPropertyValidator<T> where T : struct, Enum
{
    private int value;

    public void Invalidate(T flags)
    {
        value &= ~Convert.ToInt32(flags);
    }

    public void InvalidateAll()
    {
        value = 0;
    }

    public void Validate(T flags)
    {
        value |= Convert.ToInt32(flags);
    }

    public void ValidateAll()
    {
        InvalidateAll();

        foreach (int flag in Enum.GetValues(typeof(T)))
        {
            value |= flag;
        }
    }

    public bool IsValid(T flags)
    {
        return (value & Convert.ToInt32(flags)) == Convert.ToInt32(flags);
    }

    public T[] GetAllValidFlags()
        => Enum.GetValues<T>().Where(IsValid).ToArray();

    public T[] GetAllInvalidFlags()
        => Enum.GetValues<T>().Where(x => !IsValid(x)).ToArray();
}
