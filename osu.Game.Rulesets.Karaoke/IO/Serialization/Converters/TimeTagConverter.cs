// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class TimeTagConverter : JsonConverter<TimeTag>
    {
        public override TimeTag ReadJson(JsonReader reader, Type objectType, TimeTag existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var value = obj.Value<string>();

            if (string.IsNullOrEmpty(value))
                return new TimeTag(new TextIndex());

            var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)]:(?<time>[-0-9]+|s*|)");
            var result = regex.Match(value);
            if (!result.Success)
                return new TimeTag(new TextIndex());

            var index = int.Parse(result.Groups["index"]?.Value);
            var state = result.Groups["state"]?.Value == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;
            var timeStr = result.Groups["time"]?.Value;
            var time = timeStr == "" ? default(int?) : int.Parse(timeStr);

            return new TimeTag(new TextIndex(index, state), time);
        }

        public override void WriteJson(JsonWriter writer, TimeTag value, JsonSerializer serializer)
        {
            var tag = value.Index;
            var state = tag.State == TextIndex.IndexState.Start ? "start" : "end";
            var time = value.Time;

            var str = $"[{tag.Index},{state}]:{time}";
            writer.WriteValue(str);
        }
    }
}
