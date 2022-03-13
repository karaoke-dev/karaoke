// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Manage
{
    public class ManageSwitchSpecialActionSection : SpecialActionSection<ManageEditModeSpecialAction>
    {
        protected override string SwitchActionTitle => "Special actions";

        protected override string SwitchActionDescription => "Copy, delete or move the lyrics.";

        [BackgroundDependencyLoader]
        private void load(IManageModeState manageModeState)
        {
            BindTo(manageModeState);
        }

        protected override void UpdateActionArea(ManageEditModeSpecialAction action)
        {
            RemoveAll(x => x is ManageDeleteSubsection);

            switch (action)
            {
                case ManageEditModeSpecialAction.Copy:
                    // todo: implement
                    break;

                case ManageEditModeSpecialAction.Delete:
                    Add(new ManageDeleteSubsection());
                    break;

                case ManageEditModeSpecialAction.Move:
                    // todo: implement
                    break;

                default:
                    return;
            }
        }
    }
}
