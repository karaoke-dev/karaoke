// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    // todo: should remove this encoder eventually
    public class KaraokeSkinEncoder
    {
        public string Encode(NicoKaraSkin output)
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.Converters.Add(new ShaderConvertor());
            settings.Converters.Add(new FontUsageConvertor());

            return JsonConvert.SerializeObject(output, settings);
        }
    }
}
