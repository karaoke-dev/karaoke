// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    public class NoteTest
    {
        [TestCase]
        public void TestClone()
        {
            var referenceLyric = new Lyric();

            var note = new Note
            {
                Text = "ノート",
                RubyText = "Note",
                Display = true,
                StartTime = 1000,
                Duration = 500,
                StartTimeOffset = 100,
                EndTimeOffset = -100,
                ReferenceLyric = referenceLyric
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
            Assert.AreEqual(clonedNote.StartTime, 0);

            // note time will not being copied because the time is based on the time-tag in the lyric.
            Assert.AreEqual(clonedNote.Duration, 0);

            // note time will not being copied because the time is based on the time-tag in the lyric.
            Assert.AreEqual(clonedNote.EndTime, 0);

            Assert.AreEqual(clonedNote.StartTimeOffset, note.StartTimeOffset);

            Assert.AreEqual(clonedNote.EndTimeOffset, note.EndTimeOffset);

            Assert.AreSame(clonedNote.ReferenceLyric, note.ReferenceLyric);

            Assert.AreNotSame(clonedNote.ReferenceTimeTagIndexBindable, note.ReferenceTimeTagIndexBindable);
            Assert.AreEqual(clonedNote.ReferenceTimeTagIndex, note.ReferenceTimeTagIndex);
        }
    }
}
