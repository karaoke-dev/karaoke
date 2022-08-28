// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class LyricConvertor : JsonConverter<Lyric>
    {
        public override Lyric ReadJson(JsonReader reader, Type objectType, Lyric? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var newReader = jObject.CreateReader();

            var instance = (Lyric)Activator.CreateInstance(objectType);
            serializer.Populate(newReader, instance);
            return instance;
        }

        public override void WriteJson(JsonWriter writer, Lyric? value, JsonSerializer serializer)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            // follow: https://stackoverflow.com/a/59329703
            // not a good way but seems there's no better choice.
            serializer.Converters.Remove(this);

            var jObject = JObject.FromObject(value, serializer);

            serializer.Converters.Add(this);

            // should remove some properties from jObject if has the config.
            if (value.ReferenceLyricConfig != null)
            {
                Debug.Assert(value.ReferenceLyricConfig != null);

                // note: should convert into snake case.
                string[] removedProperties = removePropertyNamesByConfig(value.ReferenceLyricConfig)
                                             .Select(x => x.ToSnakeCase())
                                             .ToArray();

                foreach (string removedProperty in removedProperties)
                {
                    jObject.Remove(removedProperty);
                }
            }

            jObject.WriteTo(writer);
        }

        private IEnumerable<string> removePropertyNamesByConfig(IReferenceLyricPropertyConfig config)
        {
            switch (config)
            {
                case ReferenceLyricConfig:
                    yield break;

                case SyncLyricConfig syncLyricConfig:
                    yield return nameof(Lyric.Text);

                    if (syncLyricConfig.SyncTimeTagProperty)
                        yield return nameof(Lyric.TimeTags);

                    yield return nameof(Lyric.RubyTags);
                    yield return nameof(Lyric.RomajiTags);
                    yield return nameof(Lyric.StartTime);
                    yield return nameof(Lyric.Duration);
                    yield return nameof(Lyric.EndTime);

                    if (syncLyricConfig.SyncSingerProperty)
                        yield return nameof(Lyric.Singers);

                    yield return nameof(Lyric.Translates);
                    yield return nameof(Lyric.Language);
                    yield return nameof(Lyric.Order);

                    yield break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(config), config, "unknown config.");
            }
        }
    }
}
