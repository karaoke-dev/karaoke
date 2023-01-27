// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes
{
    public partial class NotePropertyChangeHandler : HitObjectPropertyChangeHandler<Note>, INotePropertyChangeHandler
    {
        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

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

        public void OffsetTone(Tone offset)
        {
            if (offset == default(Tone))
                throw new InvalidOperationException("Offset number should not be zero.");

            var noteInfo = EditorBeatmapUtils.GetPlayableBeatmap(beatmap).NoteInfo;

            PerformOnSelection(note =>
            {
                if (note.Tone >= noteInfo.MaxTone && offset > 0)
                    return;

                if (note.Tone <= noteInfo.MinTone && offset < 0)
                    return;

                note.Tone += offset;

                //Change all note to visible
                note.Display = true;
            });
        }

        protected override bool IsWritePropertyLocked(Note note)
            => HitObjectWritableUtils.IsWriteNotePropertyLocked(note, nameof(Note.Text), nameof(Note.RubyText), nameof(Note.Display));
    }
}
