// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.Converters.Add(new CultureInfoConverter());
            globalSetting.Converters.Add(new RomajiTagConverter());
            globalSetting.Converters.Add(new RomajiTagsConverter());
            globalSetting.Converters.Add(new RubyTagConverter());
            globalSetting.Converters.Add(new RubyTagsConverter());
            globalSetting.Converters.Add(new TimeTagConverter());
            globalSetting.Converters.Add(new TimeTagsConverter());
            globalSetting.Converters.Add(new ToneConverter());

            // replace string stream.ReadToEnd().Serialize(output);
            return JsonConvert.SerializeObject(output, globalSetting);
        }
    }
}
