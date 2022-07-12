// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteClearSubsection : SelectLyricButton
    {
        [Resolved]
        private INotesChangeHandler notesChangeHandler { get; set; }

        protected override LocalisableString StandardText => "Clear";

        protected override LocalisableString SelectingText => "Cancel clear";

        protected override IDictionary<Lyric, LocalisableString> GetDisableSelectingLyrics()
        {
            // todo: should not select the lyric that not contains the note.
            return new Dictionary<Lyric, LocalisableString>();
        }

        protected override void Apply()
        {
            notesChangeHandler.Clear();
        }
    }
}
