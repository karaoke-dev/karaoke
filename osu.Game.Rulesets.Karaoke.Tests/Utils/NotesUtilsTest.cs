// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
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
        public void TestSplitNoteTime(double[] time, double percentage, double[]? firstTime, double[]? secondTime)
        {
            var note = new Note
            {
                ReferenceLyric = TestCaseNoteHelper.CreateLyricForNote("Lyric", time[0], time[1]),
            };

            if (firstTime != null && secondTime != null)
            {
                var (firstNote, secondNote) = NotesUtils.SplitNote(note, percentage);
                Assert.AreEqual(firstTime[0], firstNote.StartTime);
                Assert.AreEqual(firstTime[1], firstNote.Duration);

                Assert.AreEqual(secondTime[0], secondNote.StartTime);
                Assert.AreEqual(secondTime[1], secondNote.Duration);
            }
            else
            {
                Assert.IsNull(firstTime);
                Assert.IsNull(secondTime);
            }
        }

        [Test]
        public void TestSplitNoteOtherProperty()
        {
            const double percentage = 0.3;

            var lyric = TestCaseNoteHelper.CreateLyricForNote("Lyric", 1000, 2000);
            lyric.Singers = new[] { 0 };

            var note = new Note
            {
                Text = "ka",
                Display = false,
                Tone = new Tone(-1, true),
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = 0
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
                Assert.AreEqual(expect.Text, actual.Text);
                Assert.AreEqual(expect.Display, actual.Display);
                Assert.AreEqual(expect.Tone, actual.Tone);

                Assert.AreEqual(expect.ReferenceLyric, actual.ReferenceLyric);
                Assert.AreEqual(expect.ReferenceTimeTagIndex, actual.ReferenceTimeTagIndex);

                Assert.AreEqual(expect.ReferenceLyric?.Singers, actual.ReferenceLyric?.Singers);
            }
        }

        [TestCase(new double[] { 1000, -1000 }, new double[] { 2000, -4000 }, new double[] { 1000, -1000 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 1000, 0 }, new double[] { 1000, 0 })] // it's ok to combine if duration is 0.
        public void TestCombineNoteTime(double[] firstOffset, double[] secondOffset, double[] expectedOffset)
        {
            var lyric = TestCaseNoteHelper.CreateLyricForNote("Lyric", 1000, 5000);

            var firstNote = new Note
            {
                ReferenceLyric = lyric,
                StartTimeOffset = firstOffset[0],
                EndTimeOffset = firstOffset[1],
                ReferenceTimeTagIndex = 0,
            };

            var secondNote = new Note
            {
                ReferenceLyric = lyric,
                StartTimeOffset = secondOffset[0],
                EndTimeOffset = secondOffset[1],
                ReferenceTimeTagIndex = 0,
            };

            var combineNote = NotesUtils.CombineNote(firstNote, secondNote);
            Assert.AreEqual(expectedOffset[0], combineNote.StartTimeOffset);
            Assert.AreEqual(expectedOffset[1], combineNote.EndTimeOffset);
        }
    }
}
