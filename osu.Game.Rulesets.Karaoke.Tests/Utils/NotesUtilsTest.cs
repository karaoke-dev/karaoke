// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class NotesUtilsTest
    {
        [TestCase(new double[] { 1000, 5000 }, 0.2, new double[] { 1000, 1000 }, new double[] { 2000, 4000 })]
        [TestCase(new double[] { 1000, 5000 }, 0.5, new double[] { 1000, 2500 }, new double[] { 3500, 2500 })]
        [TestCase(new double[] { 1000, 0 }, 0.2, new double[] { 1000, 0 }, new double[] { 1000, 0 })] // it's ok to split if duration is 0.
        [TestCase(new double[] { 1000, 0 }, 0.7, new double[] { 1000, 0 }, new double[] { 1000, 0 })]
        [TestCase(new double[] { 1000, 5000 }, -1, null, null)] // should be in the range.
        [TestCase(new double[] { 1000, 5000 }, 3, null, null)]
        [TestCase(new double[] { 1000, 5000 }, 0, null, null)] // should not be 0 or 1.
        [TestCase(new double[] { 1000, 5000 }, 1, null, null)]
        public void TestSplitNoteTime(double[] time, double percentage, double[] firstTime, double[] secondTime)
        {
            var note = new Note
            {
                StartTime = time[0],
                Duration = time[1],
            };

            try
            {
                var (firstNote, secondNote) = NotesUtils.SplitNote(note, percentage);
                Assert.AreEqual(firstTime[0], firstNote.StartTime);
                Assert.AreEqual(firstTime[1], firstNote.Duration);

                Assert.AreEqual(secondTime[0], secondNote.StartTime);
                Assert.AreEqual(secondTime[1], secondNote.Duration);
            }
            catch
            {
                Assert.IsNull(firstTime);
                Assert.IsNull(secondTime);
            }
        }

        [Test]
        public void TestSplitNoteOtherProperty()
        {
            const double percentage = 0.3;
            var lyric = new Lyric
            {
                Singers = new[] { 0 },
            };

            var note = new Note
            {
                StartTime = 1000,
                Duration = 2000,
                StartIndex = 1,
                EndIndex = 2,
                Text = "ka",
                Display = false,
                Tone = new Tone(-1, true),
                ParentLyric = lyric
            };

            // create other property and make sure other class is applied value.
            var (firstNote, secondNote) = NotesUtils.SplitNote(note, percentage);

            Assert.AreEqual(1000, firstNote.StartTime);
            Assert.AreEqual(1600, secondNote.StartTime);

            Assert.AreEqual(600, firstNote.Duration);
            Assert.AreEqual(1400, secondNote.Duration);

            testRemainProperty(note, firstNote);
            testRemainProperty(note, firstNote);

            static void testRemainProperty(Note expect, Note actual)
            {
                Assert.AreEqual(expect.StartIndex, actual.StartIndex);
                Assert.AreEqual(expect.EndIndex, actual.EndIndex);
                Assert.AreEqual(expect.Text, actual.Text);

                Assert.AreEqual(expect.Display, actual.Display);
                Assert.AreEqual(expect.Tone, actual.Tone);
                Assert.AreEqual(expect.ParentLyric, actual.ParentLyric);

                Assert.AreEqual(expect.ParentLyric.Singers, actual.ParentLyric.Singers);
            }
        }

        [TestCase(new double[] { 1000, 1000 }, new double[] { 2000, 4000 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 1000, 2500 }, new double[] { 3500, 2500 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 1000, 0 }, new double[] { 1000, 0 })] // it's ok to combine if duration is 0.
        public void TestCombineNoteTime(double[] firstTime, double[] secondTime, double[] expected)
        {
            const int start_index = 3;
            const int end_index = 5;

            var lyric = new Lyric();

            var firstNote = new Note
            {
                StartIndex = start_index,
                EndIndex = end_index,
                ParentLyric = lyric,
                StartTime = firstTime[0],
                Duration = firstTime[1],
            };

            var secondNote = new Note
            {
                StartIndex = start_index,
                EndIndex = end_index,
                ParentLyric = lyric,
                StartTime = secondTime[0],
                Duration = secondTime[1],
            };

            var combineNote = NotesUtils.CombineNote(firstNote, secondNote);
            Assert.AreEqual(expected[0], combineNote.StartTime);
            Assert.AreEqual(expected[1], combineNote.Duration);
        }
    }
}
