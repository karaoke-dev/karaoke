// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class SpecialActionSection<TAction> : LyricEditorSection where TAction : struct, Enum
    {
        protected sealed override LocalisableString Title => "Action";

        private readonly Bindable<TAction> bindableModeSpecialAction = new();

        [Resolved]
        private ILyricSelectionState lyricSelectionState { get; set; }

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
                // should cancel the selection after change to the new action.
                lyricSelectionState?.EndSelecting(LyricEditorSelectingAction.Cancel);

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
