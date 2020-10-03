// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas
{
    public class Singer : ISinger
    {
        public Singer(int id)
        {
            ID = id;
        }

        public int ID { get; private set; }

        public string Name { get; set; }

        public string RomajiName { get; set; }

        public string EnglishName { get; set; }

        public Color4 Color { get; set; }

        public string Avater { get; set; }

        public string Description { get; set; }
    }
}
