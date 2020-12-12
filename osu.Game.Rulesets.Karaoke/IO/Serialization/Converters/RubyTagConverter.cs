// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class RubyTagConverter : JsonConverter<RubyTag>
    {
        public override RubyTag ReadJson(JsonReader reader, Type objectType, RubyTag existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var value = obj.Value<string>();

            if (string.IsNullOrEmpty(value))
                return new RubyTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<ruby>.*$)");
            var result = regex.Match(value);
            if (!result.Success)
                return new RubyTag();

            var startIndex = int.Parse(result.Groups["start"]?.Value);
            var endIndex = int.Parse(result.Groups["end"]?.Value);
            var text = result.Groups["ruby"]?.Value;

            return new RubyTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
        }

        public override void WriteJson(JsonWriter writer, RubyTag value, JsonSerializer serializer)
        {
            var str = $"[{value.StartIndex},{value.EndIndex}]:{value.Text}";
            writer.WriteValue(str);
        }
    }
}
