// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.Groups;

namespace osu.Game.Rulesets.Karaoke.Skinning.MappingRoles
{
    public class DefaultMappingRole : MappingRole
    {
        public int GroupId { get; set; }

        public override bool CanApply(KaraokeBeatmapSkin beatmapSkin, KaraokeHitObject hitObject, ElementType elementType)
        {
            if (ElementType != elementType)
                return false;

            var group = getGroupById(beatmapSkin, GroupId);
            bool inTheGroup = group?.InTheGroup(hitObject, elementType) ?? false;
            return inTheGroup;
        }

        private static IGroup? getGroupById(KaraokeBeatmapSkin beatmapSkin, int groupId)
            => beatmapSkin.Groups.FirstOrDefault(x => x.ID == groupId);
    }
}
