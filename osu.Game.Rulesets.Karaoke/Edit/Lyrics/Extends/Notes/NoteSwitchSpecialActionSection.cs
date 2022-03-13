// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteSwitchSpecialActionSection : Section
    {
        protected sealed override string Title => "Action";

        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            Children = new[]
            {
                new LabelledEnumDropdown<NoteEditModeSpecialAction>
                {
                    Label = "Switch special actions",
                    Description = "Auto-generate, edit or clear the notes.",
                    Current = editNoteModeState.BindableSpecialAction,
                }
            };
        }
    }
}
