// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Text;

public partial class TextSwitchSpecialActionSection : SpecialActionSection<TextEditModeSpecialAction>
{
    protected override string SwitchActionTitle => "Special actions";

    protected override string SwitchActionDescription => "Copy, delete or move the lyrics.";

    [BackgroundDependencyLoader]
    private void load(IEditTextModeState editTextModeState)
    {
        BindTo(editTextModeState);
    }

    protected override void UpdateActionArea(TextEditModeSpecialAction action)
    {
        RemoveAll(x => x is TextDeleteSubsection, true);

        switch (action)
        {
            case TextEditModeSpecialAction.Copy:
                // todo: implement
                break;

            case TextEditModeSpecialAction.Delete:
                Add(new TextDeleteSubsection());
                break;

            case TextEditModeSpecialAction.Move:
                // todo: implement
                break;

            default:
                return;
        }
    }
}
