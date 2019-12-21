// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModHiddenRuby : Mod, IApplicableToHitObject
    {
        public override string Name => "HiddenRuby";
        public override string Acronym => "HR";
        public override double ScoreMultiplier => 1.0;
        public override IconUsage Icon => FontAwesome.Solid.Gem;
        public override ModType Type => ModType.DifficultyIncrease;

        public void ApplyToHitObject(HitObject hitObject)
        {
            if (hitObject is LyricLine lyricLine)
            {
                // Clear all ruby tag
                lyricLine.RubyTags = Array.Empty<RubyTag>();
            }
        }
    }
}
