// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class NoteUtilsTest
    {
        [TestCase(new double[] { 1000, 3000 }, 0, 1, new double[] { 1000, 3000 })]
        [TestCase(new double[] { 1000, 3000 }, 0, 0.5, new double[] { 1000, 1500 })]
        [TestCase(new double[] { 1000, 3000 }, 0.5, 0.5, new double[] { 2500, 1500 })]
        [TestCase(new double[] { 1000, 3000 }, 0.3, 0.4, new double[] { 1900, 1200 })]
        [TestCase(new double[] { 1000, 3000 }, 0.3, 1, null)] // start + duration should not exceed 1
        public void TestSliceNoteTime(double[] time, double startPercentage, double durationPercentage, double[] actualTime)
        {
            var note = new Note
            {
                StartTime = time[0],
                Duration = time[1],
            };

            try
            {
                var sliceNote = NoteUtils.SliceNote(note, startPercentage, durationPercentage);
                Assert.AreEqual(sliceNote.StartTime, actualTime[0]);
                Assert.AreEqual(sliceNote.Duration, actualTime[1]);
            }
            catch
            {
                Assert.IsNull(actualTime);
            }
        }

        [TestCase(new double[] { 1000, 5000 }, 0.2, new double[] { 1000, 1000 }, new double[] { 2000, 4000 })]
        [TestCase(new double[] { 1000, 5000 }, 0.5, new double[] { 1000, 2500 }, new double[] { 3500, 2500 })]
        [TestCase(new double[] { 1000, 0 }, 0.2, new double[] { 1000, 0 }, new double[] { 1000, 0 })] // it's ok to split if duration is 0.
        [TestCase(new double[] { 1000, 0 }, 0.7, new double[] { 1000, 0 }, new double[] { 1000, 0 })]
        [TestCase(new double[] { 1000, 5000 }, -1, null, null)] // should be in the range.
        [TestCase(new double[] { 1000, 5000 }, 3, null, null)]
        [TestCase(new double[] { 1000, 5000 }, 0, null, null)] // should not be 0 or 1.
        [TestCase(new double[] { 1000, 5000 }, 1, null, null)]
        public void TestSeparateNoteTime(double[] time, double percentage, double[] firstTime, double[] secondTime)
        {
            var note = new Note
            {
                StartTime = time[0],
                Duration = time[1],
            };

            try
            {
                var (firstNote, secondNote) = NoteUtils.SplitNote(note, percentage);
                Assert.AreEqual(firstNote.StartTime, firstTime[0]);
                Assert.AreEqual(firstNote.Duration, firstTime[1]);

                Assert.AreEqual(secondNote.StartTime, secondTime[0]);
                Assert.AreEqual(secondNote.Duration, secondTime[1]);
            }
            catch
            {
                Assert.IsNull(firstTime);
                Assert.IsNull(secondTime);
            }
        }

        [Test]
        public void TestSeparateLyricOtherPtoperty()
        {
            var lyric = new Lyric();

            const double percentage = 0.3;
            var note = new Note
            {
                StartTime = 1000,
                Duration = 2000,
                StartIndex = 1,
                EndIndex = 2,
                Text = "ka",
                Singers = new int[] { 0 },
                Display = false,
                Tone = new Tone(-1, true),
                ParentLyric = lyric
            };

            // create other property and make sure other class is applied value.
            var (firstNote, secondNote) = NoteUtils.SplitNote(note, percentage);

            Assert.AreEqual(firstNote.StartTime, 1000);
            Assert.AreEqual(secondNote.StartTime, 1600);

            Assert.AreEqual(firstNote.Duration, 600);
            Assert.AreEqual(secondNote.Duration, 1400);

            testRemainProperty(firstNote, note);
            testRemainProperty(firstNote, note);

            static void testRemainProperty(Note expect, Note actual)
            {
                Assert.AreEqual(expect.StartIndex, actual.StartIndex);
                Assert.AreEqual(expect.EndIndex, actual.EndIndex);
                Assert.AreEqual(expect.Text, actual.Text);

                Assert.AreEqual(expect.Singers, actual.Singers);
                Assert.AreNotSame(expect.Singers, actual.Singers);

                Assert.AreEqual(expect.Display, actual.Display);
                Assert.AreEqual(expect.Tone, actual.Tone);
                Assert.AreEqual(expect.ParentLyric, actual.ParentLyric);
            }
        }
    }
}
