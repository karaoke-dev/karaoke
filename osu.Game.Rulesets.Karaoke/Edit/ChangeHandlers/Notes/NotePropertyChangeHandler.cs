// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes
{
    public class NotePropertyChangeHandler : HitObjectPropertyChangeHandler<Note>, INotePropertyChangeHandler
    {
        public void ChangeText(string text)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(note =>
            {
                note.Text = text;
            });
        }

        public void ChangeRubyText(string ruby)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(note =>
            {
                // Should change ruby text as null if remove all words.
                note.RubyText = string.IsNullOrEmpty(ruby) ? null : ruby;
            });
        }

        public void ChangeDisplayState(bool display)
        {
            PerformOnSelection(note =>
            {
                note.Display = display;

                // Move to center if note is not display
                if (!note.Display)
                    note.Tone = new Tone();
            });
        }

        protected override bool IsWritePropertyLocked(Note note)
            => HitObjectWritableUtils.IsWriteNotePropertyLocked(note, nameof(Note.Text) , nameof(Note.RubyText) , nameof(Note.Display));
    }
}
