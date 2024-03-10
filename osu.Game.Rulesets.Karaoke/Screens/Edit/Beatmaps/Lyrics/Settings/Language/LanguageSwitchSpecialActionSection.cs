// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language;

public partial class LanguageSwitchSpecialActionSection : SpecialActionSection<LanguageEditModeSpecialAction>
{
    protected override string SwitchActionTitle => "Special actions";
    protected override string SwitchActionDescription => "Auto-generate or batch change the language by hands.";

    [BackgroundDependencyLoader]
    private void load(IEditLanguageModeState editNoteModeState)
    {
        BindTo(editNoteModeState);
    }

    protected override void UpdateActionArea(LanguageEditModeSpecialAction action)
    {
        RemoveAll(x => x is LanguageAutoGenerateSubsection or AssignLanguageSubsection, true);

        switch (action)
        {
            case LanguageEditModeSpecialAction.AutoGenerate:
                Add(new LanguageAutoGenerateSubsection());
                break;

            case LanguageEditModeSpecialAction.BatchSelect:
                Add(new AssignLanguageSubsection());
                break;

            default:
                return;
        }
    }
}
