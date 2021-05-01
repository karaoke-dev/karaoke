// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
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
    public class CheckInvalidRubyRomajiLyricsTest
    {
        private CheckInvalidRubyRomajiLyrics check;

        [SetUp]
        public void Setup()
        {
            var config = new LyricCheckerConfig().CreateDefaultConfig();
            check = new CheckInvalidRubyRomajiLyrics(config);
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
            var invalidRubyTagDictionaryKeys = issue?.InvalidRubyTags.Keys.ToArray() ?? Array.Empty<RubyTagInvalid>();
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
            var invalidRomajiTagDictionaryKeys = issue?.InvalidRomajiTags.Keys.ToArray() ?? Array.Empty<RomajiTagInvalid>();
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
            return check.Run(beatmap, new TestWorkingBeatmap(beatmap));
        }
    }
}
