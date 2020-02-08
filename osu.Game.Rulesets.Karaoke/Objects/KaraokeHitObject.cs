// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class KaraokeHitObject : HitObject
    {
        public double TimePreempt = 600;
        public double TimeFadeIn = 400;

        protected override HitWindows CreateHitWindows() => new KaraokeHitWindows();
    }
}
