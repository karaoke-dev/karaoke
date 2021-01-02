// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Layouts
{
    public class LayoutGenerator
    {
        protected LayoutGeneratorConfig Config { get; }

        protected LayoutGenerator(LayoutGeneratorConfig config)
        {
            Config = config;
        }

        protected void ApplyLayout(List<Lyric> lyrics, LocalLayout layout = LocalLayout.CycleTwo)
        {
            // todo : generate layout
        }

        protected void ApplyLayout(List<Lyric> lyrics, LyricLayout layouts)
        {
            // todo :
            // 1. create layout file to record group name.
            // 2. generate layout
        }
    }
}
