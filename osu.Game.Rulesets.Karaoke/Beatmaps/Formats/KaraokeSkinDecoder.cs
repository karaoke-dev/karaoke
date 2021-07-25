// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeSkinDecoder : Decoder<KaraokeSkin>
    {
        protected override void ParseStreamInto(LineBufferedReader stream, KaraokeSkin output)
        {
            var settings = JsonSerializableExtensions.CreateGlobalSettings();
            settings.Converters.Add(new FontUsageConvertor());

            var skinText = stream.ReadToEnd();
            var result = JsonConvert.DeserializeObject<KaraokeSkin>(skinText, settings);

            if (result == null)
                return;

            // Copy property
            output.Fonts = result.Fonts;
            output.Layouts = result.Layouts;
            output.NoteSkins = result.NoteSkins;
            output.LayoutGroups = result.LayoutGroups;
        }
    }
}
