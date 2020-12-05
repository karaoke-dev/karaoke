// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using System;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class ToneConverter : JsonConverter<Tone>
    {
        public override Tone ReadJson(JsonReader reader, Type objectType, Tone existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var value = obj.Value<double>();

            var half = Math.Abs(value) % 1 == 0.5;
            var scale = (int)value - (value < 0 && half ? 1 : 0);

            return new Tone
            {
                Scale = scale,
                Half = half
            };
        }

        public override void WriteJson(JsonWriter writer, Tone value, JsonSerializer serializer)
        {
            var scale = value.Scale + (value.Half ? 0.5 : 0);
            writer.WriteValue(scale);
        }
    }
}
