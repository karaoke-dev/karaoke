// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Skinning.Components
{
    public class KaraokeSkin : IJsonSerializable
    {
        public List<KaraokeFont> Fonts { get; set; }

        public List<KaraokeLayout> Layouts { get; set; }

        public List<NoteSkin> NoteSkins { get; set; }

        public List<Singer> Singers { get; set; }
    }
}
