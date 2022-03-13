// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteClearSection : Section
    {
        protected sealed override string Title => "Clear";

        [BackgroundDependencyLoader]
        private void load(ILyricSelectionState lyricSelectionState, INotesChangeHandler notesChangeHandler)
        {
            Children = new Drawable[]
            {
                new ClearNotesButton
                {
                    StartSelecting = () => new Dictionary<Lyric, string>()
                },
            };

            lyricSelectionState.Action = e =>
            {
                if (e != LyricEditorSelectingAction.Apply)
                    return;

                notesChangeHandler.Clear();
            };
        }

        private class ClearNotesButton : SelectLyricButton
        {
            protected override string StandardText => "Clear";

            protected override string SelectingText => "Cancel clear";
        }
    }
}
