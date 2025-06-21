// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Utils;

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
        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "Lyric", time[0], time[1]);
        var note = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
        };

        if (firstTime != null && secondTime != null)
        {
            var (firstNote, secondNote) = NotesUtils.SplitNote(note, percentage);
            Assert.That(firstNote.StartTime, Is.EqualTo(firstTime[0]));
            Assert.That(firstNote.Duration, Is.EqualTo(firstTime[1]));

            Assert.That(secondNote.StartTime, Is.EqualTo(secondTime[0]));
            Assert.That(secondNote.Duration, Is.EqualTo(secondTime[1]));
        }
        else
        {
            Assert.Catch(() => NotesUtils.SplitNote(note, percentage));
        }
    }

    [Test]
    public void TestSplitNoteOtherProperty()
    {
        const double percentage = 0.3;

        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "Lyric", 1000, 2000);
        referencedLyric.SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 0 });

        var note = new Note
        {
            Text = "ka",
            Display = false,
            Tone = new Tone(-1, true),
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = 0,
        };

        // create other property and make sure other class is applied value.
        var (firstNote, secondNote) = NotesUtils.SplitNote(note, percentage);

        Assert.That(firstNote.StartTime, Is.EqualTo(1000));
        Assert.That(secondNote.StartTime, Is.EqualTo(1600));

        Assert.That(firstNote.Duration, Is.EqualTo(600));
        Assert.That(secondNote.Duration, Is.EqualTo(1400));

        testRemainProperty(note, firstNote);
        testRemainProperty(note, firstNote);

        static void testRemainProperty(Note expect, Note actual)
        {
            Assert.That(actual.Text, Is.EqualTo(expect.Text));
            Assert.That(actual.Display, Is.EqualTo(expect.Display));
            Assert.That(actual.Tone, Is.EqualTo(expect.Tone));

            Assert.That(actual.ReferenceLyric, Is.EqualTo(expect.ReferenceLyric));
            Assert.That(actual.ReferenceTimeTagIndex, Is.EqualTo(expect.ReferenceTimeTagIndex));

            Assert.That(actual.ReferenceLyric?.SingerIds, Is.EqualTo(expect.ReferenceLyric?.SingerIds));
        }
    }

    [TestCase(new double[] { 1000, -1000 }, new double[] { 2000, -4000 }, new double[] { 1000, -1000 })]
    [TestCase(new double[] { 1000, 0 }, new double[] { 1000, 0 }, new double[] { 1000, 0 })] // it's ok to combine if duration is 0.
    public void TestCombineNoteTime(double[] firstOffset, double[] secondOffset, double[] expectedOffset)
    {
        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "Lyric", 1000, 5000);

        var firstNote = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            StartTimeOffset = firstOffset[0],
            EndTimeOffset = firstOffset[1],
            ReferenceTimeTagIndex = 0,
        };

        var secondNote = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            StartTimeOffset = secondOffset[0],
            EndTimeOffset = secondOffset[1],
            ReferenceTimeTagIndex = 0,
        };

        var combineNote = NotesUtils.CombineNote(firstNote, secondNote);
        Assert.That(combineNote.StartTimeOffset, Is.EqualTo(expectedOffset[0]));
        Assert.That(combineNote.EndTimeOffset, Is.EqualTo(expectedOffset[1]));
    }
}
