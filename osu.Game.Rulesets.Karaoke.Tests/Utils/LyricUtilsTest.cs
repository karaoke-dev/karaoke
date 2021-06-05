// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Extensions;
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
        public void TestRemoveText(string text, int position, int count, string actualText)
        {
            try
            {
                var lyric = new Lyric { Text = text };
                LyricUtils.RemoveText(lyric, position, count);
                Assert.AreEqual(lyric.Text, actualText);
            }
            catch
            {
                Assert.Null(actualText);
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
            TextTagAssert.ArePropertyEqual(lyric.RubyTags, TestCaseTagHelper.ParseRubyTags(targetRubies));
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
            TextTagAssert.ArePropertyEqual(lyric.RomajiTags, TestCaseTagHelper.ParseRomajiTags(targetRomajies));
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
            TimeTagAssert.ArePropertyEqual(lyric.TimeTags, TestCaseTagHelper.ParseTimeTags(actualTimeTags));
        }

        [TestCase("kake", 2, "rao", "karaoke")]
        [TestCase("オケ", 0, "カラ", "カラオケ")]
        [TestCase("オケ", -1, "カラ", "カラオケ")] // test start position not in the range, but it's valid.
        [TestCase("カラ", 4, "オケ", "カラオケ")] // test start position not in the range, but it's valid.
        [TestCase("", 0, "カラオケ", "カラオケ")]
        [TestCase(null, 0, "カラオケ", "カラオケ")]
        public void TestAddTextText(string text, int position, string addedText, string actualText)
        {
            var lyric = new Lyric { Text = text };
            LyricUtils.AddText(lyric, position, addedText);
            Assert.AreEqual(lyric.Text, actualText);
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
            TextTagAssert.ArePropertyEqual(lyric.RubyTags, TestCaseTagHelper.ParseRubyTags(targetRubies));
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
            TextTagAssert.ArePropertyEqual(lyric.RomajiTags, TestCaseTagHelper.ParseRomajiTags(targetRomajies));
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
            TimeTagAssert.ArePropertyEqual(lyric.TimeTags, TestCaseTagHelper.ParseTimeTags(actualTimeTags));
        }

        #endregion

        #region create default

        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new[] { "カ", "ラ", "オ", "ケ" })]
        public void TestCreateDefaultNotes(string[] timeTags, string[] noteTexts)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };

            var notes = LyricUtils.CreateDefaultNotes(lyric);
            Assert.AreEqual(notes.Select(x => x.Text).ToArray(), noteTexts);
        }

        #endregion

        #region Time tag

        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, true)]
        [TestCase(new string[] { }, false)]
        [TestCase(null, false)]
        public void TestHasTimedTimeTags(string[] timeTags, bool hasTimedTimeTag)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };
            Assert.AreEqual(LyricUtils.HasTimedTimeTags(lyric), hasTimedTimeTag);
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
        public void TestGetTimeTagIndexDisplayText(string text, string textIndexStr, string actual)
        {
            var lyric = TestCaseTagHelper.ParseLyricWithTimeTag(text);
            var textIndex = TestCaseTagHelper.ParseTextIndex(textIndexStr);
            Assert.AreEqual(LyricUtils.GetTimeTagIndexDisplayText(lyric, textIndex), actual);
        }

        #endregion

        #region Ruby/romaji tag

        [TestCase("からおけ", 0, true)]
        [TestCase("からおけ", 4, true)]
        [TestCase("からおけ", -1, false)]
        [TestCase("からおけ", 5, false)]
        [TestCase("", 0, true)]
        [TestCase(null, 0, true)]
        public void TestAbleToInsertTextTagAtIndex(string text, int index, bool actual)
        {
            var lyric = TestCaseTagHelper.ParseLyricWithTimeTag(text);
            Assert.AreEqual(LyricUtils.AbleToInsertTextTagAtIndex(lyric, index), actual);
        }

        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, "[3,4]:ke", true)]
        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, "[0,4]:karaoke", false)]
        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, null, false)]
        [TestCase(new string[] { }, "[0,4]:karaoke", false)]
        [TestCase(null, "[0,4]:karaoke", false)]
        public void TestRemoveTextTag(string[] textTags, string removeTextTag, bool actual)
        {
            var lyric = new Lyric
            {
                Text = "からおけ",
                RubyTags = TestCaseTagHelper.ParseRubyTags(textTags),
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(textTags)
            };

            var fromIndex = textTags?.IndexOf(removeTextTag) ?? -1;

            // test ruby and romaji at the same test.
            var removeRubyTag = fromIndex >= 0 ? lyric.RubyTags[fromIndex] : TestCaseTagHelper.ParseRubyTag(removeTextTag);
            var removeRomajiTag = fromIndex >= 0 ? lyric.RomajiTags[fromIndex] : TestCaseTagHelper.ParseRomajiTag(removeTextTag);

            Assert.AreEqual(LyricUtils.RemoveTextTag(lyric, removeRubyTag), actual);
            Assert.AreEqual(LyricUtils.RemoveTextTag(lyric, removeRomajiTag), actual);
        }

        #endregion

        #region Time display

        [TestCase(0, 0, "00:00:000 - 00:00:000")]
        [TestCase(0, 1000, "00:00:000 - 00:01:000")]
        [TestCase(1000, 0, "00:01:000 - 00:00:000")] // do not check time order in here
        [TestCase(-1000, 0, "-00:01:000 - 00:00:000")]
        [TestCase(0, -1000, "00:00:000 - -00:01:000")]
        public void TestLyricTimeFormattedString(double startTime, double endTime, string format)
        {
            var lyric = new Lyric
            {
                StartTime = startTime,
                Duration = endTime - startTime
            };

            Assert.AreEqual(LyricUtils.LyricTimeFormattedString(lyric), format);
        }

        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, "00:01:000 - 00:04:000")]
        [TestCase(new[] { "[0,start]:4000", "[1,start]:3000", "[2,start]:2000", "[3,start]:1000" }, "00:01:000 - 00:04:000")] // should display right-time even it's not being ordered.
        [TestCase(new[] { "[3,start]:4000", "[2,start]:3000", "[1,start]:2000", "[0,start]:1000" }, "00:01:000 - 00:04:000")] // should display right-time even it's not being ordered.
        [TestCase(new[] { "[0,start]:1000" }, "00:01:000 - 00:01:000")]
        [TestCase(new string[] { }, "--:--:--- - --:--:---")]
        [TestCase(null, "--:--:--- - --:--:---")]
        public void TestTimeTagTimeFormattedString(string[] timeTags, string format)
        {
            var lyric = new Lyric
            {
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };

            Assert.AreEqual(LyricUtils.TimeTagTimeFormattedString(lyric), format);
        }

        #endregion

        #region Singer

        [TestCase(null, "[1]name:Singer1", true, new[] { 1 })]
        [TestCase(new[] { "[1]name:Singer1" }, "[1]name:Singer1", false, new[] { 1 })]
        [TestCase(new[] { "[1]name:Singer1" }, "[2]name:Singer2", true, new[] { 1, 2 })]
        [TestCase(null, "[0]name:Singer0", false, null)] // should not add invalid singer.
        [TestCase(new[] { "[1]name:Singer1" }, "[0]name:Singer0", false, null)] // should not add invalid singer.
        public void TestAddSinger(string[] existSingers, string addSinger, bool isAdded, int[] actualSingers)
        {
            var singer = TestCaseTagHelper.ParseSinger(addSinger);
            var lyric = new Lyric
            {
                Singers = TestCaseTagHelper.ParseSingers(existSingers)?.Select(x => x.ID).ToArray()
            };

            try
            {
                Assert.AreEqual(LyricUtils.AddSinger(lyric, singer), isAdded);
                Assert.AreEqual(lyric.Singers, actualSingers);
            }
            catch
            {
                Assert.IsNull(actualSingers);
            }
        }

        [TestCase(new[] { "[1]name:Singer1" }, "[1]name:Singer1", true, new int[] { })]
        [TestCase(new[] { "[1]name:Singer1" }, "[2]name:Singer2", false, new[] { 1 })]
        [TestCase(new string[] { }, "[1]name:Singer1", false, new int[] { })]
        [TestCase(null, "[1]name:Singer1", false, null)] // null singer index will not be initialize if remove singer.
        [TestCase(null, "[0]name:Singer0", false, null)] // should not remove invalid singer.
        [TestCase(new[] { "[1]name:Singer1" }, "[0]name:Singer0", false, null)] // should not remove invalid singer.
        public void TestRemoveSinger(string[] existSingers, string removeSinger, bool isAdded, int[] actualSingers)
        {
            var singer = TestCaseTagHelper.ParseSinger(removeSinger);
            var lyric = new Lyric
            {
                Singers = TestCaseTagHelper.ParseSingers(existSingers)?.Select(x => x.ID).ToArray()
            };

            try
            {
                Assert.AreEqual(LyricUtils.RemoveSinger(lyric, singer), isAdded);
                Assert.AreEqual(lyric.Singers, actualSingers);
            }
            catch
            {
                Assert.IsNull(actualSingers);
            }
        }

        [TestCase(new[] { "[1]name:Singer1" }, "[1]name:Singer1", true)]
        [TestCase(new[] { "[1]name:Singer1" }, "[2]name:Singer2", false)]
        [TestCase(new string[] { }, "[1]name:Singer1", false)]
        [TestCase(null, "[1]name:Singer1", false)]
        public void TestContainsSinger(string[] existSingers, string compareSinger, bool isContain)
        {
            var singer = TestCaseTagHelper.ParseSinger(compareSinger);
            var lyric = new Lyric
            {
                Singers = TestCaseTagHelper.ParseSingers(existSingers)?.Select(x => x.ID).ToArray()
            };
            Assert.AreEqual(LyricUtils.ContainsSinger(lyric, singer), isContain);
        }

        [TestCase(new[] { "[1]name:Singer1" }, new[] { "[1]name:Singer1", "[1]name:Singer1" }, true)]
        [TestCase(new[] { "[1]name:Singer1" }, new[] { "[1]name:Singer1" }, true)]
        [TestCase(new[] { "[1]name:Singer1" }, new[] { "[2]name:Singer2" }, false)]
        [TestCase(new string[] { }, new[] { "[1]name:Singer1" }, true)]
        [TestCase(null, new[] { "[1]name:Singer1" }, true)]
        public void TestOnlyContainsSingers(string[] existSingers, string[] compareSingers, bool isContain)
        {
            var singers = TestCaseTagHelper.ParseSingers(compareSingers).ToList();
            var lyric = new Lyric
            {
                Singers = TestCaseTagHelper.ParseSingers(existSingers)?.Select(x => x.ID).ToArray()
            };
            Assert.AreEqual(LyricUtils.OnlyContainsSingers(lyric, singers), isContain);
        }

        #endregion

        #region Check

        [TestCase("[1000,3000]:karaoke", false)]
        [TestCase("[1000,1000]:karaoke", false)] // it's ok to let it pass(for no reason now).
        [TestCase("[1000,0]:karaoke", true)]
        public void TestCheckIsTimeOverlapping(string lyricText, bool actual)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);
            Assert.AreEqual(LyricUtils.CheckIsTimeOverlapping(lyric), actual);
        }

        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[2,start]:2000", "[4,start]:3000", "[5,start]:4000", "[7,end]:5000" }, false)]
        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)]
        [TestCase("[1000,2000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)] // not check end time now.
        [TestCase("[2000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, true)]
        public void TestCheckIsStartTimeInvalid(string lyricText, string[] timeTags, bool actual)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);
            Assert.AreEqual(LyricUtils.CheckIsStartTimeInvalid(lyric), actual);
        }

        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[2,start]:2000", "[4,start]:3000", "[5,start]:4000", "[7,end]:5000" }, false)]
        [TestCase("[1000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)]
        [TestCase("[2000,5000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, false)] // not check start time now.
        [TestCase("[1000,2000]:karaoke", new[] { "[0,start]:1000", "[7,end]:5000" }, true)]
        public void TestCheckIsEndTimeInvalid(string lyricText, string[] timeTags, bool actual)
        {
            var lyric = TestCaseTagHelper.ParseLyric(lyricText);
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);
            Assert.AreEqual(LyricUtils.CheckIsEndTimeInvalid(lyric), actual);
        }

        #endregion
    }
}
