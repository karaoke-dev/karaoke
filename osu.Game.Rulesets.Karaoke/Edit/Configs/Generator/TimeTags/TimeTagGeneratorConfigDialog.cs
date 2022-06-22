// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags
{
    public abstract class TimeTagGeneratorConfigDialog<T> : GeneratorConfigDialog<T> where T : IHasConfig<T>, new()
    {
        protected override OverlayColourScheme OverlayColourScheme => OverlayColourScheme.Blue;

        protected override string Title => "Time-tag generator config";

        protected override string Description => "Change config for time-tag generator.";
    }
}
