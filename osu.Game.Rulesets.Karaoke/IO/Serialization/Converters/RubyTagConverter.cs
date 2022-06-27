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
    public class RubyTagConverter : JsonConverter<RubyTag>
    {
        public override RubyTag ReadJson(JsonReader reader, Type objectType, RubyTag existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            string value = obj.Value<string>();

            if (string.IsNullOrEmpty(value))
                return new RubyTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<ruby>.*$)");
            var result = regex.Match(value);
            if (!result.Success)
                return new RubyTag();

            return new RubyTag
            {
                StartIndex = result.GetGroupValue<int>("start"),
                EndIndex = result.GetGroupValue<int>("end"),
                Text = result.GetGroupValue<string>("ruby")
            };
        }

        public override void WriteJson(JsonWriter writer, RubyTag value, JsonSerializer serializer)
        {
            string str = $"[{value.StartIndex},{value.EndIndex}]:{value.Text}";
            writer.WriteValue(str);
        }
    }
}
