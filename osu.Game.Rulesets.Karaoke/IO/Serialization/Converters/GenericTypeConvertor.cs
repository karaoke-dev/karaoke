// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using Newtonsoft.Json;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public abstract class GenericTypeConvertor<TType> : JsonConverter<TType>
    {
        protected abstract Type GetTypeByName(string name);

        protected virtual string GetNameByType(MemberInfo type)
            => type.Name;
    }
}
