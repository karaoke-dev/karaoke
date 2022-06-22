// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TypeUtils
    {
        public static TType ChangeType<TType>(object value)
        {
            if (value == null)
                return default;

            var type = typeof(TType);
            return (TType)Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? type);
        }
    }
}
