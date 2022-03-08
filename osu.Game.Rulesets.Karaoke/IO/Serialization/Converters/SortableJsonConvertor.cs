// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public abstract class SortableJsonConvertor<TObject> : JsonConverter<IEnumerable<TObject>>
    {
        public sealed override IEnumerable<TObject> ReadJson(JsonReader reader, Type objectType, IEnumerable<TObject> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JArray.Load(reader);
            var timeTags = obj.Select(x => serializer.Deserialize<TObject>(x.CreateReader()));
            return GetSortedValue(timeTags);
        }

        public override void WriteJson(JsonWriter writer, IEnumerable<TObject> value, JsonSerializer serializer)
        {
            // see: https://stackoverflow.com/questions/3330989/order-of-serialized-fields-using-json-net
            var sortedTimeTags = GetSortedValue(value);

            writer.WriteStartArray();

            foreach (var timeTag in sortedTimeTags)
            {
                serializer.Serialize(writer, timeTag);
            }

            writer.WriteEndArray();
        }

        protected abstract IList<TObject> GetSortedValue(IEnumerable<TObject> objects);
    }
}
