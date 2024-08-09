// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Integration.Formats;

public class LyricTextDecoderTest
{
    [TestCase("karaoke", new[] { "[0,0]:karaoke" })] // only one lyric.
    [TestCase("か\nら\nお\nけ", new[] { "[0,0]:か", "[0,0]:ら", "[0,0]:お", "[0,0]:け" })] // multi lyric.
    public void TestDecodeBeatmapToPureText(string expected, string[] lyrics)
    {
        var decoder = new LyricTextDecoder();
        var actual = decoder.Decode(expected);

        var expectedLyrics = TestCaseTagHelper.ParseLyrics(lyrics);
        Assert.AreEqual(expectedLyrics.Length, actual.Length);

        for (int i = 0; i < expectedLyrics.Length; i++)
        {
            Assert.AreEqual(expectedLyrics[i].Text, actual[i].Text);
        }
    }
}
