// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeSkinEncoder
    {
        public string Encode(KaraokeSkin output)
        {
            return output.Serialize();
        }
    }
}
