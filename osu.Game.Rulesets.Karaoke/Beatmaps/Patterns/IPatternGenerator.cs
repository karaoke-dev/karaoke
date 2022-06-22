// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Patterns
{
    public interface IPatternGenerator<in TObject> where TObject : KaraokeHitObject
    {
        void Generate(IEnumerable<TObject> hitObjects);
    }
}
