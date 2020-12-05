// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using System;
using System.Text.RegularExpressions;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class RomajiTagConverter : JsonConverter<RomajiTag>
    {
        public override RomajiTag ReadJson(JsonReader reader, Type objectType, RomajiTag existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var value = obj.Value<string>();

            if (value == null || value == "")
                return new RomajiTag();

            var regux = new Regex("([-0-9]+),([-0-9]+)]:(.*$)");
            var result = regux.Match(value);
            if (!result.Success)
                return new RomajiTag();

            var startIndex = int.Parse(result.Groups[1].Value);
            var endIndex = int.Parse(result.Groups[2].Value);
            var text = result.Groups[3].Value;


            return new RomajiTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
        }

        public override void WriteJson(JsonWriter writer, RomajiTag value, JsonSerializer serializer)
        {
            var str = $"[{value.StartIndex},{value.EndIndex}]:{value.Text}";
            writer.WriteValue(str);
        }
    }
}
