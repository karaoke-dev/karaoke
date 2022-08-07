// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class NoteUtilsTest
    {
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
