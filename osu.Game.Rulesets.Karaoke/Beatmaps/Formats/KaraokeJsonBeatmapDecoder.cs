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

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats;

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

        // should not let json decoder to read this line.
        if (stream.PeekLine()?.Contains("// karaoke json file format v") ?? false)
        {
            stream.ReadLine();
        }

        // equal to stream.ReadToEnd().DeserializeInto(output); in the base class.
        JsonConvert.PopulateObject(stream.ReadToEnd(), output, globalSetting);
    }

    private class KaraokeBeatmapContractResolver : SnakeCaseKeyContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);

            return type == typeof(BeatmapInfo)
                ? props.Where(p => p.PropertyName != "ruleset_id").ToList()
                : props;
        }
    }
}
