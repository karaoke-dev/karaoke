﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Utils;

public class LyricUtilsTest
{
    #region progessing

    [TestCase("karaoke", 2, 2, "kaoke")]
    [TestCase("カラオケ", 2, 2, "カラ")]
    [TestCase("カラオケ", -1, 2, null)] // test start char gap not in the range
    [TestCase("カラオケ", 4, 2, "カラオケ")] // test start char gap not in the range, but it's valid
    [TestCase("カラオケ", 0, -1, null)] // test end char gap not in the range
    [TestCase("カラオケ", 0, 100, "")] // test end char gap not in the range
    [TestCase("", 0, 0, "")]
    public void TestRemoveText(string text, int charGap, int count, string? expected)
    {
        var lyric = new Lyric { Text = text };

        if (expected != null)
        {
            LyricUtils.RemoveText(lyric, charGap, count);
            Assert.That(lyric.Text, Is.EqualTo(expected));
        }
        else
        {
            Assert.Catch(() => LyricUtils.RemoveText(lyric, charGap, count));
        }
    }

    [TestCase(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, 0, 2, new[] { "[0]:お", "[1]:け" })]
    [TestCase(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, 1, 1, new[] { "[0]:か", "[1]:お", "[2]:け" })]
    [TestCase(new[] { "[0,1]:から", "[2,3]:おけ" }, 1, 2, new[] { "[0]:から", "[1]:おけ" })]
    [TestCase(new[] { "[0,3]:からおけ" }, 0, 1, new[] { "[0,2]:からおけ" })]
    [TestCase(new[] { "[0,3]:からおけ" }, 1, 2, new[] { "[0,1]:からおけ" })]
    public void TestRemoveTextRuby(string[] rubies, int charGap, int count, string[] targetRubies)
    {
        var lyric = new Lyric
        {
            Text = "カラオケ",
            RubyTags = TestCaseTagHelper.ParseRubyTags(rubies),
        };
        LyricUtils.RemoveText(lyric, charGap, count);

        var expected = TestCaseTagHelper.ParseRubyTags(targetRubies);
        var actual = lyric.RubyTags;
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 0, 2, new[] { "[0,start]:3000", "[1,start]:4000" })]
    [TestCase(new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]" }, 0, 2, new[] { "[0,start]", "[1,start]" })]
    [TestCase(new[] { "[0,start]:1000", "[2,start]:3000" }, 1, 2, new[] { "[0,start]:1000" })]
    public void TestRemoveTextTimeTag(string[] timeTags, int charGap, int count, string[] actualTimeTags)
    {
        var lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };
        LyricUtils.RemoveText(lyric, charGap, count);

        var expected = TestCaseTagHelper.ParseTimeTags(actualTimeTags);
        var actual = lyric.TimeTags;
        TimeTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase("kake", 2, "rao", "karaoke")]
    [TestCase("karaoke", 7, "-", "karaoke-")]
    [TestCase("オケ", 0, "カラ", "カラオケ")]
    [TestCase("オケ", -1, "カラ", "カラオケ")] // test start char gap not in the range, but it's valid.
    [TestCase("カラ", 4, "オケ", "カラオケ")] // test start char gap not in the range, but it's valid.
    [TestCase("", 0, "カラオケ", "カラオケ")]
    public void TestAddTextText(string text, int charGap, string addedText, string expected)
    {
        var lyric = new Lyric { Text = text };
        LyricUtils.AddText(lyric, charGap, addedText);

        string actual = lyric.Text;
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, 0, "karaoke", new[] { "[7]:か", "[8]:ら", "[9]:お", "[10]:け" })]
    [TestCase(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, 2, "karaoke", new[] { "[0]:か", "[1]:ら", "[9]:お", "[10]:け" })]
    [TestCase(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, 4, "karaoke", new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" })]
    public void TestAddTextRuby(string[] rubies, int charGap, string addedText, string[] targetRubies)
    {
        var lyric = new Lyric
        {
            Text = "カラオケ",
            RubyTags = TestCaseTagHelper.ParseRubyTags(rubies),
        };
        LyricUtils.AddText(lyric, charGap, addedText);

        var expected = TestCaseTagHelper.ParseRubyTags(targetRubies);
        var actual = lyric.RubyTags;
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 0, "karaoke", new[] { "[7,start]:1000", "[8,start]:2000", "[9,start]:3000", "[10,start]:4000" })]
    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 2, "karaoke", new[] { "[0,start]:1000", "[1,start]:2000", "[9,start]:3000", "[10,start]:4000" })]
    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, 4, "karaoke", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" })]
    [TestCase(new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]" }, 0, "karaoke", new[] { "[7,start]", "[8,start]", "[9,start]", "[10,start]" })]
    [TestCase(new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]" }, 2, "karaoke", new[] { "[0,start]", "[1,start]", "[9,start]", "[10,start]" })]
    [TestCase(new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]" }, 4, "karaoke", new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]" })]
    public void TestAddTextTimeTag(string[] timeTags, int charGap, string addedText, string[] actualTimeTags)
    {
        var lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };
        LyricUtils.AddText(lyric, charGap, addedText);

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
        Assert.That(expected, Is.EqualTo(actual));
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
        Assert.That(expected, Is.EqualTo(actual));
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
        var timeTag = lyric.TimeTags.First(x => x.Index == textIndex);

        string actual = LyricUtils.GetTimeTagDisplayText(lyric, timeTag);
        Assert.That(expected, Is.EqualTo(actual));
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
                "[0]:か",
                "[2,3]:おけ",
            }),
        };
        var timeTag = lyric.TimeTags[indexOfTimeTag];

        string actual = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag);
        Assert.That(expected, Is.EqualTo(actual));
    }

    #endregion

    #region Ruby tag

    [TestCase("からおけ", 0, true)]
    [TestCase("からおけ", 4, true)]
    [TestCase("からおけ", -1, false)]
    [TestCase("からおけ", 5, false)]
    [TestCase("", 0, true)]
    public void TestAbleToInsertRubyTagAtIndex(string text, int index, bool expected)
    {
        var lyric = TestCaseTagHelper.ParseLyricWithTimeTag(text);

        bool actual = LyricUtils.AbleToInsertRubyTagAtIndex(lyric, index);
        Assert.That(expected, Is.EqualTo(actual));
    }

    #endregion

    #region Time display

    [TestCase(0, 0, "00:00:000 - 00:00:000")]
    [TestCase(0, 1000, "00:00:000 - 00:01:000")]
    [TestCase(1000, 0, "00:00:000 - 00:01:000")] // should check the order of time.
    [TestCase(-1000, 0, "-00:01:000 - 00:00:000")]
    [TestCase(0, -1000, "-00:01:000 - 00:00:000")] // should check the order of time.
    public void TestLyricTimeFormattedString(double startTime, double endTime, string expected)
    {
        var lyric = new Lyric
        {
            TimeTags = new List<TimeTag>
            {
                new (new TextIndex(), startTime),
                new (new TextIndex(), endTime),
            },
        };

        string actual = LyricUtils.LyricTimeFormattedString(lyric);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000" }, "00:01:000 - 00:04:000")]
    [TestCase(new[] { "[0,start]:4000", "[1,start]:3000", "[2,start]:2000", "[3,start]:1000" }, "00:01:000 - 00:04:000")] // should display right-time even it's not being ordered.
    [TestCase(new[] { "[3,start]:4000", "[2,start]:3000", "[1,start]:2000", "[0,start]:1000" }, "00:01:000 - 00:04:000")] // should display right-time even it's not being ordered.
    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[1,start]" }, "00:01:000 - 00:02:000")] // with null case.
    [TestCase(new[] { "[1,start]", "[0,start]:1000", "[1,start]:2000" }, "00:01:000 - 00:02:000")] // with null case.
    [TestCase(new[] { "[0,start]:1000", "[1,start]" }, "00:01:000 - 00:01:000")] // with null case.
    [TestCase(new[] { "[0,start]:1000" }, "00:01:000 - 00:01:000")]
    [TestCase(new[] { "[0,start]" }, "--:--:--- - --:--:---")] // with null case.
    [TestCase(new string[] { }, "--:--:--- - --:--:---")]
    public void TestTimeTagTimeFormattedString(string[] timeTags, string expected)
    {
        var lyric = new Lyric
        {
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        string actual = LyricUtils.TimeTagTimeFormattedString(lyric);
        Assert.That(expected, Is.EqualTo(actual));
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
            SingerIds = TestCaseTagHelper.ParseSingers(existSingers).Select(x => x.ID).ToArray(),
        };

        bool actual = LyricUtils.ContainsSinger(lyric, singer);
        Assert.That(expected, Is.EqualTo(actual));
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
            SingerIds = TestCaseTagHelper.ParseSingers(existSingers).Select(x => x.ID).ToArray(),
        };

        bool actual = LyricUtils.OnlyContainsSingers(lyric, singers);
        Assert.That(expected, Is.EqualTo(actual));
    }

    #endregion
}
