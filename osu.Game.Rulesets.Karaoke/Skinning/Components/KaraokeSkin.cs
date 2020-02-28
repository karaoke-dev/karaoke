// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.IO.Serialization;

namespace osu.Game.Rulesets.Karaoke.Skinning.Components
{
    public class KaraokeSkin : IJsonSerializable
    {
        public List<KaraokeFont> DefinedFonts { get; set; }

        public List<KaraokeLayout> DefinedLayouts { get; set; }

        public List<NoteSkin> DefinedNoteSkins { get; set; }

        public List<Singer> Singers { get; set; }
    }
}
