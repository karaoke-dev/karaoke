// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

public class BindableEnumMenuItem<T> : MenuItem where T : struct, Enum
{
    private readonly Bindable<T> bindableEnum = new();

    public BindableEnumMenuItem(string text, Bindable<T> bindable)
        : base(text)
    {
        Items = createMenuItems();

        bindableEnum.BindTo(bindable);
        bindableEnum.BindValueChanged(e =>
        {
            var newSelection = e.NewValue;
            Items.OfType<ToggleMenuItem>().ForEach(x =>
            {
                bool match = x.Text.Value == GetName(newSelection);
                x.State.Value = match;
            });
        }, true);
    }

    private ToggleMenuItem[] createMenuItems()
    {
        return ValidEnums.Select(e =>
        {
            var item = new ToggleMenuItem(GetName(e), MenuItemType.Standard, _ => UpdateSelection(e));
            return item;
        }).ToArray();
    }

    protected virtual IEnumerable<T> ValidEnums => Enum.GetValues<T>();

    protected string GetName(T selection)
        => selection.GetDescription();

    protected virtual void UpdateSelection(T selection)
    {
        bindableEnum.Value = selection;
    }
}
