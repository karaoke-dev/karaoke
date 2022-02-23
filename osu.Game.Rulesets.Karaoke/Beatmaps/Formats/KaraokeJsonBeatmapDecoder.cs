// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeJsonBeatmapDecoder : JsonBeatmapDecoder
    {
        public new static void Register()
        {
            AddDecoder<Beatmap>("// karaoke json file format v", m => new KaraokeJsonBeatmapDecoder());

            // use this weird way to let all the fall-back beatmap(include karaoke beatmap) become karaoke beatmap.
            SetFallbackDecoder<Beatmap>(() => new KaraokeJsonBeatmapDecoder());
        }

        protected override void ParseStreamInto(LineBufferedReader stream, Beatmap output)
        {
            var globalSetting = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.ContractResolver = new KaraokeBeatmapContractResolver();

            // create id if object is by reference.
            globalSetting.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            // also notice that it might have a bug that cannot deserializer the reference normally, so should add those options.
            // https://stackoverflow.com/questions/25853407/json-net-not-respecting-preservereferenceshandling-on-deserialization
            // demo: https://dotnetfiddle.net/j1Qhu6
            // discussion: https://github.com/JamesNK/Newtonsoft.Json/issues/124
            globalSetting.TypeNameHandling = TypeNameHandling.Auto;

            // should not let json decoder to read this line.
            if (stream.PeekLine()?.Contains("// karaoke json file format v") ?? false)
            {
                stream.ReadLine();
            }

            // equal to stream.ReadToEnd().DeserializeInto(output); in the base class.
            JsonConvert.PopulateObject(stream.ReadToEnd(), output, globalSetting);
            var lyrics = output.HitObjects.OfType<Lyric>().ToArray();

            foreach (var hitObject in output.HitObjects)
            {
                if (hitObject is Note note)
                {
                    // because of json serializer contains object reference issue with serialize/deserialize the beatmap.
                    // so should re-assign the lyric instance.
                    note.ReferenceLyric = lyrics.FirstOrDefault(x => x.ID == note.ReferenceLyric?.ID) ?? throw new InvalidOperationException();
                }

                hitObject.ApplyDefaults(output.ControlPointInfo, output.Difficulty);
            }

            var notes = output.HitObjects.OfType<Note>();
        }

        private class KaraokeBeatmapContractResolver : SnakeCaseKeyContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = base.CreateProperties(type, memberSerialization);

                if (type == typeof(BeatmapInfo))
                {
                    return props.Where(p => p.PropertyName != "ruleset_id").ToList();
                }

                return props;
            }
        }
    }
}
