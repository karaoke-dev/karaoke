// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeJsonBeatmapEncoder
    {
        public string Encode(Beatmap output)
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

            // replace string stream.ReadToEnd().Serialize(output);
            return JsonConvert.SerializeObject(output, globalSetting);
        }
    }
}
