// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class SingerDictionary : KaraokeHitObject
    {
        public IDictionary<int, Singer> Singers { get; } = new Dictionary<int, Singer>();

        public class Singer
        {
            public string Name { get; set; }

            public string Romaji { get; set; }

            public string EnglishName { get; set; }

            public Color4 Color { get; set; }
        }
    }
}
