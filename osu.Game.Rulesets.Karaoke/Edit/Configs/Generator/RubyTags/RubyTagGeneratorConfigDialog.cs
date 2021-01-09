// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RubyTags
{
    public abstract class RubyTagGeneratorConfigDialog<T> : GeneratorConfigDialog<T> where T : IHasConfig<T>, new()
    {
        protected override OverlayColourScheme OverlayColourScheme => OverlayColourScheme.Purple;

        protected override string Title => "Ruby generator config";

        protected override string Description => "Change config for ruby generator.";
    }
}
