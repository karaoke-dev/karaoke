// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class LyricUtilsTest
    {
        #region progessing

        [TestCase("karaoke", 2, 2, "kaoke")]
        [TestCase("カラオケ", 2, 2, "カラ")]
        [TestCase("カラオケ", -1, 2, null)] // test start position not in the range
        [TestCase("カラオケ", 4, 2, "カラオケ")] // test start position not in the range, but it's valid
        [TestCase("カラオケ", 0, -1, null)] // test end position not in the range
        [TestCase("カラオケ", 0, 100, "")] // test end position not in the range
        [TestCase("", 0, 0, "")]
        [TestCase(null, 0, 0, null)]
        public void TestRemoveText(string text, int position, int count, string expected)
        {
            try
            {
                var lyric = new Lyric { Text = text };
                LyricUtils.RemoveText(lyric, position, count);
                Assert.AreEqual(expected, lyric.Text);
            }
            catch
            {
                Assert.IsNull(expected);
            }
        }

        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 0, 2, new[] { "[0,1]:お", "[1,2]:け" })]
        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 1, 1, new[] { "[0,1]:か", "[1,2]:お", "[2,3]:け" })]
        [TestCase(new[] { "[0,2]:から", "[2,4]:おけ" }, 1, 2, new[] { "[0,1]:から", "[1,2]:おけ" })]
        [TestCase(new[] { "[0,4]:からおけ" }, 0, 1, new[] { "[0,3]:からおけ" })]
        [TestCase(new[] { "[0,4]:からおけ" }, 1, 2, new[] { "[0,2]:からおけ" })]
        public void TestRemoveTextRuby(string[] rubies, int position, int count, string[] targetRubies)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies),
            };
            LyricUtils.RemoveText(lyric, position, count);

            var expected = TestCaseTagHelper.ParseRubyTags(targetRubies);
            var actual = lyric.RubyTags;
            TextTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, 0, 2, new[] { "[0,1]:o", "[1,2]:ke" })]
        [TestCase(new[] { "[0,2]:kara", "[2,4]:oke" }, 1, 2, new[] { "[0,1]:kara", "[1,2]:oke" })]
        public void TestRemoveTextRomaji(string[] romajies, int position, int count, string[] targetRomajies)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies),
            };
            LyricUtils.RemoveText(lyric, position, count);

            var expected = TestCaseTagHelper.ParseRomajiTags(targetRomajies);
            var actual = lyric.RomajiTags;
            TextTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 0, 2, new[] { "[0,start]:3000", "[1,start]:4000" })]
        [TestCase(new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:" }, 0, 2, new[] { "[0,start]:", "[1,start]:" })]
        [TestCase(new[] { "[0,start]:1000", "[2,start]:3000" }, 1, 2, new[] { "[0,start]:1000" })]
        public void TestRemoveTextTimeTag(string[] timeTags, int position, int count, string[] actualTimeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };
            LyricUtils.RemoveText(lyric, position, count);

            var expected = TestCaseTagHelper.ParseTimeTags(actualTimeTags);
            var actual = lyric.TimeTags;
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase("kake", 2, "rao", "karaoke")]
        [TestCase("karaoke", 7, "-", "karaoke-")]
        [TestCase("オケ", 0, "カラ", "カラオケ")]
        [TestCase("オケ", -1, "カラ", "カラオケ")] // test start position not in the range, but it's valid.
        [TestCase("カラ", 4, "オケ", "カラオケ")] // test start position not in the range, but it's valid.
        [TestCase("", 0, "カラオケ", "カラオケ")]
        [TestCase(null, 0, "カラオケ", "カラオケ")]
        public void TestAddTextText(string text, int position, string addedText, string expected)
        {
            var lyric = new Lyric { Text = text };
            LyricUtils.AddText(lyric, position, addedText);

            string actual = lyric.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 0, "karaoke", new[] { "[7,8]:か", "[8,9]:ら", "[9,10]:お", "[10,11]:け" })]
        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 2, "karaoke", new[] { "[0,1]:か", "[1,2]:ら", "[9,10]:お", "[10,11]:け" })]
        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 4, "karaoke", new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" })]
        public void TextAddTextRuby(string[] rubies, int position, string addedText, string[] targetRubies)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies),
            };
            LyricUtils.AddText(lyric, position, addedText);

            var expected = TestCaseTagHelper.ParseRubyTags(targetRubies);
            var actual = lyric.RubyTags;
            TextTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 0, "karaoke", new[] { "[7,8]:か", "[8,9]:ら", "[9,10]:お", "[10,11]:け" })]
        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 2, "karaoke", new[] { "[0,1]:か", "[1,2]:ら", "[9,10]:お", "[10,11]:け" })]
        [TestCase(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, 4, "karaoke", new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" })]
        public void TextAddTextRomaji(string[] romajies, int position, string addedText, string[] targetRomajies)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies),
            };
            LyricUtils.AddText(lyric, position, addedText);

            var expected = TestCaseTagHelper.ParseRomajiTags(targetRomajies);
            var actual = lyric.RomajiTags;
            TextTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 0, "karaoke", new[] { "[7,start]:1000", "[8,start]:2000", "[9,start]:3000", "[10,start]:4000" })]
        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 2, "karaoke", new[] { "[0,start]:1000", "[1,start]:2000", "[9,start]:3000", "[10,start]:4000" })]
        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 4, "karaoke", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" })]
        [TestCase(new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:" }, 0, "karaoke", new[] { "[7,start]:", "[8,start]:", "[9,start]:", "[10,start]:" })]
        [TestCase(new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:" }, 2, "karaoke", new[] { "[0,start]:", "[1,start]:", "[9,start]:", "[10,start]:" })]
        [TestCase(new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:" }, 4, "karaoke", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:" })]
        public void TestAddTextTimeTag(string[] timeTags, int position, string addedText, string[] actualTimeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };
            LyricUtils.AddText(lyric, position, addedText);

            var expected = TestCaseTagHelper.ParseTimeTags(actualTimeTags);
            var actual = lyric.TimeTags;
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        #endregion

        #region Time tag

        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, true)]
        [TestCase(new string[] { }, false)]
        public void TestHasTimedTimeTags(string[] timeTags, bool expected)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };

            bool actual = LyricUtils.HasTimedTimeTags(lyric);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "[0,start]", "か-")]
        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "[0,end]", "-か")]
        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "[3,start]", "け-")]
        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "[3,end]", "-け")]
        [TestCase("[00:01.00]からおけ[00:05.00]", "[0,start]", "からおけ-")]
        [TestCase("[00:01.00]からおけ[00:05.00]", "[0,end]", "-か")]
        [TestCase("[00:01.00]からおけ[00:05.00]", "[3,start]", "け-")]
        [TestCase("[00:01.00]からおけ[00:05.00]", "[3,end]", "-からおけ")]
        [TestCase("からおけ", "[0,start]", "からおけ-")]
        [TestCase("からおけ", "[0,end]", "-か")]
        [TestCase("からおけ", "[3,start]", "け-")]
        [TestCase("からおけ", "[3,end]", "-からおけ")]
        [TestCase("からおけ", "[4,start]", "-")] // not showing text if index out of range.
        [TestCase("からおけ", "[4,end]", "-")]
        [TestCase("からおけ", "[-1,start]", "-")]
        [TestCase("からおけ", "[-1,end]", "-")]
        public void TestGetTimeTagIndexDisplayText(string text, string textIndexStr, string expected)
        {
            var lyric = TestCaseTagHelper.ParseLyricWithTimeTag(text);
            var textIndex = TestCaseTagHelper.ParseTextIndex(textIndexStr);

            string actual = LyricUtils.GetTimeTagIndexDisplayText(lyric, textIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "[0,start]", "か-")]
        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "[3,start]", "け-")]
        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "[3,end]", "-け")]
        [TestCase("[00:01.00]からおけ[00:05.00]", "[0,start]", "からおけ-")]
        [TestCase("[00:01.00]からおけ[00:05.00]", "[3,end]", "-からおけ")]
        public void TestGetTimeTagDisplayText(string text, string textIndexStr, string expected)
        {
            var lyric = TestCaseTagHelper.ParseLyricWithTimeTag(text);
            var textIndex = TestCaseTagHelper.ParseTextIndex(textIndexStr);
            var timeTag = lyric.TimeTags?.Where(x => x.Index == textIndex).FirstOrDefault();

            string actual = LyricUtils.GetTimeTagDisplayText(lyric, timeTag);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, "(か)-")]
        [TestCase(1, "(か)-")]
        [TestCase(2, "-(か)")]
        [TestCase(3, "ラ-")]
        [TestCase(4, "ラ-")]
        [TestCase(5, "-ラ")]
        [TestCase(6, "(お)-")]
        [TestCase(7, "(け)-")]
        [TestCase(8, "(け)-")]
        [TestCase(9, "-(け)")]
        public void TestGetTimeTagDisplayRubyText(int indexOfTimeTag, string expected)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[0,start]:1000",
                    "[0,end]:1000",
                    "[1,start]:2000",
                    "[1,start]:2000",
                    "[1,end]:2000",
                    "[2,start]:3000",
                    "[2,start]:3000",
                    "[3,start]:4000",
                    "[3,end]:5000",
                }),
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:か",
                    "[2,4]:おけ",
                })
            };
            var timeTag = lyric.TimeTags[indexOfTimeTag];

            string actual = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Ruby/romaji tag

        [TestCase("からおけ", 0, true)]
        [TestCase("からおけ", 4, true)]
        [TestCase("からおけ", -1, false)]
        [TestCase("からおけ", 5, false)]
        [TestCase("", 0, true)]
        [TestCase(null, 0, true)]
        public void TestAbleToInsertTextTagAtIndex(string text, int index, bool expected)
        {
            var lyric = TestCaseTagHelper.ParseLyricWithTimeTag(text);

            bool actual = LyricUtils.AbleToInsertTextTagAtIndex(lyric, index);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Time display

        [TestCase(0, 0, "00:00:000 - 00:00:000")]
        [TestCase(0, 1000, "00:00:000 - 00:01:000")]
        [TestCase(1000, 0, "00:01:000 - 00:00:000")] // do not check time order in here
        [TestCase(-1000, 0, "-00:01:000 - 00:00:000")]
        [TestCase(0, -1000, "00:00:000 - -00:01:000")]
        public void TestLyricTimeFormattedString(double startTime, double endTime, string expected)
        {
            var lyric = new Lyric
            {
                StartTime = startTime,
                Duration = endTime - startTime
            };

            string actual = LyricUtils.LyricTimeFormattedString(lyric);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, "00:01:000 - 00:04:000")]
        [TestCase(new[] { "[0,start]:4000", "[1,start]:3000", "[2,start]:2000", "[3,start]:1000" }, "00:01:000 - 00:04:000")] // should display right-time even it's not being ordered.
        [TestCase(new[] { "[3,start]:4000", "[2,start]:3000", "[1,start]:2000", "[0,start]:1000" }, "00:01:000 - 00:04:000")] // should display right-time even it's not being ordered.
        [TestCase(new[] { "[0,start]:1000" }, "00:01:000 - 00:01:000")]
        [TestCase(new string[] { }, "--:--:--- - --:--:---")]
        public void TestTimeTagTimeFormattedString(string[] timeTags, string expected)
        {
            var lyric = new Lyric
            {
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };

            string actual = LyricUtils.TimeTagTimeFormattedString(lyric);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Singer

        [TestCase(new[] { "[1]name:Singer1" }, "[1]name:Singer1", true)]
        [TestCase(new[] { "[1]name:Singer1" }, "[2]name:Singer2", false)]
        [TestCase(new string[] { }, "[1]name:Singer1", false)]
        public void TestContainsSinger(string[] existSingers, string compareSinger, bool expected)
        {
            var singer = TestCaseTagHelper.ParseSinger(compareSinger);
            var lyric = new Lyric
            {
                Singers = TestCaseTagHelper.ParseSingers(existSingers)?.Select(x => x.ID).ToArray()
            };

            bool actual = LyricUtils.ContainsSinger(lyric, singer);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { "[1]name:Singer1" }, new[] { "[1]name:Singer1", "[1]name:Singer1" }, true)]
        [TestCase(new[] { "[1]name:Singer1" }, new[] { "[1]name:Singer1" }, true)]
        [TestCase(new[] { "[1]name:Singer1" }, new[] { "[2]name:Singer2" }, false)]
        [TestCase(new string[] { }, new[] { "[1]name:Singer1" }, true)]
        public void TestOnlyContainsSingers(string[] existSingers, string[] compareSingers, bool expected)
        {
            var singers = TestCaseTagHelper.ParseSingers(compareSingers).ToList();
            var lyric = new Lyric
            {
                Singers = TestCaseTagHelper.ParseSingers(existSingers)?.Select(x => x.ID).ToArray()
            };

            bool actual = LyricUtils.OnlyContainsSingers(lyric, singers);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Check

        [TestCase("[1000,3000]:karaoke", false)]
        [TestCase("[1000,1000]:karaoke", false)] // it's ok to let it pass(for no reason now).
        [TestCase("[1000,0]:karaoke", true)]
        public void TestCheckIsTimeOverlapping(string lyricText, bool expected)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);

            bool actual = LyricUtils.CheckIsTimeOverlapping(lyric);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[2,start]:2000", "[4,start]:3000", "[5,start]:4000", "[7,end]:5000" }, false)]
        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)]
        [TestCase("[1000,2000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)] // not check end time now.
        [TestCase("[2000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, true)]
        public void TestCheckIsStartTimeInvalid(string lyricText, string[] timeTags, bool expected)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

            bool actual = LyricUtils.CheckIsStartTimeInvalid(lyric);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[2,start]:2000", "[4,start]:3000", "[5,start]:4000", "[7,end]:5000" }, false)]
        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)]
        [TestCase("[2000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)] // not check start time now.
        [TestCase("[1000,2000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, true)]
        public void TestCheckIsEndTimeInvalid(string lyricText, string[] timeTags, bool expected)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

            bool actual = LyricUtils.CheckIsEndTimeInvalid(lyric);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
