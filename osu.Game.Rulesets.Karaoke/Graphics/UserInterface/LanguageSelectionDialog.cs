// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class LanguageSelectionDialog : TitleFocusedOverlayContainer, IHasCurrentValue<CultureInfo>
    {
        protected override string Title => "Select language";

        public Bindable<CultureInfo> Current
        {
            get => languageSelector.Current;
            set => languageSelector.Current = value;
        }

        private readonly LanguageSelector languageSelector;

        public LanguageSelectionDialog()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.5f, 0.8f);

            Child = languageSelector = new LanguageSelector
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
