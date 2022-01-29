// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Skinning.Groups;

namespace osu.Game.Rulesets.Karaoke.Skinning.MappingRoles
{
    public class DefaultMappingRole : MappingRole
    {
        public Type ElementType { get; set; }

        public int ElementId { get; set; }

        public int GroupId { get; set; }

        public override IEnumerable<KaraokeHitObject> GetApplicableHitObjects(KaraokeBeatmapSkin beatmapSkin, IBeatmap beatmap)
        {
            var element = getElementByTypeAndId(beatmapSkin, ElementType, ElementId);
            var group = getGroupById(beatmapSkin, GroupId);

            return group.GetGroupHitObjects(beatmap, element);
        }

        private static IKaraokeSkinElement getElementByTypeAndId(KaraokeBeatmapSkin beatmapSkin, Type elementType, int elementId) =>
            elementType switch
            {
                Type t when t == typeof(LyricConfig) => beatmapSkin.BindableDefaultLyricConfig.Value, // todo: implementation
                Type t when t == typeof(LyricLayout) => beatmapSkin.BindableLayouts[elementId].Value,
                Type t when t == typeof(LyricStyle) => beatmapSkin.BindableLyricStyles[elementId].Value,
                Type t when t == typeof(NoteStyle) => beatmapSkin.BindableNoteStyles[elementId].Value,
                _ => throw new InvalidCastException(nameof(elementType))
            };

        private static IGroup getGroupById(KaraokeBeatmapSkin beatmapSkin, int groupId)
            => beatmapSkin.BindableStyleMappingRoles.FirstOrDefault(x => x.ID == groupId);
    }
}
