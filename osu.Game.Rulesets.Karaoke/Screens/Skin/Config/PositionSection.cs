// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    internal class PositionSection : LyricConfigSection
    {
        private LabelledEnumDropdown<KaraokeTextSmartHorizon> smartHorizonDropdown;

        protected override string Title => "Position";

        [BackgroundDependencyLoader]
        private void load(LyricConfigManager manager)
        {
            Children = new Drawable[]
            {
                smartHorizonDropdown = new LabelledEnumDropdown<KaraokeTextSmartHorizon>
                {
                    Label = "Smart horizon",
                    Description = "Smart horizon section",
                }
            };

            manager.LoadedLyricConfig.BindValueChanged(e =>
            {
                var lyricConfig = e.NewValue;
                applyCurrent(smartHorizonDropdown.Current, lyricConfig.SmartHorizon);

                static void applyCurrent<T>(Bindable<T> bindable, T value)
                    => bindable.Value = bindable.Default = value;
            }, true);

            smartHorizonDropdown.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.SmartHorizon = x.NewValue));
        }
    }
}
