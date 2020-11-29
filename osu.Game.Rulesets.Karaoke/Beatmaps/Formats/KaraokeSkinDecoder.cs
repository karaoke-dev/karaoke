// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeSkinDecoder : Decoder<KaraokeSkin>
    {
        protected override void ParseStreamInto(LineBufferedReader stream, KaraokeSkin output)
        {
            var skinText = stream.ReadToEnd();
            var result = skinText.Deserialize<KaraokeSkin>();

            // Copy properties
            output.Fonts = result.Fonts;
            output.Layouts = result.Layouts;
            output.NoteSkins = result.NoteSkins;
        }
    }
}
