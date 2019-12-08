// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class BarLine : KaraokeHitObject, IBarLine
    {
        public bool Major { get; set; }
    }
}
