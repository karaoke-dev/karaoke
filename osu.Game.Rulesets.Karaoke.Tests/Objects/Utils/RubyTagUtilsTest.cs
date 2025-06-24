// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Utils;

[TestFixture]
public class RubyTagUtilsTest
{
    [TestCase("[0,1]:ka", "karaoke", "[0,1]:ka")]
    [TestCase("[0,1]:", "karaoke", "[0,1]:")]
    [TestCase("[0,0]:ka", "karaoke", "[0,0]:ka")] // ignore at same index
    [TestCase("[-1,1]:ka", "karaoke", "[0,1]:ka")]
    [TestCase("[3,1]:ka", "karaoke", "[1,3]:ka")]
    [TestCase("[3,-1]:ka", "karaoke", "[0,3]:ka")]
    public void TestGetFixedIndex(string rubyTagStr, string lyric, string actualTag)
    {
        // test ruby tag.
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        var expectedRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
        var actualRubyTag = generateFixedTag(rubyTag, lyric);
        RubyTagAssert.ArePropertyEqual(expectedRubyTag, actualRubyTag);

        static RubyTag generateFixedTag(RubyTag rubyTag, string lyric)
        {
            (int startIndex, int endIndex) = RubyTagUtils.GetFixedIndex(rubyTag, lyric);
            return new RubyTag
            {
                Text = rubyTag.Text,
                StartIndex = startIndex,
                EndIndex = endIndex,
            };
        }
    }

    [TestCase("[0]:ka", "karaoke", 1, "[1]:ka")]
    [TestCase("[0]:", "karaoke", 1, "[1]:")]
    [TestCase("[0]:ka", "karaoke", -1, "[0]:ka")]
    [TestCase("[0]:ka", "", -1, null)] // should not be able to adjust the time-tag if lyric is empty.
    [TestCase("[0]:ka", "", 1, null)] // should not be able to adjust the time-tag if lyric is empty.
    [TestCase("[1,0]:ka", "karaoke", 0, "[0,1]:ka")] // will auto fix the position
    [TestCase("[1,0]:ka", "karaoke", 1, "[1,2]:ka")]
    public void TestGetShiftingIndex(string rubyTagStr, string lyric, int offset, string? actualTag)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        if (actualTag == null)
        {
            Assert.That(() => generateShiftingTag(rubyTag, lyric, offset), Throws.TypeOf<InvalidOperationException>());
            Assert.That(() => generateShiftingTag(rubyTag, lyric, offset), Throws.TypeOf<InvalidOperationException>());
            return;
        }

        // test ruby tag.
        var expectedRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
        var actualRubyTag = generateShiftingTag(rubyTag, lyric, offset);
        RubyTagAssert.ArePropertyEqual(expectedRubyTag, actualRubyTag);

        static RubyTag generateShiftingTag(RubyTag rubyTag, string lyric, int offset)
        {
            (int startIndex, int endIndex) = RubyTagUtils.GetShiftingIndex(rubyTag, lyric, offset);
            return new RubyTag
            {
                Text = rubyTag.Text,
                StartIndex = startIndex,
                EndIndex = endIndex,
            };
        }
    }

    [TestCase("[0,1]:ka", "karaoke", false)]
    [TestCase("[0,1]:ka", "", true)]
    [TestCase("[0,-1]:ka", "karaoke", true)]
    [TestCase("[1,0]:ka", "karaoke", false)] // should not be counted as out of range if index is not ordered.
    [TestCase("[0,0]:ka", "", true)] // should be counted as out of range if lyric is empty
    public void TestOutOfRange(string rubyTagStr, string lyric, bool expected)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        bool actual = RubyTagUtils.OutOfRange(rubyTag, lyric);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase("[0,1]:ka", 0, true)]
    [TestCase("[0,1]:ka", 1, true)]
    [TestCase("[0,1]:ka", -1, true)] // should be ok with negative value because we only check if valid with current text-tag index.
    [TestCase("[2,1]:ka", 0, true)]
    [TestCase("[2,1]:ka", 1, true)]
    [TestCase("[2,1]:ka", 2, false)]
    public void TestValidNewStartIndex(string rubyTagStr, int newStartIndex, bool expected)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        bool actual = RubyTagUtils.ValidNewStartIndex(rubyTag, newStartIndex);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase("[0,1]:ka", 1, true)]
    [TestCase("[0,1]:ka", 0, true)]
    [TestCase("[0,1]:ka", 1000, true)] // should be ok with large value because we only check if valid with current text-tag index.
    [TestCase("[2,1]:ka", 0, false)]
    [TestCase("[2,1]:ka", 1, false)]
    [TestCase("[2,1]:ka", 2, true)]
    [TestCase("[2,1]:ka", 3, true)]
    public void TestValidNewEndIndex(string rubyTagStr, int newEndIndex, bool expected)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        bool actual = RubyTagUtils.ValidNewEndIndex(rubyTag, newEndIndex);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase("karaoke", 0, false)]
    [TestCase("karaoke", 6, false)]
    [TestCase("karaoke", -1, true)]
    [TestCase("karaoke", 7, true)]
    [TestCase("", -1, true)]
    [TestCase("", 0, true)]
    [TestCase("", 1, true)]
    public void TestOutOfRange(string lyric, int index, bool expected)
    {
        bool actual = RubyTagUtils.OutOfRange(lyric, index);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase("[0,1]:ka", false)]
    [TestCase("[0,1]:", true)]
    public void TestEmptyText(string rubyTagStr, bool expected)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        bool actual = RubyTagUtils.EmptyText(rubyTag);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase("[0,1]:ka", "ka(0 ~ 1)")]
    [TestCase("[0,1]:", "empty(0 ~ 1)")]
    [TestCase("[-1,1]:ka", "ka(-1 ~ 1)")]
    [TestCase("[-1,-1]:ka", "ka(-1 ~ -1)")]
    [TestCase("[-1,-2]:ka", "ka(-1 ~ -2)")] // will not fix the order in display.
    [TestCase("[2,1]:ka", "ka(2 ~ 1)")] // will not fix the order in display.
    public void TestPositionFormattedString(string rubyTagStr, string expected)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        string actual = RubyTagUtils.PositionFormattedString(rubyTag);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase("[0]:ka", "カラオケ", "カ")]
    [TestCase("[0,3]:karaoke", "カラオケ", "カラオケ")]
    [TestCase("[-1,0]:", "カラオケ", "カ")] // will get the first char if out of the range.
    [TestCase("[4]:", "カラオケ", "ケ")] // will get the last char if out of the range.
    [TestCase("[3,0]:karaoke", "カラオケ", "カラオケ")] // should not have those state but still give it a value.
    [TestCase("[0,3]:karaoke", "", null)]
    public void TestGetTextFromLyric(string rubyTagStr, string lyric, string? expected)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);

        if (expected == null)
        {
            Assert.That(() => RubyTagUtils.GetTextFromLyric(rubyTag, lyric), Throws.TypeOf<InvalidOperationException>());
        }
        else
        {
            string actual = RubyTagUtils.GetTextFromLyric(rubyTag, lyric);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    [TestCase("[0,1]:ka")]
    [TestCase("[1,0]:ka")] // Should be able to convert even if time-tag is invalid.
    [TestCase("[-1,1]:ka")] // Should be able to convert even if time-tag is invalid.
    public void TestToPositionText(string rubyTagStr)
    {
        var rubyTag = TestCaseTagHelper.ParseRubyTag(rubyTagStr);
        var actual = RubyTagUtils.ToPositionText(rubyTag);

        Assert.That(actual.Text, Is.EqualTo(rubyTag.Text));
        Assert.That(actual.StartIndex, Is.EqualTo(rubyTag.StartIndex));
        Assert.That(actual.EndIndex, Is.EqualTo(rubyTag.EndIndex));
    }
}
