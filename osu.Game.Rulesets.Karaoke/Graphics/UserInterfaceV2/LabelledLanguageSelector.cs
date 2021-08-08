// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2
{
    public abstract class LabelledLanguageSelector : LabelledComponent<LabelledLanguageSelector.LanguageSelectionButton, CultureInfo>
    {
        protected LabelledLanguageSelector()
            : base(true)
        {
        }

        protected override LanguageSelectionButton CreateComponent()
            => new LanguageSelectionButton();

        public class LanguageSelectionButton : OsuButton, IHasCurrentValue<CultureInfo>
        {
            [Resolved]
            protected LanguageSelectionDialog LanguageSelectionDialog { get; private set; }

            private readonly BindableWithCurrent<CultureInfo> current = new BindableWithCurrent<CultureInfo>();

            public Bindable<CultureInfo> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public LanguageSelectionButton()
            {
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;

                Action = () =>
                {
                    LanguageSelectionDialog.Current = Current;
                    LanguageSelectionDialog.Show();
                };

                Current.BindValueChanged(e => Text = e.NewValue.DisplayName);
                Current.BindDisabledChanged(e => Enabled.Value = !e);
            }
        }
    }
}
