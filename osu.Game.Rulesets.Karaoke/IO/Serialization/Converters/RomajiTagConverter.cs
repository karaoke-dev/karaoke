// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class RomajiTagConverter : JsonConverter<RomajiTag>
    {
        public override RomajiTag ReadJson(JsonReader reader, Type objectType, RomajiTag? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            string? value = obj.Value<string>();

            if (string.IsNullOrEmpty(value))
                return new RomajiTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<romaji>.*$)");
            var result = regex.Match(value);
            if (!result.Success)
                return new RomajiTag();

            return new RomajiTag
            {
                StartIndex = result.GetGroupValue<int>("start"),
                EndIndex = result.GetGroupValue<int>("end"),
                Text = result.GetGroupValue<string>("romaji")
            };
        }

        public override void WriteJson(JsonWriter writer, RomajiTag? value, JsonSerializer serializer)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            string str = $"[{value.StartIndex},{value.EndIndex}]:{value.Text}";
            writer.WriteValue(str);
        }
    }
}
