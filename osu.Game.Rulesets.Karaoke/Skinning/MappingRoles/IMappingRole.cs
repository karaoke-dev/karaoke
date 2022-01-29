// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Skinning.MappingRoles
{
    public interface IMappingRole
    {
        string Name { get; set; }

        IEnumerable<KaraokeHitObject> GetApplicableHitObjects(KaraokeBeatmapSkin beatmapSkin, IBeatmap beatmap);
    }
}
