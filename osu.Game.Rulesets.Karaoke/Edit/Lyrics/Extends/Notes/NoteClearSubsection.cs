// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteClearSubsection : SelectLyricButton
    {
        protected override LocalisableString StandardText => "Clear";

        protected override LocalisableString SelectingText => "Cancel clear";

        [BackgroundDependencyLoader]
        private void load(ILyricSelectionState lyricSelectionState, INotesChangeHandler notesChangeHandler)
        {
            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                notesChangeHandler.Clear();
            };
        }
    }
}
