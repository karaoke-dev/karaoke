// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Integration.Formats;

public class LrcEncoderTest
{
    [TestCase("[00:01.00]<00:01.00>か<00:02.00>ら<00:03.00>お<00:04.00>け<00:05.00>", "からおけ", 1000, 5000)]
    public void TestLyricTextAndTime(string lyricText, string expectedText, double expectedStartTime, double expectedEndTime)
    {
        // Get first lyric from beatmap
        var lyrics = new LrcDecoder().Decode(lyricText);
        var actual = lyrics.FirstOrDefault()!;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Text, Is.EqualTo(expectedText));
        Assert.That(actual.StartTime, Is.EqualTo(expectedStartTime));
        Assert.That(actual.EndTime, Is.EqualTo(expectedEndTime));
    }

    [TestCase("[00:01.00]<00:01.00>か<00:02.00>ら<00:03.00>お<00:04.00>け<00:05.00>", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
    public void TestLyricTimeTag(string text, string[] timeTags)
    {
        // Get first lyric from beatmap
        var lyrics = new LrcDecoder().Decode(text);
        var lyric = lyrics.First();

        // Check time tag
        var expected = TestCaseTagHelper.ParseTimeTags(timeTags);
        var actual = lyric.TimeTags;
        TimeTagAssert.ArePropertyEqual(expected, actual);
    }

    [Ignore("Time-tags with same time might be allowed.")]
    [TestCase("[00:04.00]<00:04.00>か<00:04.00>ら<00:05.00>お<00:06.00>け<00:07.00>")]
    public void TestDecodeLyricWithDuplicatedTimeTag(string text)
    {
        Assert.Throws<FormatException>(() => new LrcDecoder().Decode(text));
    }

    [Ignore("Waiting for lyric parser update.")]
    [TestCase("[00:04.00]<00:04.00>か<00:03.00>ら<00:02.00>お<00:01.00>け<00:00.00>")]
    public void TestDecodeLyricWithTimeTagNotOrder(string text)
    {
        Assert.Throws<FormatException>(() => new LrcDecoder().Decode(text));
    }
}
