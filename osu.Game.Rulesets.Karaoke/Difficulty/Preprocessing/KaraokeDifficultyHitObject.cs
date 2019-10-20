// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Difficulty.Preprocessing
{
    public class KaraokeDifficultyHitObject : DifficultyHitObject
    {
        public new KaraokeNote BaseObject => (KaraokeNote)base.BaseObject;

        public KaraokeDifficultyHitObject(HitObject hitObject, HitObject lastObject, double clockRate)
            : base(hitObject, lastObject, clockRate)
        {
        }
    }
}
