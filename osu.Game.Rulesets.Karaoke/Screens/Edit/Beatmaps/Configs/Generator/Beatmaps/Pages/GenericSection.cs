// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Beatmaps.Pages;

public partial class GenericSection : GeneratorConfigSection<PageGeneratorConfig>
{
    private readonly LabelledSliderBar<double> minTimeTextBox;
    private readonly LabelledSliderBar<double> maxTimeTextButton;
    private readonly LabelledSwitchButton clearExistPagesSwitchButton;

    protected override string Title => "Generic";

    public GenericSection(Bindable<PageGeneratorConfig> current)
        : base(current)
    {
        Children = new Drawable[]
        {
            minTimeTextBox = new LabelledSliderBar<double>
            {
                Label = "Min time",
                Description = "Min interval between pages.",
                Current = new BindableDouble
                {
                    MinValue = CheckBeatmapPageInfo.MIN_INTERVAL,
                    MaxValue = CheckBeatmapPageInfo.MAX_INTERVAL,
                    Precision = 0.1f,
                }
            },
            maxTimeTextButton = new LabelledSliderBar<double>
            {
                Label = "Max time",
                Description = "Max interval between pages.",
                Current = new BindableDouble
                {
                    MinValue = CheckBeatmapPageInfo.MIN_INTERVAL,
                    MaxValue = CheckBeatmapPageInfo.MAX_INTERVAL,
                    Precision = 0.1f,
                }
            },
            clearExistPagesSwitchButton = new LabelledSwitchButton
            {
                Label = "Clear the exist page.",
                Description = "Clear the exist page after generated.",
            },
        };

        RegisterConfig(minTimeTextBox.Current, nameof(PageGeneratorConfig.MinTime));
        RegisterConfig(maxTimeTextButton.Current, nameof(PageGeneratorConfig.MaxTime));
        RegisterConfig(clearExistPagesSwitchButton.Current, nameof(PageGeneratorConfig.ClearExistPages));
    }
}
