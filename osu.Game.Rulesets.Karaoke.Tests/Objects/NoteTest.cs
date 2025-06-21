// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects;

public class NoteTest
{
    [Test]
    public void TestClone()
    {
        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "ノート", 1000, 1000);
        var note = new Note
        {
            Text = "ノート",
            RubyText = "Note",
            Display = true,
            StartTimeOffset = 100,
            EndTimeOffset = -100,
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = 0,
        };

        var clonedNote = note.DeepClone();

        Assert.That(clonedNote.TextBindable, Is.Not.SameAs(note.TextBindable));
        Assert.That(clonedNote.Text, Is.EqualTo(note.Text));

        Assert.That(clonedNote.RubyTextBindable, Is.Not.SameAs(note.RubyTextBindable));
        Assert.That(clonedNote.RubyText, Is.EqualTo(note.RubyText));

        Assert.That(clonedNote.DisplayBindable, Is.Not.SameAs(note.DisplayBindable));
        Assert.That(clonedNote.Display, Is.EqualTo(note.Display));

        Assert.That(clonedNote.ToneBindable, Is.Not.SameAs(note.ToneBindable));
        Assert.That(clonedNote.Tone, Is.EqualTo(note.Tone));

        // note time will not being copied because the time is based on the time-tag in the lyric.
        Assert.That(clonedNote.StartTimeBindable, Is.Not.SameAs(note.StartTimeBindable));
        Assert.That(clonedNote.StartTime, Is.EqualTo(clonedNote.StartTime));

        // note time will not being copied because the time is based on the time-tag in the lyric.
        Assert.That(clonedNote.Duration, Is.EqualTo(clonedNote.Duration));

        // note time will not being copied because the time is based on the time-tag in the lyric.
        Assert.That(clonedNote.EndTime, Is.EqualTo(clonedNote.EndTime));

        Assert.That(clonedNote.StartTimeOffset, Is.EqualTo(note.StartTimeOffset));

        Assert.That(clonedNote.EndTimeOffset, Is.EqualTo(note.EndTimeOffset));

        Assert.That(clonedNote.ReferenceLyric, Is.SameAs(note.ReferenceLyric));

        Assert.That(clonedNote.ReferenceTimeTagIndexBindable, Is.Not.SameAs(note.ReferenceTimeTagIndexBindable));
        Assert.That(clonedNote.ReferenceTimeTagIndex, Is.EqualTo(note.ReferenceTimeTagIndex));
    }

    [Test]
    public void TestReferenceTime()
    {
        var note = new Note();

        // Should not have the time.
        Assert.That(note.StartTime, Is.EqualTo(0));
        Assert.That(note.Duration, Is.EqualTo(0));
        Assert.That(note.EndTime, Is.EqualTo(0));

        const double first_time_tag_time = 1000;
        const double second_time_tag_time = 3000;
        const double duration = second_time_tag_time - first_time_tag_time;
        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "Lyric", first_time_tag_time, duration);
        note.ReferenceLyricId = referencedLyric.ID;
        note.ReferenceLyric = referencedLyric;

        // Should have calculated time.
        Assert.That(note.StartTime, Is.EqualTo(first_time_tag_time));
        Assert.That(note.Duration, Is.EqualTo(duration));

        const double time_tag_offset_time = 500;
        referencedLyric.TimeTags.ForEach(x => x.Time += time_tag_offset_time);

        // Should change the time if time-tag time has been changed.
        Assert.That(note.StartTime, Is.EqualTo(first_time_tag_time + time_tag_offset_time));
        Assert.That(note.Duration, Is.EqualTo(duration));

        note.ReferenceTimeTagIndex = 1;

        // Duration will be zero if there's no next time-tag.
        Assert.That(note.StartTime, Is.EqualTo(second_time_tag_time + time_tag_offset_time));
        Assert.That(note.Duration, Is.EqualTo(0));

        note.ReferenceTimeTagIndex = 2;

        // Time will be zero if there's no matched time-tag.
        Assert.That(note.StartTime, Is.EqualTo(0));
        Assert.That(note.Duration, Is.EqualTo(0));

        const double note_start_offset_time = 500;
        const double note_end_offset_time = 500;
        note.ReferenceTimeTagIndex = 0;
        note.StartTimeOffset = note_start_offset_time;
        note.EndTimeOffset = note_end_offset_time;

        // start time and end time will apply the offset time.
        Assert.That(note.StartTime, Is.EqualTo(first_time_tag_time + time_tag_offset_time + note_start_offset_time));
        Assert.That(note.Duration, Is.EqualTo(duration + time_tag_offset_time - note_end_offset_time));

        note.EndTimeOffset = -100000;

        // duration should not be empty.
        Assert.That(note.StartTime, Is.EqualTo(first_time_tag_time + time_tag_offset_time + note_start_offset_time));
        Assert.That(note.Duration, Is.EqualTo(0));

        note.ReferenceLyricId = null;
        note.ReferenceLyric = null;

        // time will be zero if lyric has been removed.
        Assert.That(note.StartTime, Is.EqualTo(0));
        Assert.That(note.Duration, Is.EqualTo(0));
    }
}
