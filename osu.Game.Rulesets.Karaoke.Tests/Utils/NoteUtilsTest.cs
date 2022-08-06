// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class NoteUtilsTest
    {
        [TestCase(0, 1, new double[] { 1000, 3000 })]
        [TestCase(0, 0.5, new double[] { 1000, 1500 })]
        [TestCase(0.5, 0.5, new double[] { 2500, 1500 })]
        [TestCase(0.3, 0.4, new double[] { 1900, 1200 })]
        [TestCase(0.3, 1, null)] // start + duration should not exceed 1
        public void TestSliceNoteTime(double startPercentage, double durationPercentage, double[]? expected)
        {
            var lyric = new Lyric
            {
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[1,start]:4000" }),
            };

            // start time will be 1000, and duration will be 3000.
            var note = new Note
            {
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = 0
            };

            if (expected != null)
            {
                var sliceNote = NoteUtils.SliceNote(note, startPercentage, durationPercentage);

                Assert.AreEqual(expected[0], sliceNote.StartTime);
                Assert.AreEqual(expected[1], sliceNote.Duration);
            }
            else
            {
                Assert.IsNull(expected);
            }
        }

        [TestCase("karaoke", "", false, "karaoke")]
        [TestCase("karaoke", "ka- ra- o- ke-", false, "karaoke")]
        [TestCase("", "ka- ra- o- ke-", false, "")]
        [TestCase("karaoke", "", true, "karaoke")]
        [TestCase("karaoke", "ka- ra- o- ke-", true, "ka- ra- o- ke-")]
        [TestCase("", "ka- ra- o- ke-", true, "ka- ra- o- ke-")]
        public void TestDisplayText(string text, string rubyText, bool useRubyTextIfHave, string expected)
        {
            var note = new Note
            {
                Text = text,
                RubyText = rubyText
            };

            string actual = NoteUtils.DisplayText(note, useRubyTextIfHave);
            Assert.AreEqual(expected, actual);
        }
    }
}
