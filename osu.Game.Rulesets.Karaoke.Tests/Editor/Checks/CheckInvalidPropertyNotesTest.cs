// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckInvalidPropertyNotes;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckInvalidPropertyNotesTest : HitObjectCheckTest<Note, CheckInvalidPropertyNotes>
    {
        [TestCase(0)]
        public void TestCheckReferenceLyric(int? lyricIndex)
        {
            var lyric = new Lyric();
            var notInBeatmapLyric = new Lyric();

            var note = new Note();

            note.ReferenceLyric = lyricIndex switch
            {
                0 => lyric,
                1 => notInBeatmapLyric,
                _ => note.ReferenceLyric
            };

            AssertOk(lyric);
        }

        [TestCase(1)] // should have error if id is not exist.
        [TestCase(null)]
        public void TestCheckInvalidReferenceLyric(int? lyricIndex)
        {
            var lyric = new Lyric();
            var notInBeatmapLyric = new Lyric();

            var note = new Note();

            note.ReferenceLyric = lyricIndex switch
            {
                0 => lyric,
                1 => notInBeatmapLyric,
                _ => note.ReferenceLyric
            };

            AssertNotOk<IssueTemplateInvalidReferenceLyric>(lyric);
        }
    }
}
