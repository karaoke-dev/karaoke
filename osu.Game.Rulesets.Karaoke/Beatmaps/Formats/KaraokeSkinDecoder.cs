// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    // todo: should remove this decider eventually
    public class KaraokeSkinDecoder : Decoder<NicoKaraSkin>
    {
        protected override void ParseStreamInto(LineBufferedReader stream, NicoKaraSkin output)
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.Converters.Add(new ShaderConvertor());
            settings.Converters.Add(new FontUsageConvertor());
            settings.Converters.Add(new ColourConvertor());

            string skinText = stream.ReadToEnd();
            var result = JsonConvert.DeserializeObject<NicoKaraSkin>(skinText, settings);

            if (result == null)
                return;

            // Copy property
            output.DefaultLyricConfig = result.DefaultLyricConfig;
            output.Styles = result.Styles;
            output.Layouts = result.Layouts;
            output.NoteSkins = result.NoteSkins;
            output.LayoutGroups = result.LayoutGroups;
        }
    }
}
