// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Layouts
{
    public class LayoutGeneratorConfig : IHasConfig<LayoutGeneratorConfig>
    {
        public double NewLyricLineTime { get; set; }

        public LayoutGeneratorConfig CreateDefaultConfig()
        {
            return new LayoutGeneratorConfig();
        }
    }
}
