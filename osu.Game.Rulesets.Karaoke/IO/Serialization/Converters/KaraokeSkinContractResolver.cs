// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Skinning;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    /// <summary>
    /// This contract resolver is for save and load data from <see cref="KaraokeSkin"/>
    /// </summary>
    public class KaraokeSkinContractResolver : SnakeCaseKeyContractResolver
    {
        // we only wants to save properties that only writable.
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);
            return props.Where(p => p.Writable).ToList();
        }
    }
}
