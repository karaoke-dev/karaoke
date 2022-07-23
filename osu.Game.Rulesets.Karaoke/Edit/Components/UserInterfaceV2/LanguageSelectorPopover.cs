// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2
{
    public class LanguageSelectorPopover : OsuPopover
    {
        private readonly LanguageSelector languageSelector;

        public LanguageSelectorPopover(Bindable<CultureInfo> bindable)
        {
            Child = languageSelector = new LanguageSelector
            {
                Width = 400,
                Height = 600,
                Current = bindable
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            GetContainingInputManager().ChangeFocus(languageSelector);
        }
    }
}
