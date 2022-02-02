// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class NicoKaraSkin
    {
        public LyricConfig DefaultLyricConfig { get; set; } = LyricConfig.DEFAULT;

        public List<LyricLayout> Layouts { get; set; }

        public List<LyricStyle> LyricStyles { get; set; }
    }
}
