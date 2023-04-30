// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.IO.Serialization;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats;

public class KaraokeJsonBeatmapEncoder
{
    public string Encode(Beatmap output)
    {
        var globalSetting = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
        string json = JsonConvert.SerializeObject(output, globalSetting);
        return "// karaoke json file format v1" + '\n' + json;
    }
}
