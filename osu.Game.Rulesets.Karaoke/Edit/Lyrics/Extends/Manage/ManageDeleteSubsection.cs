// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Manage
{
    public class ManageDeleteSubsection : SelectLyricButton
    {
        protected override LocalisableString StandardText => "Delete";

        protected override LocalisableString SelectingText => "Cancel delete";

        [BackgroundDependencyLoader]
        private void load(ILyricSelectionState lyricSelectionState, ILyricsChangeHandler lyricsChangeHandler)
        {
            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                lyricsChangeHandler.Remove();
            };
        }
    }
}
