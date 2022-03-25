// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings
{
    public class SettingsLanguage : SettingsItem<CultureInfo>
    {
        protected override Drawable CreateControl() => new LanguageSelectionButton
        {
            RelativeSizeAxes = Axes.X,
        };

        private class LanguageSelectionButton : SettingsButton, IHasCurrentValue<CultureInfo>, IHasPopover
        {
            private readonly BindableWithCurrent<CultureInfo> current = new();

            public Bindable<CultureInfo> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public LanguageSelectionButton()
            {
                Height = 30;
                Action = this.ShowPopover;

                Current.BindValueChanged(e => Text = CultureInfoUtils.GetLanguageDisplayText(e.NewValue));
            }

            public Popover GetPopover()
                => new LanguageSelectorPopover(Current);
        }

        private class LanguageSelectorPopover : OsuPopover
        {
            public LanguageSelectorPopover(Bindable<CultureInfo> bindableCultureInfo)
            {
                Child = new LanguageSelector
                {
                    Width = 400,
                    Height = 600,
                    Current = bindableCultureInfo
                };
            }
        }
    }
}
