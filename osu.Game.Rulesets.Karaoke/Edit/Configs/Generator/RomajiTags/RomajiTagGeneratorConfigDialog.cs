// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RomajiTags
{
    public abstract class RomajiTagGeneratorConfigDialog<T> : GeneratorConfigDialog<T> where T : IHasConfig<T>, new()
    {
        protected override OverlayColourScheme OverlayColourScheme => OverlayColourScheme.Pink;

        protected override string Title => "Romaji generator config";

        protected override string Description => "Change config for romaji generator.";
    }
}
