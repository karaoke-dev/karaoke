// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckNoteReferenceLyric;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckNoteReferenceLyricTest : HitObjectCheckTest<Note, CheckNoteReferenceLyric>
    {
        [Test]
        public void TestCheck()
        {
            var lyric = new Lyric();
            var note = new Note
            {
                ReferenceLyric = lyric
            };

            AssertOk(new HitObject[] { lyric, note });
        }

        [Test]
        public void TestCheckNullReferenceLyric()
        {
            var note = new Note
            {
                ReferenceLyric = null // reference should not be null.
            };

            AssertNotOk<IssueTemplateNullReferenceLyric>(note);
        }

        [Test]
        public void TestCheckInvalidReferenceLyric()
        {
            var lyric = new Lyric();
            var note = new Note
            {
                ReferenceLyric = lyric // reference lyric should be in the beatmap.
            };

            AssertNotOk<IssueTemplateInvalidReferenceLyric>(note);
        }
    }
}
