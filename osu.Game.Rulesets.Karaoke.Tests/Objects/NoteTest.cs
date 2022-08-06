// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    public class NoteTest
    {
        [TestCase]
        public void TestClone()
        {
            var note = new Note
            {
                Text = "ノート",
                RubyText = "Note",
                Display = true,
                StartTimeOffset = 100,
                EndTimeOffset = -100,
                ReferenceLyric = TestCaseNoteHelper.CreateLyricForNote("ノート", 1000, 1000),
                ReferenceTimeTagIndex = 0,
            };

            var clonedNote = note.DeepClone();

            Assert.AreNotSame(clonedNote.TextBindable, note.TextBindable);
            Assert.AreEqual(clonedNote.Text, note.Text);

            Assert.AreNotSame(clonedNote.RubyTextBindable, note.RubyTextBindable);
            Assert.AreEqual(clonedNote.RubyText, note.RubyText);

            Assert.AreNotSame(clonedNote.DisplayBindable, note.DisplayBindable);
            Assert.AreEqual(clonedNote.Display, note.Display);

            Assert.AreNotSame(clonedNote.ToneBindable, note.ToneBindable);
            Assert.AreEqual(clonedNote.Tone, note.Tone);

            // note time will not being copied because the time is based on the time-tag in the lyric.
            Assert.AreNotSame(clonedNote.StartTimeBindable, note.StartTimeBindable);
            Assert.AreEqual(clonedNote.StartTime, clonedNote.StartTime);

            // note time will not being copied because the time is based on the time-tag in the lyric.
            Assert.AreEqual(clonedNote.Duration, clonedNote.Duration);

            // note time will not being copied because the time is based on the time-tag in the lyric.
            Assert.AreEqual(clonedNote.EndTime, clonedNote.EndTime);

            Assert.AreEqual(clonedNote.StartTimeOffset, note.StartTimeOffset);

            Assert.AreEqual(clonedNote.EndTimeOffset, note.EndTimeOffset);

            Assert.AreSame(clonedNote.ReferenceLyric, note.ReferenceLyric);

            Assert.AreNotSame(clonedNote.ReferenceTimeTagIndexBindable, note.ReferenceTimeTagIndexBindable);
            Assert.AreEqual(clonedNote.ReferenceTimeTagIndex, note.ReferenceTimeTagIndex);
        }

        [TestCase]
        public void TestReferenceTime()
        {
            var note = new Note();

            // Should not have the time.
            Assert.AreEqual(0, note.StartTime);
            Assert.AreEqual(0, note.Duration);
            Assert.AreEqual(0, note.EndTime);

            const double first_time_tag_time = 1000;
            const double second_time_tag_time = 3000;
            const double duration = second_time_tag_time - first_time_tag_time;
            var lyric = TestCaseNoteHelper.CreateLyricForNote("Lyric", first_time_tag_time, duration);
            note.ReferenceLyric = lyric;

            // Should have calculated time.
            Assert.AreEqual(first_time_tag_time, note.StartTime);
            Assert.AreEqual(duration, note.Duration);

            const double time_tag_offset_time = 500;
            lyric.TimeTags.ForEach(x => x.Time += time_tag_offset_time);

            // Should change the time if time-tag time has been changed.
            Assert.AreEqual(first_time_tag_time + time_tag_offset_time, note.StartTime);
            Assert.AreEqual(duration, note.Duration);

            note.ReferenceTimeTagIndex = 1;

            // Duration will be zero if there's no next time-tag.
            Assert.AreEqual(second_time_tag_time + time_tag_offset_time, note.StartTime);
            Assert.AreEqual(0, note.Duration);

            note.ReferenceTimeTagIndex = 2;

            // Time will be zero if there's no matched time-tag.
            Assert.AreEqual(0, note.StartTime);
            Assert.AreEqual(0, note.Duration);

            const double note_start_offset_time = 500;
            const double note_end_offset_time = 500;
            note.ReferenceTimeTagIndex = 0;
            note.StartTimeOffset = note_start_offset_time;
            note.EndTimeOffset = note_end_offset_time;

            // start time and end time will apply the offset time.
            Assert.AreEqual(first_time_tag_time + time_tag_offset_time + note_start_offset_time, note.StartTime);
            Assert.AreEqual(duration + time_tag_offset_time - note_end_offset_time, note.Duration);

            note.EndTimeOffset = -100000;

            // duration should not be empty.
            Assert.AreEqual(first_time_tag_time + time_tag_offset_time + note_start_offset_time, note.StartTime);
            Assert.AreEqual(0, note.Duration);

            note.ReferenceLyric = null;

            // time will be zero if lyric has been removed.
            Assert.AreEqual(0, note.StartTime);
            Assert.AreEqual(0, note.Duration);
        }
    }
}
