// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class FontUsageConvertor : JsonConverter<FontUsage>
    {
        private const float default_text_size = 20;

        public override FontUsage ReadJson(JsonReader reader, Type objectType, FontUsage existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var properties = obj.Children().OfType<JProperty>().ToArray();

            if (!properties.Any())
                return new FontUsage(size: default_text_size);

            var font = new FontUsage(size: default_text_size);

            return properties.Aggregate(font, (current, property) => property.Name switch
            {
                "family" => current.With(property.Value.ToObject<string>()),
                "weight" => current.With(weight: property.Value.ToObject<string>()),
                "size" => current.With(size: property.Value.ToObject<float>()),
                "italics" => current.With(italics: property.Value.ToObject<bool>()),
                "fixedWidth" => current.With(fixedWidth: property.Value.ToObject<bool>()),
                _ => current
            });
        }

        public override void WriteJson(JsonWriter writer, FontUsage value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            if (!string.IsNullOrEmpty(value.Family))
            {
                writer.WritePropertyName("family");
                writer.WriteValue(value.Family);
            }

            if (!string.IsNullOrEmpty(value.Weight))
            {
                writer.WritePropertyName("weight");
                writer.WriteValue(value.Weight);
            }

            if (value.Size != default_text_size)
            {
                writer.WritePropertyName("size");
                writer.WriteValue(value.Size);
            }

            if (value.Italics)
            {
                writer.WritePropertyName("italics");
                writer.WriteValue(true);
            }

            if (value.FixedWidth)
            {
                writer.WritePropertyName("fixedWidth");
                writer.WriteValue(true);
            }

            writer.WriteEndObject();
        }
    }
}
