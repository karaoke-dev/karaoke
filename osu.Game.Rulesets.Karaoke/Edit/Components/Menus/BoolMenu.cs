// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus;

public class BoolMenu : ToggleMenuItem
{
    public BoolMenu(Bindable<bool> bindable, string text)
        : base(text)
    {
        State.BindTo(bindable);
    }
}
