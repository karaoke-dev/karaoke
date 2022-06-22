// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Skinning.MappingRoles
{
    public abstract class MappingRole : IMappingRole
    {
        public string Name { get; set; }

        public ElementType ElementType { get; set; }

        public int ElementId { get; set; }

        public abstract bool CanApply(KaraokeBeatmapSkin beatmapSkin, KaraokeHitObject hitObject, ElementType elementType);
    }
}
