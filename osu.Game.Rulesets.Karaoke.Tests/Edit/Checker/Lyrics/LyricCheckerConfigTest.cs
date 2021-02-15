// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Checker.Lyrics
{
    /// <summary>
    /// Test all the lyric check result and invalid type
    /// </summary>
    [TestFixture]
    public class LyricCheckerConfigTest
    {
        [TestCase("[1000,3000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" }, new TimeInvalid[] { })]
        [TestCase("[3000,1000]:カラオケ", new string[] { }, new [] { TimeInvalid.Overlapping })]
        [TestCase("[2000,3000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" }, new [] { TimeInvalid.StartTimeInvalid })]
        [TestCase("[1000,2000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" }, new[] { TimeInvalid.EndTimeInvalid })]
        public void TestCheckInvalidLyricTime(string lyricText, string[] timeTags, TimeInvalid[] invalid)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

            var checker = createChecker();
            var result = checker.CheckInvalidLyricTime(lyric);
            Assert.AreEqual(result, invalid);
        }

        [TestCase("カラオケ", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new TimeTagInvalid[] { })]
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:5000" }, new TimeTagInvalid[] { })]
        [TestCase("カラオケ", new[] { "[-1,start]:1000" }, new [] { TimeTagInvalid.OutOfRange })]
        [TestCase("カラオケ", new[] { "[4,start]:4000" }, new[] { TimeTagInvalid.OutOfRange })]
        [TestCase("カラオケ", new[] { "[0,start]:5000", "[3,end]:1000" }, new [] { TimeTagInvalid.Overlapping })]
        public void TestCheckInvalidTimeTags(string text, string[] timeTags, TimeTagInvalid[] invalids)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            var checker = createChecker();
            var result = checker.CheckInvalidTimeTags(lyric);
            Assert.AreEqual(result.Keys.ToArray(), invalids);
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

            var checker = createChecker();
            var result = checker.CheckInvalidRubyTags(lyric);
            Assert.AreEqual(result.Keys.ToArray(), invalids);
        }

        [TestCase("karaoke", new[] { "[0,2]:ka", "[2,4]:ra", "[4,5]:o", "[5,7]:ke" }, new RomajiTagInvalid[] { })]
        [TestCase("karaoke", new[] { "[0,7]:karaoke" }, new RomajiTagInvalid[] { })]
        [TestCase("karaoke", new[] { "[-1,2]:ka" }, new [] { RomajiTagInvalid.OutOfRange })]
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

            var checker = createChecker();
            var result = checker.CheckInvalidRomajiTags(lyric);
            Assert.AreEqual(result.Keys.ToArray(), invalids);
        }

        // create checker with default config.
        // config change is not test scope so use default it ok.
        private static LyricChecker createChecker()
            => new LyricChecker(new LyricCheckerConfig().CreateDefaultConfig()); 
    }
}
