// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class SpecialActionSection<TAction> : Section where TAction : struct, Enum
    {
        protected sealed override string Title => "Action";

        private readonly Bindable<TAction> bindableModeSpecialAction = new();

        protected SpecialActionSection()
        {
            Children = new[]
            {
                new LabelledSpecialActionSelection
                {
                    Label = SwitchActionTitle,
                    Description = SwitchActionDescription,
                    Current = bindableModeSpecialAction,
                }
            };

            bindableModeSpecialAction.BindValueChanged(e =>
            {
                UpdateActionArea(e.NewValue);
            }, true);
        }

        protected void BindTo(IHasSpecialAction<TAction> specialAction)
        {
            bindableModeSpecialAction.BindTo(specialAction.BindableSpecialAction);
        }

        protected abstract string SwitchActionTitle { get; }

        protected abstract string SwitchActionDescription { get; }

        protected abstract void UpdateActionArea(TAction action);

        private class LabelledSpecialActionSelection : LabelledEnumDropdown<TAction>
        {
            public LabelledSpecialActionSelection()
            {
                // should change the component size because LabelledDropdown use 0.5 as drawable with scale.
                Component.Width = 1;
            }
        }
    }
}
