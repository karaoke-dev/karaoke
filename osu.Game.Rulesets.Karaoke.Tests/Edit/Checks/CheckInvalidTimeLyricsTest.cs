// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Configs;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Objects;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Checks
{
    [TestFixture]
    public class CheckInvalidTimeLyricsTest
    {
        private CheckInvalidTimeLyrics check;

        [SetUp]
        public void Setup()
        {
            var config = new LyricCheckerConfig().CreateDefaultConfig();
            check = new CheckInvalidTimeLyrics(config);
        }

        [TestCase("[1000,3000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" }, new TimeInvalid[] { })]
        [TestCase("[3000,1000]:カラオケ", new string[] { }, new[] { TimeInvalid.Overlapping })]
        [TestCase("[2000,3000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" }, new[] { TimeInvalid.StartTimeInvalid })]
        [TestCase("[1000,2000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" }, new[] { TimeInvalid.EndTimeInvalid })]
        public void TestCheckInvalidLyricTime(string lyricText, string[] timeTags, TimeInvalid[] invalid)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

            var issue = run(lyric).OfType<LyricTimeIssue>().FirstOrDefault();
            var invalidTimeTagDictionaryKeys = issue?.InvalidLyricTime ?? Array.Empty<TimeInvalid>();
            Assert.AreEqual(invalidTimeTagDictionaryKeys, invalid);
        }

        [TestCase("カラオケ", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new TimeTagInvalid[] { })]
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:5000" }, new TimeTagInvalid[] { })]
        [TestCase("カラオケ", new[] { "[-1,start]:1000" }, new[] { TimeTagInvalid.OutOfRange })]
        [TestCase("カラオケ", new[] { "[4,start]:4000" }, new[] { TimeTagInvalid.OutOfRange })]
        [TestCase("カラオケ", new[] { "[0,start]:5000", "[3,end]:1000" }, new[] { TimeTagInvalid.Overlapping })]
        public void TestCheckInvalidTimeTags(string text, string[] timeTags, TimeTagInvalid[] invalids)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            var issue = run(lyric).OfType<TimeTagIssue>().FirstOrDefault();
            var invalidTimeTagDictionaryKeys = issue?.InvalidTimeTags.Keys.ToArray() ?? Array.Empty<TimeTagInvalid>();
            Assert.AreEqual(invalidTimeTagDictionaryKeys, invalids);
        }

        private IEnumerable<Issue> run(HitObject lyric)
        {
            var beatmap = new Beatmap
            {
                HitObjects = new List<HitObject>
                {
                    lyric
                }
            };
            var context = new BeatmapVerifierContext(beatmap, new TestWorkingBeatmap(beatmap));
            return check.Run(context);
        }
    }
}
