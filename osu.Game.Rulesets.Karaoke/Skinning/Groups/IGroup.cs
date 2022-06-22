// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Skinning.Groups
{
    public interface IGroup
    {
        public int ID { get; set; }

        public string Name { get; set; }

        bool InTheGroup(KaraokeHitObject hitObject, ElementType elementType);
    }
}
