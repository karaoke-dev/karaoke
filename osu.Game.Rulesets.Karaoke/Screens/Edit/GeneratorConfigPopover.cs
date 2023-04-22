// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.UserInterface;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public partial class GeneratorConfigPopover : OsuPopover
{
    private const string default_category_name = "General";

    private readonly KaraokeRulesetEditGeneratorSetting setting;

    private readonly FillFlowContainer<GeneratorConfigSection> sections;

    public GeneratorConfigPopover(KaraokeRulesetEditGeneratorSetting setting)
    {
        this.setting = setting;

        Child = new OsuScrollContainer
        {
            Height = 500,
            Width = 300,
            Child = sections = new FillFlowContainer<GeneratorConfigSection>
            {
                Direction = FillDirection.Vertical,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            }
        };
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetEditGeneratorConfigManager config)
    {
        var generatorConfig = config.GetGeneratorConfig(setting);
        sections.Children = createConfigsSections(generatorConfig).ToArray();
    }

    private static IEnumerable<GeneratorConfigSection> createConfigsSections(GeneratorConfig config)
    {
        var defaultCategory = new ConfigCategoryAttribute(default_category_name);

        foreach (var (category, properties) in config.GetOrderedConfigsSourceDictionary(defaultCategory))
        {
            yield return new GeneratorConfigSection
            {
                Title = category.Category,
                Children = properties.Select(x =>
                {
                    object value = x.Item2.GetValue(config)!;
                    return createControl(value, x.Item1);
                }).ToArray()
            };
        }
    }

    private static Drawable createControl(object value, ConfigSourceAttribute attribute)
    {
        return value switch
        {
            BindableNumber<int> bInt => new LabelledSliderBar<int>
            {
                Label = attribute.Label,
                Description = attribute.Description,
                Current = bInt,
            },
            BindableNumber<float> bFloat => new LabelledSliderBar<float>
            {
                Label = attribute.Label,
                Description = attribute.Description,
                Current = bFloat,
            },
            BindableNumber<double> bDouble => new LabelledSliderBar<double>
            {
                Label = attribute.Label,
                Description = attribute.Description,
                Current = bDouble,
            },
            Bindable<bool> bBool => new LabelledSwitchButton
            {
                Label = attribute.Label,
                Description = attribute.Description,
                Current = bBool
            },
            Bindable<CultureInfo[]> bCultureInfos => new LanguagesSelector
            {
                Current = bCultureInfos
            },
            _ => throw new InvalidOperationException($"{nameof(SettingSourceAttribute)} was attached to an unsupported type ({value})")
        };
    }

    public partial class GeneratorConfigSection : Container
    {
        private readonly FillFlowContainer flow;
        private readonly OsuSpriteText title;

        protected override Container<Drawable> Content => flow;

        public LocalisableString Title
        {
            get => title.Text;
            set => title.Text = value;
        }

        public GeneratorConfigSection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            Padding = new MarginPadding(10);

            InternalChildren = new Drawable[]
            {
                title = new OsuSpriteText
                {
                    Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 18),
                },
                flow = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(10),
                    Direction = FillDirection.Vertical,
                    Margin = new MarginPadding { Top = 30 }
                }
            };
        }
    }
}
