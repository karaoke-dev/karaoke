// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types
{
    public interface ISinger : IHasOrder
    {
        int ID { get; }

        int Hue { get; }
    }
}
