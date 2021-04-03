// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class SettingsLanguage : SettingsItem<CultureInfo>
    {
        protected override Drawable CreateControl() => new LanguageSelectionButton
        {
            Padding = new MarginPadding { Top = 5},
            RelativeSizeAxes = Axes.X,
        };

        internal class LanguageSelectionButton : TriangleButton, IHasCurrentValue<CultureInfo>
        {
            [Resolved(canBeNull: true)]
            protected OsuGame Game { get; private set; }

            private readonly BindableWithCurrent<CultureInfo> current = new BindableWithCurrent<CultureInfo>();

            public Bindable<CultureInfo> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public LanguageSelectionButton()
            {
                Action = () =>
                {
                    try
                    {
                        var displayContainer = Game.GetDisplayContainer();
                        if (displayContainer == null)
                            return;

                        // Should only has one instance.
                        var dialog = displayContainer.Children.OfType<LanguageSelectionDialog>().FirstOrDefault();
                        if (dialog == null)
                        {
                            displayContainer.Add(dialog = new LanguageSelectionDialog());
                        }

                        dialog.Current = Current;
                        dialog?.Show();
                    }
                    catch
                    {
                        // maybe this overlay has been moved into internal.
                    }
                };

                Current.BindValueChanged(e => Text = e.NewValue.DisplayName);
            }
        }
    }
}
