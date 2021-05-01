// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckInvalidPropertyNotes;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Checks
{
    [TestFixture]
    public class CheckInvalidPropertyNotesTest
    {
        private CheckInvalidPropertyNotes check;

        [SetUp]
        public void Setup()
        {
            check = new CheckInvalidPropertyNotes();
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(null, true)]
        public void TestCheckParentLyric(int? lyricIndex, bool hasIssue)
        {
            var lyric = new Lyric();
            var notInBeatmapLyric = new Lyric();

            var note = new Note();

            switch (lyricIndex)
            {
                case 0:
                    note.ParentLyric = lyric;
                    break;

                case 1:
                    note.ParentLyric = notInBeatmapLyric;
                    break;
            }

            var issueTemplate = run(lyric, note).Select(x => x.Template).OfType<IssueTemplateInvalidParentLyric>().FirstOrDefault();
            Assert.AreEqual(issueTemplate != null, hasIssue);
        }

        private IEnumerable<Issue> run(HitObject lyric, HitObject note)
        {
            var beatmap = new Beatmap
            {
                HitObjects = new List<HitObject>
                {
                    lyric,
                    note
                }
            };
            return check.Run(beatmap, new TestWorkingBeatmap(beatmap));
        }
    }
}
