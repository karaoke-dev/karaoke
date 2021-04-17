// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Checks
{
    /// <summary>
    /// Test all the lyric check result and invalid type
    /// </summary>
    [TestFixture]
    public class CheckInvalidLyricsTest
    {
        private CheckInvalidLyrics check;

        [SetUp]
        public void Setup()
        {
            var config = new LyricCheckerConfig().CreateDefaultConfig();
            check = new CheckInvalidLyrics(config);
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
            var invalidTimeTagDictionaryKeys = issue?.InvalidLyricTime ?? new TimeInvalid[] { };
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
            var invalidTimeTagDictionaryKeys = issue?.InvalidTimeTags.Keys.ToArray() ?? new TimeTagInvalid[] { };
            Assert.AreEqual(invalidTimeTagDictionaryKeys, invalids);
        }

        [TestCase("カラオケ", new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, new RubyTagInvalid[] { })]
        [TestCase("カラオケ", new[] { "[0,4]:からおけ" }, new RubyTagInvalid[] { })]
        [TestCase("カラオケ", new[] { "[-1,1]:か" }, new[] { RubyTagInvalid.OutOfRange })]
        [TestCase("カラオケ", new[] { "[4,5]:け" }, new[] { RubyTagInvalid.OutOfRange })]
        [TestCase("カラオケ", new[] { "[0,1]:か", "[0,1]:ら" }, new[] { RubyTagInvalid.Overlapping })]
        [TestCase("カラオケ", new[] { "[0,3]:か", "[1,2]:ら" }, new[] { RubyTagInvalid.Overlapping })]
        public void TestCheckInvalidRubyTags(string text, string[] rubies, RubyTagInvalid[] invalids)
        {
            var lyric = new Lyric
            {
                Text = text,
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies)
            };

            var issue = run(lyric).OfType<RubyTagIssue>().FirstOrDefault();
            var invalidRubyTagDictionaryKeys = issue?.InvalidRubyTags.Keys.ToArray() ?? new RubyTagInvalid[] { };
            Assert.AreEqual(invalidRubyTagDictionaryKeys, invalids);
        }

        [TestCase("karaoke", new[] { "[0,2]:ka", "[2,4]:ra", "[4,5]:o", "[5,7]:ke" }, new RomajiTagInvalid[] { })]
        [TestCase("karaoke", new[] { "[0,7]:karaoke" }, new RomajiTagInvalid[] { })]
        [TestCase("karaoke", new[] { "[-1,2]:ka" }, new[] { RomajiTagInvalid.OutOfRange })]
        [TestCase("karaoke", new[] { "[7,8]:ke" }, new[] { RomajiTagInvalid.OutOfRange })]
        [TestCase("karaoke", new[] { "[0,2]:ka", "[1,3]:ra" }, new[] { RomajiTagInvalid.Overlapping })]
        [TestCase("karaoke", new[] { "[0,3]:ka", "[1,2]:ra" }, new[] { RomajiTagInvalid.Overlapping })]
        public void TestCheckInvalidRomajiTags(string text, string[] romajies, RomajiTagInvalid[] invalids)
        {
            var lyric = new Lyric
            {
                Text = text,
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
            };

            var issue = run(lyric).OfType<RomajiTagIssue>().FirstOrDefault();
            var invalidRomajiTagDictionaryKeys = issue?.InvalidRomajiTags.Keys.ToArray() ?? new RomajiTagInvalid[] { };
            Assert.AreEqual(invalidRomajiTagDictionaryKeys, invalids);
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
            return check.Run(beatmap);
        }
    }
}
