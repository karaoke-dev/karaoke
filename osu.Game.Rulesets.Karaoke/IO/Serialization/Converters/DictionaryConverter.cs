// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public abstract class DictionaryConverter<TKey, TValue> : JsonConverter<IDictionary<TKey, TValue>> where TKey : notnull
    {
        public sealed override IDictionary<TKey, TValue> ReadJson(JsonReader reader, Type objectType, IDictionary<TKey, TValue>? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JArray.Load(reader);
            return obj.OfType<JObject>().ToDictionary(
                x => deserializeKey((JProperty)x.First!),
                x => deserializeValue((JProperty)x.Last!)
            );

            TKey deserializeKey(JProperty token)
                => serializer.Deserialize<TKey>(token.Value.CreateReader())!;

            TValue deserializeValue(JProperty token)
                => serializer.Deserialize<TValue>(token.Value.CreateReader())!;
        }

        public override void WriteJson(JsonWriter writer, IDictionary<TKey, TValue>? value, JsonSerializer serializer)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            writer.WriteStartArray();

            foreach (var keyValuePair in value)
            {
                var jObject = JObject.FromObject(keyValuePair, serializer);
                jObject.WriteTo(writer);
            }

            writer.WriteEndArray();
        }
    }
}
