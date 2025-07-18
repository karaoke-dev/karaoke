﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Utils;

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
            Assert.That(firstLyric.Text, Is.EqualTo(expectedFirstText));
            Assert.That(secondLyric.Text, Is.EqualTo(expectedSecondText));
        }
        else
        {
            Assert.Catch(() => LyricsUtils.SplitLyric(lyric, splitIndex));
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
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, splitIndex);

        TimeTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseTimeTags(firstTimeTags), firstLyric.TimeTags);
        TimeTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseTimeTags(secondTimeTags), secondLyric.TimeTags);
    }

    [TestCase("カラオケ", new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, 2,
        new[] { "[0]:か", "[1]:ら" }, new[] { "[0]:お", "[1]:け" })]
    [TestCase("カラオケ", new[] { "[0,2]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
    [TestCase("カラオケ", new[] { "[1,3]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
    [TestCase("カラオケ", new[] { "[1,2]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
    [TestCase("カラオケ", new[] { "[0,3]:からおけ" }, 2, new string[] { }, new string[] { })] // tag won't be assign to lyric if not fully in the range of the text.
    [TestCase("カラオケ", new string[] { }, 2, new string[] { }, new string[] { })]
    public void TestSeparateLyricRubyTag(string text, string[] rubyTags, int splitIndex, string[] firstRubyTags, string[] secondRubyTags)
    {
        var lyric = new Lyric
        {
            Text = text,
            RubyTags = TestCaseTagHelper.ParseRubyTags(rubyTags),
        };

        var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, splitIndex);

        RubyTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseRubyTags(firstRubyTags), firstLyric.RubyTags);
        RubyTagAssert.ArePropertyEqual(TestCaseTagHelper.ParseRubyTags(secondRubyTags), secondLyric.RubyTags);
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
            SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(singerIndexes),
        };

        var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, split_index);
        var expectedFirstSingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(expectedFirstSingerIndexes);
        var expectedSecondSingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(expectedSecondSingerIndexes);

        Assert.That(firstLyric.SingerIds, Is.EqualTo(expectedFirstSingerIds));
        Assert.That(secondLyric.SingerIds, Is.EqualTo(expectedSecondSingerIds));

        // also should check is not same object as origin lyric for safety purpose.
        Assert.That(lyric.SingerIds, Is.Not.SameAs(firstLyric.SingerIds));
        Assert.That(lyric.SingerIds, Is.Not.SameAs(secondLyric.SingerIds));
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
            Language = cultureInfo,
        };

        var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, split_index);

        Assert.That(firstLyric.Language, Is.EqualTo(expectedFirstCultureInfo));
        Assert.That(secondLyric.Language, Is.EqualTo(expectedSecondCultureInfo));
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
        Assert.That(combineLyric.Text, Is.EqualTo(expected));
    }

    [TestCase(new[] { "[0,start]" }, new[] { "[0,start]" }, new[] { "[0,start]", "[7,start]" })]
    [TestCase(new[] { "[0,end]" }, new[] { "[0,end]" }, new[] { "[0,end]", "[7,end]" })]
    [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:1000" }, new[] { "[0,start]:1000", "[7,start]:1000" })] // deal with the case with time.
    [TestCase(new[] { "[0,start]:1000" }, new[] { "[0,start]:-1000" }, new[] { "[0,start]:1000", "[7,start]:-1000" })] // deal with the case with not invalid time tag time.
    [TestCase(new[] { "[-1,start]" }, new[] { "[-1,start]" }, new[] { "[-1,start]", "[6,start]" })] // deal with the case with not invalid time tag position.
    public void TestCombineLyricTimeTag(string[] firstTimeTags, string[] secondTimeTags, string[] expectTimeTags)
    {
        var lyric1 = new Lyric
        {
            Text = "karaoke",
            TimeTags = TestCaseTagHelper.ParseTimeTags(firstTimeTags),
        };
        var lyric2 = new Lyric
        {
            Text = "karaoke",
            TimeTags = TestCaseTagHelper.ParseTimeTags(secondTimeTags),
        };

        var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);
        var timeTags = combineLyric.TimeTags;

        for (int i = 0; i < timeTags.Count; i++)
        {
            var expected = TestCaseTagHelper.ParseTimeTag(expectTimeTags[i]);
            Assert.That(timeTags[i].Index, Is.EqualTo(expected.Index));
            Assert.That(timeTags[i].Time, Is.EqualTo(expected.Time));
        }
    }

    [TestCase(new[] { "[0]:ruby" }, new[] { "[0]:ルビ" }, new[] { "[0]:ruby", "[7]:ルビ" })]
    [TestCase(new[] { "[0]:" }, new[] { "[0]:" }, new[] { "[0]:", "[7]:" })]
    [TestCase(new[] { "[0,2]:" }, new[] { "[0,2]:" }, new[] { "[0,2]:", "[7,9]:" })]
    [TestCase(new[] { "[0,9]:" }, new[] { "[0,9]:" }, new[] { "[0,9]:", "[7,13]:" })] // will auto-fix ruby index.
    [TestCase(new[] { "[-10,-1]:" }, new[] { "[-10,-1]:" }, new[] { "[-10,-1]:", "[0,6]:" })] // will auto-fix ruby index.
    public void TestCombineLyricRubyTag(string[] firstRubyTags, string[] secondRubyTags, string[] expectedRubyTags)
    {
        var lyric1 = new Lyric
        {
            Text = "karaoke",
            RubyTags = TestCaseTagHelper.ParseRubyTags(firstRubyTags),
        };
        var lyric2 = new Lyric
        {
            Text = "karaoke",
            RubyTags = TestCaseTagHelper.ParseRubyTags(secondRubyTags),
        };

        var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);

        var expected = TestCaseTagHelper.ParseRubyTags(expectedRubyTags);
        var actual = combineLyric.RubyTags;
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { 1 }, new[] { 2 }, new[] { 1, 2 })]
    [TestCase(new[] { 1 }, new[] { 1 }, new[] { 1 })] // deal with duplicated case.
    [TestCase(new[] { 1 }, new[] { -2 }, new[] { 1, -2 })] // deal with id not right case.
    public void TestCombineLyricSinger(int[] firstSingerIndexes, int[] secondSingerIndexes, int[] combinedSingerIds)
    {
        var lyric1 = new Lyric { SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(firstSingerIndexes) };
        var lyric2 = new Lyric { SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(secondSingerIndexes) };

        var combineLyric = LyricsUtils.CombineLyric(lyric1, lyric2);

        var expected = TestCaseElementIdHelper.CreateElementIdsByNumbers(combinedSingerIds);
        var actual = combineLyric.SingerIds;
        Assert.That(expected, Is.EqualTo(actual));
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
        Assert.That(expected, Is.EqualTo(actual));
    }

    #endregion
}
