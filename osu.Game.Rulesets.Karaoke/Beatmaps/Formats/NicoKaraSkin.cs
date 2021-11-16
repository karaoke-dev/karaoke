// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Notes;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class NicoKaraSkin
    {
        public List<LayoutGroup> LayoutGroups { get; set; }

        public List<LyricStyle> Styles { get; set; }

        public List<LyricLayout> Layouts { get; set; }

        public List<NoteSkin> NoteSkins { get; set; }
    }
}
