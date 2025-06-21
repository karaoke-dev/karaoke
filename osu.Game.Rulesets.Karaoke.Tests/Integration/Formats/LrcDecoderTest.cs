// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Integration.Formats;

public class LrcDecoderTest
{
    [TestCase("からおけ", new string[] { }, "[00:00.00] からおけ")] // todo: handle the start time.
    [TestCase("からおけ", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, "[00:00.00] <00:01.00>か<00:02.00>ら<00:03.00>お<00:04.00>け<00:05.00>")]
    public void TestLyricWithTimeTag(string lyricText, string[] timeTags, string expected)
    {
        var lyric = new Lyric
        {
            Text = lyricText,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        string actual = new LrcEncoder().Encode(new Beatmap
        {
            HitObjects = new List<HitObject>
            {
                lyric,
            },
        });

        Assert.That(actual, Is.EqualTo(expected));
    }
}
