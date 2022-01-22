// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes
{
    public class NotePropertyChangeHandler : HitObjectChangeHandler<Note>, INotePropertyChangeHandler
    {
        public void ChangeText(string text)
        {
            PerformOnSelection(note =>
            {
                note.Text = text;
            });
        }

        public void ChangeRubyText(string ruby)
        {
            PerformOnSelection(note =>
            {
                note.RubyText = ruby;
            });
        }

        public void ChangeDisplayState(bool display)
        {
            PerformOnSelection(note =>
            {
                note.Display = display;
            });
        }
    }
}
