// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
        public void TestRemoveTextText(string text, int position, int count, string actualText)
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
        [TestCase(new[] { "[0,2]:から", "[2,4]:おけ" }, 1, 2, new[] { "[0,1]:から", "[1,2]:おけ" })]
        public void TestRemoveTextRuby(string[] rubies, int position, int count, string[] targetRubies)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies),
            };
            LyricUtils.RemoveText(lyric, position, count);
            Assert.AreEqual(lyric.RubyTags, TestCaseTagHelper.ParseRubyTags(targetRubies));
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
            Assert.AreEqual(lyric.RomajiTags, TestCaseTagHelper.ParseRomajiTags(targetRomajies));
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
            TimeTagAssert.AreEqual(lyric.TimeTags, TestCaseTagHelper.ParseTimeTags(actualTimeTags));
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
            Assert.AreEqual(lyric.RubyTags, TestCaseTagHelper.ParseRubyTags(targetRubies));
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
            Assert.AreEqual(lyric.RomajiTags, TestCaseTagHelper.ParseRomajiTags(targetRomajies));
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
            TimeTagAssert.AreEqual(lyric.TimeTags, TestCaseTagHelper.ParseTimeTags(actualTimeTags));
        }

        #endregion

        #region create default

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
        [TestCase(new[] { "[0,start]:1000"  }, "00:01:000 - 00:01:000")]
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
    }
}
