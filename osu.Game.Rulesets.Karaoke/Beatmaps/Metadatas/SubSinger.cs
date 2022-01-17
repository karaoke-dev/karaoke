// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas
{
    public class SubSinger : ISinger
    {
        public int Order { get; set; }

        public int ID { get; private set; }

        public int Hue { get; set; }

        public int Parent { get; set; }

        public string Description { get; set; }
    }
}
