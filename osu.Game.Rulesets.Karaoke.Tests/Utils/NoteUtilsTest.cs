// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        [TestCase("karaoke", "", false, "karaoke")]
        [TestCase("karaoke", "ka- ra- o- ke-", false, "karaoke")]
        [TestCase("", "ka- ra- o- ke-", false, "")]
        [TestCase(null, "ka- ra- o- ke-", false, null)]
        [TestCase("karaoke", "", true, "karaoke")]
        [TestCase("karaoke", null, true, "karaoke")]
        [TestCase("karaoke", "ka- ra- o- ke-", true, "ka- ra- o- ke-")]
        [TestCase("", "ka- ra- o- ke-", true, "ka- ra- o- ke-")]
        [TestCase(null, "ka- ra- o- ke-", true, "ka- ra- o- ke-")]
        public void TestDisplayText(string text, string alternativeText, bool useAlternativeTextIfHave, string actual)
        {
            var note = new Note
            {
                Text = text,
                AlternativeText = alternativeText
            };

            var result = NoteUtils.DisplayText(note, useAlternativeTextIfHave);
            Assert.AreEqual(result, actual);
        }
    }
}
