// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class SpecialActionSection<TAction> : Section where TAction : Enum
    {
        protected sealed override string Title => "Action";

        private readonly IBindable<TAction> bindableModeSpecialAction = new Bindable<TAction>();

        protected SpecialActionSection()
        {
            bindableModeSpecialAction.BindValueChanged(e =>
            {
                Schedule(() =>
                {
                    UpdateActionArea(e.NewValue);
                });
            }, true);
        }

        protected void BindTo(IHasSpecialAction<TAction> specialAction)
        {
            bindableModeSpecialAction.UnbindBindings();
            bindableModeSpecialAction.BindTo(specialAction.BindableSpecialAction);
        }

        protected abstract void UpdateActionArea(TAction action);
    }
}
