// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class ToneConverter : JsonConverter<Tone>
    {
        public override Tone ReadJson(JsonReader reader, Type objectType, Tone existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            double value = obj.Value<double>();

            bool half = Math.Abs(value) % 1 == 0.5;
            int scale = (int)value - (value < 0 && half ? 1 : 0);

            return new Tone
            {
                Scale = scale,
                Half = half
            };
        }

        public override void WriteJson(JsonWriter writer, Tone value, JsonSerializer serializer)
        {
            double scale = value.Scale + (value.Half ? 0.5 : 0);
            writer.WriteValue(scale);
        }
    }
}
