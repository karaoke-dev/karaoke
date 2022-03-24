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
        public LanguageSelectorPopover(Bindable<CultureInfo> bindable)
        {
            Child = new LanguageSelector
            {
                Width = 400,
                Height = 600,
                Current = bindable
            };
        }
    }
}
