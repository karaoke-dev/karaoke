// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Skinning.Components
{
    public class KaraokeSkin
    {
        public List<KaraokeFont> DefinedFonts { get; set; }

        public List<KaraokeLayout> DefinedLayouts { get; set; }

        public List<NoteSkin> DefinedNoteSkins { get; set; }
    }
}
