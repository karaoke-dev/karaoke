// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class LyricUtilsTest
    {
        #region separate

        [TestCase("karaoke", 4, "kara", "oke")]
        [TestCase("カラオケ", 2, "カラ", "オケ")]
        [TestCase("", 0, null, null)] // Test error
        [TestCase(null, 0, null, null)]
        [TestCase("karaoke", 100, null, null)]
        [TestCase("", 100, null, null)]
        [TestCase(null, 100, null, null)]
        public void TestSeparateLyricText(string text, int splitIndex, string firstText, string secondText)
        {
            var lyric = new Lyric { Text = text };

            try
            {
                var (firstLyric, secondLyric) = LyricUtils.SplitLyric(lyric, splitIndex);
                Assert.AreEqual(firstLyric.Text, firstText);
                Assert.AreEqual(secondLyric.Text, secondText);
            }
            catch
            {
                Assert.IsNull(firstText);
                Assert.IsNull(secondText);
            }
        }

        [TestCase("カラオケ", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, 2,
            new[] { "[0,start]:1000", "[1,start]:2000", "[1,end]:3000" },
            new[] { "[0,start]:3000", "[1,start]:4000", "[1,end]:5000" })]
        public void TestSeparateLyricTimeTag(string text, string[] timeTags, int splitIndex, string[] firstTimeTags, string[] secondTimeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            var (firstLyric, secondLyric) = LyricUtils.SplitLyric(lyric, splitIndex);

            testTimeTags(firstLyric.TimeTags, TestCaseTagHelper.ParseTimeTags(firstTimeTags));
            testTimeTags(secondLyric.TimeTags, TestCaseTagHelper.ParseTimeTags(secondTimeTags));

            static void testTimeTags(IReadOnlyList<TimeTag> expect, IReadOnlyList<TimeTag> actually)
            {
                Assert.AreEqual(expect?.Count, actually?.Count);
                if (expect == null || actually == null)
                    return;

                for (int i = 0; i < expect.Count; i++)
                {
                    Assert.AreEqual(expect[i].Index, actually[i].Index);
                    Assert.AreEqual(expect[i].Time, actually[i].Time);
                }
            }
        }

        [TestCase("カラオケ", new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 2,
            new[] { "[0,1]:か", "[1,2]:ら" }, new[] { "[0,1]:お", "[1,2]:け" })]
        [TestCase("カラオケ", new[] { "[0,3]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[1,4]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[2,2]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[0,4]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new string[] { }, 2, new string[] { }, new string[] { })]
        [TestCase("カラオケ", null, 2, null, null)]
        public void TestSeparateLyricRubyTag(string text, string[] rubyTags, int splitIndex, string[] firstRubyTags, string[] secondRubyTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubyTags)
            };

            var (firstLyric, secondLyric) = LyricUtils.SplitLyric(lyric, splitIndex);

            Assert.AreEqual(firstLyric.RubyTags, TestCaseTagHelper.ParseRubyTags(firstRubyTags));
            Assert.AreEqual(secondLyric.RubyTags, TestCaseTagHelper.ParseRubyTags(secondRubyTags));
        }

        [TestCase("カラオケ", new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, 2,
            new[] { "[0,1]:ka", "[1,2]:ra" }, new[] { "[0,1]:o", "[1,2]:ke" })]
        [TestCase("カラオケ", new[] { "[0,3]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[1,4]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[2,2]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[0,4]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new string[] { }, 2, new string[] { }, new string[] { })]
        [TestCase("カラオケ", null, 2, null, null)]
        public void TestSeparateLyricRomajiTag(string text, string[] romajiTags, int splitIndex, string[] firstRomajiTags, string[] secondRomajiTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajiTags)
            };

            var (firstLyric, secondLyric) = LyricUtils.SplitLyric(lyric, splitIndex);

            Assert.AreEqual(firstLyric.RomajiTags, TestCaseTagHelper.ParseRomajiTags(firstRomajiTags));
            Assert.AreEqual(secondLyric.RomajiTags, TestCaseTagHelper.ParseRomajiTags(secondRomajiTags));
        }

        [Ignore("Not really sure second lyric is based on lyric time or time-tag time.")]
        public void TestSeparateLyricStartTime()
        {
            // todo : implement
        }

        [Ignore("Not really sure second lyric is based on lyric time or time-tag time.")]
        public void TestSeparateLyricDuration()
        {
            // todo : implement
        }

        [TestCase(new[] { 1, 2 }, new[] { 1, 2 }, new[] { 1, 2 })]
        [TestCase(new[] { 1 }, new[] { 1 }, new[] { 1 })]
        [TestCase(new[] { -1 }, new[] { -1 }, new[] { -1 })] // copy singer index even it's invalid.
        [TestCase(new int[] { }, new int[] { }, new int[] { })]
        [TestCase(null, null, null)]
        public void TestSeparateLyricSinger(int[] singerIndexes, int[] firstSingerIndexes, int[] secondSingerIndexes)
        {
            const int split_index = 2;
            var lyric = new Lyric
            {
                Text = "karaoke!",
                Singers = singerIndexes
            };

            var (firstLyric, secondLyric) = LyricUtils.SplitLyric(lyric, split_index);

            Assert.AreEqual(firstLyric.Singers, firstSingerIndexes);
            Assert.AreEqual(secondLyric.Singers, secondSingerIndexes);

            if (lyric.Singers == null)
                return;

            // also should check is not same object as origin lyric for safety purpose.
            Assert.AreNotSame(firstLyric.Singers, lyric.Singers);
            Assert.AreNotSame(secondLyric.Singers, lyric.Singers);
        }

        [TestCase(1, 1, 1)]
        [TestCase(2, 2, 2)]
        [TestCase(-5, -5, -5)] // copy layout index even it's wrong.
        public void TestSeparateLyricLayoutIndex(int actualLayout, int firstLayout, int secondLayout)
        {
            const int split_index = 2;
            var lyric = new Lyric
            {
                Text = "karaoke!",
                LayoutIndex = actualLayout
            };

            var (firstLyric, secondLyric) = LyricUtils.SplitLyric(lyric, split_index);

            Assert.AreEqual(firstLyric.LayoutIndex, firstLayout);
            Assert.AreEqual(secondLyric.LayoutIndex, secondLayout);
        }

        [TestCase(1, 1, 1)]
        [TestCase(54, 54, 54)]
        [TestCase(null, null, null)]
        public void TestSeparateLyricLanguage(int? lcid, int? firstLcid, int? secondLcid)
        {
            var cultureInfo = lcid != null ? new CultureInfo(lcid.Value) : null;
            var firstCultureInfo = firstLcid != null ? new CultureInfo(firstLcid.Value) : null;
            var secondCultureInfo = secondLcid != null ? new CultureInfo(secondLcid.Value) : null;

            const int split_index = 2;
            var lyric = new Lyric
            {
                Text = "karaoke!",
                Language = cultureInfo
            };

            var (firstLyric, secondLyric) = LyricUtils.SplitLyric(lyric, split_index);

            Assert.AreEqual(firstLyric.Language, firstCultureInfo);
            Assert.AreEqual(secondLyric.Language, secondCultureInfo);
        }

        #endregion

        #region combine

        [TestCase("Kara", "oke", "Karaoke")]
        [TestCase("", "oke", "oke")]
        [TestCase(null, "oke", "oke")]
        [TestCase("Kara", "", "Kara")]
        [TestCase("Kara", null, "Kara")]
        public void TestCombineLyricText(string firstText, string secondText, string actualText)
        {
            var lyric1 = new Lyric { Text = firstText };
            var lyric2 = new Lyric { Text = secondText };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.Text, actualText);
        }

        [TestCase(new[] { "[0,start]:" }, new[] { "[0,start]:" }, new[] { "[0,start]:", "[7,start]:" })]
        [TestCase(new[] { "[0,end]:" }, new[] { "[0,end]:" }, new[] { "[0,end]:", "[7,end]:" })]
        [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:1000" }, new[] { "[0,start]:1000", "[7,start]:1000" })] // deal with the case with time.
        [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:-1000" }, new[] { "[0,start]:1000", "[7,start]:-1000" })] // deal with the case with not invalid time tag time.
        [TestCase(new[] { "[-1,start]:" }, new[] { "[-1,start]:" }, new[] { "[-1,start]:", "[6,start]:" })] // deal with the case with not invalid time tag position.
        public void TestCombineLyricTimeTag(string[] firstTimeTags, string[] secondTimeTags, string[] actualTimeTags)
        {
            var lyric1 = new Lyric
            {
                Text = "karaoke",
                TimeTags = TestCaseTagHelper.ParseTimeTags(firstTimeTags)
            };
            var lyric2 = new Lyric
            {
                Text = "karaoke",
                TimeTags = TestCaseTagHelper.ParseTimeTags(secondTimeTags)
            };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            var timeTags = combineLyric.TimeTags;

            for (int i = 0; i < timeTags.Length; i++)
            {
                var actualTimeTag = TestCaseTagHelper.ParseTimeTag(actualTimeTags[i]);
                Assert.AreEqual(timeTags[i].Index, actualTimeTag.Index);
                Assert.AreEqual(timeTags[i].Time, actualTimeTag.Time);
            }
        }

        [TestCase(new[] { "[0,0]:ruby" }, new[] { "[0,0]:ルビ" }, new[] { "[0,0]:ruby", "[7,7]:ルビ" })]
        [TestCase(new[] { "[0,0]:" }, new[] { "[0,0]:" }, new[] { "[0,0]:", "[7,7]:" })]
        [TestCase(new[] { "[0,3]:" }, new[] { "[0,3]:" }, new[] { "[0,3]:", "[7,10]:" })]
        [TestCase(new[] { "[0,10]:" }, new[] { "[0,10]:" }, new[] { "[0,10]:", "[7,17]:" })] // deal with the case out of range.
        [TestCase(new[] { "[-10,0]:" }, new[] { "[-10,0]:" }, new[] { "[-10,0]:", "[-3,7]:" })] // deal with the case out of range.
        public void TestCombineLyricRubyTag(string[] firstRubyTags, string[] secondRubyTags, string[] actualRubyTags)
        {
            var lyric1 = new Lyric
            {
                Text = "karaoke",
                RubyTags = TestCaseTagHelper.ParseRubyTags(firstRubyTags)
            };
            var lyric2 = new Lyric
            {
                Text = "karaoke",
                RubyTags = TestCaseTagHelper.ParseRubyTags(secondRubyTags)
            };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            var rubyTags = combineLyric.RubyTags;
            Assert.AreEqual(rubyTags, TestCaseTagHelper.ParseRubyTags(actualRubyTags));
        }

        [TestCase(new[] { "[0,0]:romaji" }, new[] { "[0,0]:ローマ字" }, new[] { "[0,0]:romaji", "[7,7]:ローマ字" })]
        [TestCase(new[] { "[0,0]:" }, new[] { "[0,0]:" }, new[] { "[0,0]:", "[7,7]:" })]
        [TestCase(new[] { "[0,3]:" }, new[] { "[0,3]:" }, new[] { "[0,3]:", "[7,10]:" })]
        [TestCase(new[] { "[0,10]:" }, new[] { "[0,10]:" }, new[] { "[0,10]:", "[7,17]:" })] // deal with the case out of range.
        [TestCase(new[] { "[-10,0]:" }, new[] { "[-10,0]:" }, new[] { "[-10,0]:", "[-3,7]:" })] // deal with the case out of range.
        public void TestCombineLyricRomajiTag(string[] firstRomajiTags, string[] secondRomajiTags, string[] actualRomajiTags)
        {
            var lyric1 = new Lyric
            {
                Text = "karaoke",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(firstRomajiTags)
            };
            var lyric2 = new Lyric
            {
                Text = "karaoke",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(secondRomajiTags)
            };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            var romajiTags = combineLyric.RomajiTags;
            Assert.AreEqual(romajiTags, TestCaseTagHelper.ParseRomajiTags(actualRomajiTags));
        }

        [TestCase(new double[] { 1000, 0 }, new double[] { 1000, 0 }, new double[] { 1000, 0 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 2000, 0 }, new double[] { 1000, 1000 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 2000, 2000 }, new double[] { 1000, 3000 })]
        [TestCase(new double[] { 1000, 5000 }, new double[] { 1000, 0 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 1000, 5000 }, new double[] { 2000, 0 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 2000, 5000 }, new double[] { 1000, 0 }, new double[] { 1000, 6000 })]
        [TestCase(new double[] { 2000, 5000 }, new double[] { 1000, 10000 }, new double[] { 1000, 10000 })]
        public void TestCombineLyricTime(double[] firstLyricTime, double[] secondLyricTime, double[] actuallyTime)
        {
            var lyric1 = new Lyric
            {
                StartTime = firstLyricTime[0],
                Duration = firstLyricTime[1],
            };
            var lyric2 = new Lyric
            {
                StartTime = secondLyricTime[0],
                Duration = secondLyricTime[1],
            };

            // use min time as start time, and use max end time - min star time as duration
            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.StartTime, actuallyTime[0]);
            Assert.AreEqual(combineLyric.Duration, actuallyTime[1]);
        }

        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 1, 2 })]
        [TestCase(new[] { 1 }, new[] { 1 }, new[] { 1 })] // deal with duplicated case.
        [TestCase(new[] { 1 }, new[] { -2 }, new[] { 1, -2 })] // deal with id not right case.
        [TestCase(null, new[] { 2 }, new[] { 2 })] // deal with null case.
        [TestCase(new[] { 1 }, null, new[] { 1 })] // deal with null case.
        [TestCase(null, null, new int[] { })] // deal with null case.
        public void TestCombineLyricSinger(int[] firstSingerIndexes, int[] secondSingerIndexes, int[] actualSingerIndexes)
        {
            var lyric1 = new Lyric { Singers = firstSingerIndexes };
            var lyric2 = new Lyric { Singers = secondSingerIndexes };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.Singers, actualSingerIndexes);
        }

        [TestCase(1, 2, 1)]
        [TestCase(1, 3, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(-1, 1, -1)]
        [TestCase(-5, 1, -5)] // Wrong layout index
        public void TestCombineLayoutIndex(int firstLayout, int secondLayout, int actualLayout)
        {
            var lyric1 = new Lyric { LayoutIndex = firstLayout };
            var lyric2 = new Lyric { LayoutIndex = secondLayout };

            // just use first lyric's layout id.
            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.LayoutIndex, actualLayout);
        }

        [TestCase(1, 1, 1)]
        [TestCase(54, 54, 54)]
        [TestCase(null, 1, null)]
        [TestCase(1, null, null)]
        [TestCase(null, null, null)]
        public void TestCombineLayoutLanguage(int? firstLcid, int? secondLcid, int? actualLcid)
        {
            var cultureInfo1 = firstLcid != null ? new CultureInfo(firstLcid.Value) : null;
            var cultureInfo2 = secondLcid != null ? new CultureInfo(secondLcid.Value) : null;
            var actualCultureInfo = actualLcid != null ? new CultureInfo(actualLcid.Value) : null;

            var lyric1 = new Lyric { Language = cultureInfo1 };
            var lyric2 = new Lyric { Language = cultureInfo2 };

            var combineLyric = LyricUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(combineLyric.Language, actualCultureInfo);
        }

        #endregion
    }
}
