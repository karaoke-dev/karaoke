// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class TimeTagsConverter : JsonConverter<TimeTag[]>
    {
        public override TimeTag[] ReadJson(JsonReader reader, Type objectType, TimeTag[] existingValues, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JArray.Load(reader);

            return obj.Select(line =>
            {
                var value = line.ToString();
                return deserializeTuple(value);
            }).ToArray();

            TimeTag deserializeTuple(string str)
            {
                var strArray = str.Split(',');

                var state = strArray[1] == "0" ? TimeTagIndex.IndexState.Start : TimeTagIndex.IndexState.End;
                var timeTag = new TimeTagIndex(int.Parse(strArray[0]), state);
                var time = strArray[2] != "" ? int.Parse(strArray[2]) : default(double?);
                return new TimeTag(timeTag, time);
            }
        }

        public override void WriteJson(JsonWriter writer, TimeTag[] values, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            foreach (var value in values)
            {
                writer.WriteValue(serializeTuple(value));
            }

            writer.WriteEndArray();

            string serializeTuple(TimeTag timeTagTuple)
            {
                var tag = timeTagTuple.Item1;
                var state = tag.State == TimeTagIndex.IndexState.Start ? "0" : "1";
                var time = timeTagTuple.Item2;
                return $"{tag.Index},{state},{time}";
            }
        }
    }
}
