// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class CultureInfoConverter : JsonConverter<CultureInfo>
    {
        public override CultureInfo ReadJson(JsonReader reader, Type objectType, CultureInfo existingValues, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var value = obj.Value<int?>();

            if (value == null)
                return null;

            return new CultureInfo(value.Value);
        }

        public override void WriteJson(JsonWriter writer, CultureInfo values, JsonSerializer serializer)
        {
            writer.WriteValue(values?.LCID ?? null);
        }
    }
}
