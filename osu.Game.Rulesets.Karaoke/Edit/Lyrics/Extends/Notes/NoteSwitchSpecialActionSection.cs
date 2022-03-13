// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteSwitchSpecialActionSection : SpecialActionSection<NoteEditModeSpecialAction>
    {
        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            BindTo(editNoteModeState);

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

        protected override void UpdateActionArea(NoteEditModeSpecialAction action)
        {
            RemoveAll(x => x is NoteAutoGenerateSubsection or NoteClearSubsection);

            switch (action)
            {
                case NoteEditModeSpecialAction.AutoGenerate:
                    Add(new NoteAutoGenerateSubsection());
                    break;

                case NoteEditModeSpecialAction.SyncTime:
                    // todo: implement
                    break;

                case NoteEditModeSpecialAction.Clear:
                    Add(new NoteClearSubsection());
                    break;

                default:
                    return;
            }
        }
    }
}
