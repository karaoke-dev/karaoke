// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Patterns
{
    public interface IPatternGenerator<T>
    {
        void Generate(IEnumerable<T> hitObjects);
    }
}
