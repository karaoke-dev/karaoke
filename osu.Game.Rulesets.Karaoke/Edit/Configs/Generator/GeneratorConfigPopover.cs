// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator
{
    public abstract class GeneratorConfigPopover<TConfig> : OsuPopover where TConfig : IHasConfig<TConfig>, new()
    {
        private readonly Bindable<TConfig> bindableConfig = new();

        protected GeneratorConfigPopover()
        {
            Child = new OsuScrollContainer
            {
                Height = 500,
                Width = 300,
                Child = new FillFlowContainer<GeneratorConfigSection>
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = CreateConfigSection(bindableConfig) ?? Array.Empty<GeneratorConfigSection>()
                }
            };
        }

        protected abstract KaraokeRulesetEditGeneratorSetting Config { get; }

        protected abstract GeneratorConfigSection[] CreateConfigSection(Bindable<TConfig> current);

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditGeneratorConfigManager config)
        {
            bindableConfig.BindTo(config.GetBindable<TConfig>(Config));
        }
    }
}
