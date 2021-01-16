// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeJsonBeatmapDecoder : JsonBeatmapDecoder
    {
        protected override void ParseStreamInto(LineBufferedReader stream, Beatmap output)
        {
            var convertor = new List<JsonConverter>
            {
                new CultureInfoConverter(),
                new RomajiTagConverter(),
                new RubyTagConverter(),
                new TimeTagConverter(),
                new ToneConverter(),
            };

            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            convertor.AddRange(globalSetting.Converters);
            globalSetting.Converters = convertor.ToArray();

            // create id if object is by reference.
            globalSetting.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            // replace string stream.ReadToEnd().DeserializeInto(output);
            JsonConvert.PopulateObject(stream.ReadToEnd(), output, globalSetting);

            base.ParseStreamInto(stream, output);
        }
    }
}
