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
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator
{
    public abstract class GeneratorConfigDialog<T> : OsuFocusedOverlayContainer where T : IHasConfig<T>, new()
    {
        private const float section_scale = 0.75f;

        private readonly Bindable<T> bindableConfig = new Bindable<T>();

        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        protected GeneratorConfigDialog()
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Green);

            // todo : also has apply, cancel and reset button.
            RelativeSizeAxes = Axes.Both;
            Child = new Container
            {
                Name = "Layout adjustment area",
                Width = 400,
                Height = 600,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                CornerRadius = 10,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Colour = ColourProvider.Background2,
                        RelativeSizeAxes = Axes.Both,
                    },
                    new SectionsContainer<GeneratorConfigSection>
                    {
                        FixedHeader = new GeneratorConfigScreenHeader(),
                        Footer = new GeneratorConfigScreenFooter(),
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(section_scale),
                        Size = new Vector2(1 / section_scale),
                        Children = CreateConfigSection(bindableConfig) ?? new GeneratorConfigSection[] { }
                    }
                }
            };
        }

        protected abstract KaraokeRulesetEditGeneratorSetting Config { get; }

        protected abstract GeneratorConfigSection[] CreateConfigSection(Bindable<T> current);

        [BackgroundDependencyLoader]
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

        internal class GeneratorConfigScreenFooter : Container
        {
            public GeneratorConfigScreenFooter()
            {
                Height = 45;
                RelativeSizeAxes = Axes.X;
            }
        }
    }
}
