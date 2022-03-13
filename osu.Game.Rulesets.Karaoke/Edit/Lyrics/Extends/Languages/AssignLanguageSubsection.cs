// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class AssignLanguageSubsection : SelectLyricButton
    {
        protected override string StandardText => "Change language";

        protected override string SelectingText => "Cancel change language";

        [BackgroundDependencyLoader]
        private void load(ILyricSelectionState lyricSelectionState, ILyricLanguageChangeHandler lyricLanguageChangeHandler)
        {
            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                // todo: should have a popover for user to select the language.
                var language = new CultureInfo("Ja-jp");
                lyricLanguageChangeHandler.SetLanguage(language);
            };
        }
    }
}
