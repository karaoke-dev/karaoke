// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config;

internal partial class PositionSection : LyricConfigSection
{
    private readonly LabelledEnumDropdown<KaraokeTextSmartHorizon> smartHorizonDropdown;

    protected override LocalisableString Title => "Position";

    public PositionSection()
    {
        Children = new Drawable[]
        {
            smartHorizonDropdown = new LabelledEnumDropdown<KaraokeTextSmartHorizon>
            {
                Label = "Smart horizon",
                Description = "Smart horizon section",
            }
        };
    }

    [BackgroundDependencyLoader]
    private void load(LyricFontInfoManager lyricFontInfoManager)
    {
        lyricFontInfoManager.LoadedLyricFontInfo.BindValueChanged(e =>
        {
            var lyricFontInfo = e.NewValue;
            applyCurrent(smartHorizonDropdown.Current, lyricFontInfo.SmartHorizon);

            static void applyCurrent<T>(Bindable<T> bindable, T value)
                => bindable.Value = bindable.Default = value;
        }, true);

        smartHorizonDropdown.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.SmartHorizon = x.NewValue));
    }
}
