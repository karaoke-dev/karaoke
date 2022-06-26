// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Skinning.MappingRoles
{
    public interface IMappingRole
    {
        string Name { get; set; }

        ElementType ElementType { get; set; }

        int ElementId { get; set; }

        bool CanApply(KaraokeBeatmapSkin beatmapSkin, KaraokeHitObject hitObject, ElementType elementType);
    }
}
