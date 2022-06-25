// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class LyricsUtilsTest
    {
        #region separate

        [TestCase("karaoke", 4, "kara", "oke")]
        [TestCase("カラオケ", 2, "カラ", "オケ")]
        [TestCase("", 0, null, null)] // Test error
        [TestCase("karaoke", 100, null, null)]
        [TestCase("", 100, null, null)]
        public void TestSeparateLyricText(string text, int splitIndex, string? expectedFirstText, string? expectedSecondText)
        {
            var lyric = new Lyric { Text = text };

            if (expectedFirstText != null && expectedSecondText != null)
            {
                var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, splitIndex);
                Assert.AreEqual(expectedFirstText, firstLyric.Text);
                Assert.AreEqual(expectedSecondText, secondLyric.Text);
            }
            else
            {
                Assert.IsNull(expectedFirstText);
                Assert.IsNull(expectedSecondText);
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

            var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, splitIndex);

            TimeTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseTimeTags(firstTimeTags), firstLyric.TimeTags);
            TimeTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseTimeTags(secondTimeTags), secondLyric.TimeTags);
        }

        [TestCase("カラオケ", new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 2,
            new[] { "[0,1]:か", "[1,2]:ら" }, new[] { "[0,1]:お", "[1,2]:け" })]
        [TestCase("カラオケ", new[] { "[0,3]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[1,4]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[2,2]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[0,4]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new string[] { }, 2, new string[] { }, new string[] { })]
        public void TestSeparateLyricRubyTag(string text, string[] rubyTags, int splitIndex, string[] firstRubyTags, string[] secondRubyTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubyTags)
            };

            var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, splitIndex);

            TextTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseRubyTags(firstRubyTags), firstLyric.RubyTags);
            TextTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseRubyTags(secondRubyTags), secondLyric.RubyTags);
        }

        [TestCase("カラオケ", new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, 2,
            new[] { "[0,1]:ka", "[1,2]:ra" }, new[] { "[0,1]:o", "[1,2]:ke" })]
        [TestCase("カラオケ", new[] { "[0,3]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[1,4]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[2,2]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new[] { "[0,4]:karaoke" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
        [TestCase("カラオケ", new string[] { }, 2, new string[] { }, new string[] { })]
        public void TestSeparateLyricRomajiTag(string text, string[] romajiTags, int splitIndex, string[] firstRomajiTags, string[] secondRomajiTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajiTags)
            };

            var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, splitIndex);

            TextTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseRomajiTags(firstRomajiTags), firstLyric.RomajiTags);
            TextTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseRomajiTags(secondRomajiTags), secondLyric.RomajiTags);
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
        public void TestSeparateLyricSinger(int[] singerIndexes, int[] expectedFirstSingerIndexes, int[] expectedSecondSingerIndexes)
        {
            const int split_index = 2;
            var lyric = new Lyric
            {
                Text = "karaoke!",
                Singers = singerIndexes
            };

            var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, split_index);

            Assert.AreEqual(expectedFirstSingerIndexes, firstLyric.Singers);
            Assert.AreEqual(expectedSecondSingerIndexes, secondLyric.Singers);

            // also should check is not same object as origin lyric for safety purpose.
            Assert.AreNotSame(lyric.Singers, firstLyric.Singers);
            Assert.AreNotSame(lyric.Singers, secondLyric.Singers);
        }

        [TestCase(1, 1, 1)]
        [TestCase(54, 54, 54)]
        [TestCase(null, null, null)]
        public void TestSeparateLyricLanguage(int? lcid, int? firstLcid, int? secondLcid)
        {
            var cultureInfo = lcid != null ? new CultureInfo(lcid.Value) : null;
            var expectedFirstCultureInfo = firstLcid != null ? new CultureInfo(firstLcid.Value) : null;
            var expectedSecondCultureInfo = secondLcid != null ? new CultureInfo(secondLcid.Value) : null;

            const int split_index = 2;
            var lyric = new Lyric
            {
                Text = "karaoke!",
                Language = cultureInfo
            };

            var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, split_index);

            Assert.AreEqual(expectedFirstCultureInfo, firstLyric.Language);
            Assert.AreEqual(expectedSecondCultureInfo, secondLyric.Language);
        }

        #endregion

        #region combine

        [TestCase("Kara", "oke", "Karaoke")]
        [TestCase("", "oke", "oke")]
        [TestCase("Kara", "", "Kara")]
        public void TestCombineLyricText(string firstText, string secondText, string expected)
        {
            var lyric1 = new Lyric { Text = firstText };
            var lyric2 = new Lyric { Text = secondText };

            var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(expected, combineLyric.Text);
        }

        [TestCase(new[] { "[0,start]:" }, new[] { "[0,start]:" }, new[] { "[0,start]:", "[7,start]:" })]
        [TestCase(new[] { "[0,end]:" }, new[] { "[0,end]:" }, new[] { "[0,end]:", "[7,end]:" })]
        [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:1000" }, new[] { "[0,start]:1000", "[7,start]:1000" })] // deal with the case with time.
        [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:-1000" }, new[] { "[0,start]:1000", "[7,start]:-1000" })] // deal with the case with not invalid time tag time.
        [TestCase(new[] { "[-1,start]:" }, new[] { "[-1,start]:" }, new[] { "[-1,start]:", "[6,start]:" })] // deal with the case with not invalid time tag position.
        public void TestCombineLyricTimeTag(string[] firstTimeTags, string[] secondTimeTags, string[] expectTimeTags)
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

            var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);
            var timeTags = combineLyric.TimeTags;

            for (int i = 0; i < timeTags.Count; i++)
            {
                var expected = TestCaseTagHelper.ParseTimeTag(expectTimeTags[i]);
                Assert.AreEqual(expected.Index, timeTags[i].Index);
                Assert.AreEqual(expected.Time, timeTags[i].Time);
            }
        }

        [TestCase(new[] { "[0,0]:ruby" }, new[] { "[0,0]:ルビ" }, new[] { "[0,0]:ruby", "[7,7]:ルビ" })]
        [TestCase(new[] { "[0,0]:" }, new[] { "[0,0]:" }, new[] { "[0,0]:", "[7,7]:" })]
        [TestCase(new[] { "[0,3]:" }, new[] { "[0,3]:" }, new[] { "[0,3]:", "[7,10]:" })]
        [TestCase(new[] { "[0,10]:" }, new[] { "[0,10]:" }, new[] { "[0,10]:", "[7,14]:" })] // will auto-fix ruby index.
        [TestCase(new[] { "[-10,0]:" }, new[] { "[-10,0]:" }, new[] { "[-10,0]:", "[0,7]:" })] // will auto-fix ruby index.
        public void TestCombineLyricRubyTag(string[] firstRubyTags, string[] secondRubyTags, string[] expectedRubyTags)
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

            var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);

            var expected = TestCaseTagHelper.ParseRubyTags(expectedRubyTags);
            var actual = combineLyric.RubyTags;
            TextTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new[] { "[0,0]:romaji" }, new[] { "[0,0]:ローマ字" }, new[] { "[0,0]:romaji", "[7,7]:ローマ字" })]
        [TestCase(new[] { "[0,0]:" }, new[] { "[0,0]:" }, new[] { "[0,0]:", "[7,7]:" })]
        [TestCase(new[] { "[0,3]:" }, new[] { "[0,3]:" }, new[] { "[0,3]:", "[7,10]:" })]
        [TestCase(new[] { "[0,10]:" }, new[] { "[0,10]:" }, new[] { "[0,10]:", "[7,14]:" })] // will auto-fix romaji index.
        [TestCase(new[] { "[-10,0]:" }, new[] { "[-10,0]:" }, new[] { "[-10,0]:", "[0,7]:" })] // will auto-fix romaji index.
        public void TestCombineLyricRomajiTag(string[] firstRomajiTags, string[] secondRomajiTags, string[] expectedRomajiTags)
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

            var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);

            var expected = TestCaseTagHelper.ParseRomajiTags(expectedRomajiTags);
            var actual = combineLyric.RomajiTags;
            TextTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new double[] { 1000, 0 }, new double[] { 1000, 0 }, new double[] { 1000, 0 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 2000, 0 }, new double[] { 1000, 1000 })]
        [TestCase(new double[] { 1000, 0 }, new double[] { 2000, 2000 }, new double[] { 1000, 3000 })]
        [TestCase(new double[] { 1000, 5000 }, new double[] { 1000, 0 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 1000, 5000 }, new double[] { 2000, 0 }, new double[] { 1000, 5000 })]
        [TestCase(new double[] { 2000, 5000 }, new double[] { 1000, 0 }, new double[] { 1000, 6000 })]
        [TestCase(new double[] { 2000, 5000 }, new double[] { 1000, 10000 }, new double[] { 1000, 10000 })]
        public void TestCombineLyricTime(double[] firstLyricTime, double[] secondLyricTime, double[] expectedTime)
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
            var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);
            Assert.AreEqual(expectedTime[0], combineLyric.StartTime);
            Assert.AreEqual(expectedTime[1], combineLyric.Duration);
        }

        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 1, 2 })]
        [TestCase(new[] { 1 }, new[] { 1 }, new[] { 1 })] // deal with duplicated case.
        [TestCase(new[] { 1 }, new[] { -2 }, new[] { 1, -2 })] // deal with id not right case.
        public void TestCombineLyricSinger(int[] firstSingerIndexes, int[] secondSingerIndexes, int[] expected)
        {
            var lyric1 = new Lyric { Singers = firstSingerIndexes };
            var lyric2 = new Lyric { Singers = secondSingerIndexes };

            var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);

            var actual = combineLyric.Singers;
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1, 1)]
        [TestCase(54, 54, 54)]
        [TestCase(null, 1, null)]
        [TestCase(1, null, null)]
        [TestCase(null, null, null)]
        public void TestCombineLayoutLanguage(int? firstLcid, int? secondLcid, int? expectedLcid)
        {
            var cultureInfo1 = firstLcid != null ? new CultureInfo(firstLcid.Value) : null;
            var cultureInfo2 = secondLcid != null ? new CultureInfo(secondLcid.Value) : null;

            var lyric1 = new Lyric { Language = cultureInfo1 };
            var lyric2 = new Lyric { Language = cultureInfo2 };

            var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);

            var expected = expectedLcid != null ? new CultureInfo(expectedLcid.Value) : null;
            var actual = combineLyric.Language;
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Lock

        [TestCase(new[] { LockState.Full, LockState.Partial, LockState.None }, 1)]
        [TestCase(new LockState[] { }, 0)]
        public void TestFindUnlockLyrics(LockState[] lockStates, int? expected)
        {
            var lyrics = lockStates.Select(x => new Lyric
            {
                Text = "karaoke",
                Lock = x
            });

            int actual = LyricsUtils.FindUnlockLyrics(lyrics).Length;
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
