// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

public class BindableBoolMenuItem : ToggleMenuItem
{
    public BindableBoolMenuItem(Bindable<bool> bindable, string text)
        : base(text)
    {
        State.BindTo(bindable);
    }
}
