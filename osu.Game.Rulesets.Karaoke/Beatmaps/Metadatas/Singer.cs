// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas
{
    public class Singer : ISinger
    {
        public string Name { get; set; }

        public string Romaji { get; set; }

        public string EnglishName { get; set; }

        public Color4 Color { get; set; }
    }
}
