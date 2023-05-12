// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Components.UserInterfaceV2;

public partial class LanguageSelectorPopover : OsuPopover
{
    private readonly LanguageSelector languageSelector;

    public LanguageSelectorPopover(Bindable<CultureInfo?> bindable)
    {
        Child = languageSelector = new LanguageSelector
        {
            Width = 260,
            Height = 400,
            Current = bindable
        };
    }

    public bool EnableEmptyOption
    {
        get => languageSelector.EnableEmptyOption;
        set => languageSelector.EnableEmptyOption = value;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        GetContainingInputManager().ChangeFocus(languageSelector);
    }
}
