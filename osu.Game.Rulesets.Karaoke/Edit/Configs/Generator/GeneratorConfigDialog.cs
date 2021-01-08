// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator
{
    public abstract class GeneratorConfigDialog<T> : TitleFocusedOverlayContainer where T : IHasConfig<T>, new()
    {
        private const float section_scale = 0.75f;

        private readonly Bindable<T> bindableConfig = new Bindable<T>();

        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        public GeneratorConfigDialog()
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Green);

            var defaultConfig = new T().CreateDefaultConfig();
            var selections = CreateConfigSection(bindableConfig, defaultConfig);

            // enable to assign layou by property.

            // todo : add selection into drawable
            // todo : sldo has scroll-bar in here.

            // also has apply, cancel and reset button.

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.3f),
                    new Dimension(GridSizeMode.Distributed)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new SectionsContainer<GeneratorConfigSection>
                        {
                            FixedHeader = new GeneratorConfigScreenHeader(),
                            RelativeSizeAxes = Axes.Both,
                            Scale = new Vector2(section_scale),
                            Size = new Vector2(1 / section_scale),
                            Children = new GeneratorConfigSection[]
                            {
                            }
                        }
                    },
                    new Drawable[]
                    {
                        new Container
                        {
                            Name = "Layout adjustment area",
                            RelativeSizeAxes = Axes.Both,
                            Masking = true,
                            CornerRadius = 10,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    Colour = ColourProvider.Background2,
                                    RelativeSizeAxes = Axes.Both,
                                },
                            }
                        },
                    }
                },
            };
        }

        protected abstract KaraokeRulesetEditGeneratorSetting Config { get; }

        protected abstract GeneratorConfigSection[] CreateConfigSection(Bindable<T> current, T defaultConfig);

        private void load(KaraokeRulesetEditGeneratorConfigManager config)
        {
            bindableConfig.BindTo(config.GetBindable<T>(Config));
        }

        internal class GeneratorConfigScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new GeneratorConfigScreenTitle();

            private class GeneratorConfigScreenTitle : OverlayTitle
            {
                public GeneratorConfigScreenTitle()
                {
                    Title = "Config";
                    Description = "config aaa";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}
