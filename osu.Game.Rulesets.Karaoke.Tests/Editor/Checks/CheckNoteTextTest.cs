// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckNoteText;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class CheckNoteTextTest : HitObjectCheckTest<Note, CheckNoteText>
    {
        [Test]
        public void TestCheck()
        {
            var note = new Note
            {
                Text = "karaoke",
                RubyText = null, // ruby text should be null or having the value.
            };

            AssertOk(new HitObject[] { note });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")] // but should not be empty or white space.
        [TestCase("　")] // but should not be empty or white space.
        public void TestCheckNoText(string text)
        {
            var note = new Note
            {
                Text = text
            };

            AssertNotOk<IssueTemplateNoteNoText>(note);
        }

        [TestCase("")]
        [TestCase(" ")] // but should not be empty or white space.
        [TestCase("　")] // but should not be empty or white space.
        public void TestCheckNoRubyText(string? rubyText)
        {
            var note = new Note
            {
                Text = "karaoke",
                RubyText = rubyText
            };

            AssertNotOk<IssueTemplateNoteNoRubyText>(note);
        }
    }
}
