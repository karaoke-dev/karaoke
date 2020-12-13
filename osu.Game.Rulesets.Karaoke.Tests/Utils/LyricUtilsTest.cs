// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class LyricUtilsTest
    {
        #region combine

        [TestCase("Kara", "oke", "Karaoke")]
        [TestCase("", "oke", "oke")]
        [TestCase(null, "oke", "oke")]
        [TestCase("Kara", "", "Kara")]
        [TestCase("Kara", null, "Kara")]
        public void TestCombineLyricText(string text1, string text2, string expectText)
        {
            var lyric1 = new Lyric { Text = text1 };
            var lyric2 = new Lyric { Text = text2 };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.Text, expectText);
        }

        [TestCase(new[] { "[0,start]:" }, new[] { "[0,start]:" }, new[] { "[0,start]:", "[7,start]:" })]
        [TestCase(new[] { "[0,end]:" }, new[] { "[0,end]:" }, new[] { "[0,end]:", "[7,end]:" })]
        [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:1000" }, new[] { "[0,start]:1000", "[7,start]:1000" })] // deal with the case with time.
        [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:-1000" }, new[] { "[0,start]:1000", "[7,start]:-1000" })] // deal with the case with not invalid time tag time.
        [TestCase(new[] { "[-1,start]:" }, new[] { "[-1,start]:" }, new[] { "[-1,start]:", "[6,start]:" })] // deal with the case with not invalid time tag position.
        public void TestCombineLyricTimeTag(string[] timeTags1, string[] timeTags2, string[] expectTimeTags)
        {
            var lyric1 = new Lyric
            {
                Text = "karaoke",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags1)
            };
            var lyric2 = new Lyric
            {
                Text = "karaoke",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags2)
            };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            var timeTags = combineLyric.TimeTags;
            for (int i = 0; i < timeTags.Length; i++)
            {
                var actualTimeTag = TestCaseTagHelper.ParseTimeTag(expectTimeTags[i]);
                Assert.AreEqual(timeTags[i].Index, actualTimeTag.Index);
                Assert.AreEqual(timeTags[i].Time, actualTimeTag.Time);
            }
        }

        [TestCase(new[] { "[0,0]:ruby" }, new[] { "[0,0]:ルビ" }, new[] { "[0,0]:ruby", "[7,7]:ルビ" })]
        [TestCase(new[] { "[0,0]:" }, new[] { "[0,0]:" }, new[] { "[0,0]:", "[7,7]:" })]
        [TestCase(new[] { "[0,3]:" }, new[] { "[0,3]:" }, new[] { "[0,3]:", "[7,10]:" })]
        [TestCase(new[] { "[0,10]:" }, new[] { "[0,10]:" }, new[] { "[0,10]:", "[7,17]:" })] // deal with the case out of range.
        [TestCase(new[] { "[-10,0]:" }, new[] { "[-10,0]:" }, new[] { "[-10,0]:", "[-3,7]:" })] // deal with the case out of range.
        public void TestCombineLyricRubyTag(string[] rubyTags1, string[] rubyTags2, string[] expectRubyTags)
        {
            var lyric1 = new Lyric
            {
                Text = "karaoke",
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubyTags1)
            };
            var lyric2 = new Lyric
            {
                Text = "karaoke",
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubyTags2)
            };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            var rubyTags = combineLyric.RubyTags;
            var actualRubyTags = TestCaseTagHelper.ParseRubyTags(expectRubyTags);
            Assert.AreEqual(rubyTags, actualRubyTags);
        }

        [TestCase(new[] { "[0,0]:romaji" }, new[] { "[0,0]:ローマ字" }, new[] { "[0,0]:romaji", "[7,7]:ローマ字" })]
        [TestCase(new[] { "[0,0]:" }, new[] { "[0,0]:" }, new[] { "[0,0]:", "[7,7]:" })]
        [TestCase(new[] { "[0,3]:" }, new[] { "[0,3]:" }, new[] { "[0,3]:", "[7,10]:" })]
        [TestCase(new[] { "[0,10]:" }, new[] { "[0,10]:" }, new[] { "[0,10]:", "[7,17]:" })] // deal with the case out of range.
        [TestCase(new[] { "[-10,0]:" }, new[] { "[-10,0]:" }, new[] { "[-10,0]:", "[-3,7]:" })] // deal with the case out of range.
        public void TestCombineLyricRomajiTag(string[] romajiTags1, string[] romajiTags2, string[] expectRomajiTags)
        {
            var lyric1 = new Lyric
            {
                Text = "karaoke",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajiTags1)
            };
            var lyric2 = new Lyric
            {
                Text = "karaoke",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajiTags2)
            };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            var romajiTags = combineLyric.RomajiTags;
            var actualRomajiTags = TestCaseTagHelper.ParseRomajiTags(expectRomajiTags);
            Assert.AreEqual(romajiTags, actualRomajiTags);
        }

        [TestCase(new double[] { 1000, 0 }, new double[] { 1000, 0 }, new double[] { 1000, 0 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 2000, 0 }, new double[] { 1000, 1000 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 2000, 2000 }, new double[] { 1000, 3000 })]
        [TestCase(new double[] { 1000, 5000 }, new double[] { 1000, 0 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 1000, 5000 }, new double[] { 2000, 0 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 2000, 5000 }, new double[] { 1000, 0 }, new double[] { 1000, 6000 })]
        [TestCase(new double[] { 2000, 5000 }, new double[] { 1000, 10000 }, new double[] { 1000, 10000 })]
        public void TestCombineLyricTime(double[] lyric1Time, double[] lyric2Time, double[] actuallyTime)
        {
            var lyric1 = new Lyric
            {
                StartTime = lyric1Time[0],
                Duration = lyric1Time[1],
            };
            var lyric2 = new Lyric
            {
                StartTime = lyric2Time[0],
                Duration = lyric2Time[1],
            };

            // use min time as start time, and use max end time - min star time as duration
            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.StartTime, actuallyTime[0]);
            Assert.AreEqual(combineLyric.Duration, actuallyTime[1]);
        }

        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 1, 2 })]
        [TestCase(new[] { 1 }, new[] { 1 }, new[] { 1 })] // deal with dulicated case.
        [TestCase(new[] { 1 }, new[] { -2 }, new[] { 1, -2 })] // deal with id not right case.
        [TestCase(null, new[] { 2 }, new[] { 2 })] // deal with null case.
        [TestCase(new[] { 1 }, null, new[] { 1 })] // deal with null case.
        [TestCase(null, null, new int[] { })] // deal with null case.
        public void TestCombineLyricSinger(int[] singerIndexes1, int[] singerIndexes2, int[] actualSingerIndexes)
        {
            var lyric1 = new Lyric { Singers = singerIndexes1 };
            var lyric2 = new Lyric { Singers = singerIndexes2 };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.Singers, actualSingerIndexes);
        }

        [TestCase(1, 2, 1)]
        [TestCase(1, 3, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(-1, 1, -1)]
        [TestCase(-5, 1, -5)] // Wrong layout index
        public void TestCombineLayoutIndex(int layout1, int layout2, int actualLayout)
        {
            var lyric1 = new Lyric { LayoutIndex = layout1 };
            var lyric2 = new Lyric { LayoutIndex = layout2 };

            // just use first lyric's layout id.
            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.LayoutIndex, actualLayout);
        }

        [TestCase(1, 1, 1)]
        [TestCase(54, 54, 54)]
        [TestCase(null, 1, null)]
        [TestCase(1, null, null)]
        [TestCase(null, null, null)]
        public void TestLanguage(int? lcid1, int? lcid2, int? actuallyLcid)
        {
            var caltureInfo1 = lcid1 != null ? new CultureInfo(lcid1.Value) : null;
            var caltureInfo2 = lcid2 != null ? new CultureInfo(lcid2.Value) : null;
            var actualCaltureInfo = actuallyLcid != null ? new CultureInfo(actuallyLcid.Value) : null;

            var lyric1 = new Lyric { Language = caltureInfo1 };
            var lyric2 = new Lyric { Language = caltureInfo2 };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.Language, actualCaltureInfo);
        }

        #endregion
    }
}
