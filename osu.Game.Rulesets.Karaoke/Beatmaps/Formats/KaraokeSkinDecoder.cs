// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.IO.Serialization;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeSkinDecoder : Decoder<KaraokeSkin>
    {
        protected override void ParseStreamInto(LineBufferedReader stream, KaraokeSkin output)
        {
            var skinText = stream.ReadToEnd();
            var result = skinText.Deserialize<KaraokeSkin>();

            // Copy property
            output.DefinedFonts = result.DefinedFonts;
            output.DefinedLayouts = result.DefinedLayouts;
            output.DefinedNoteSkins = result.DefinedNoteSkins;
        }
    }
}
