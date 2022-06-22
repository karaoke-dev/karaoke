// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class LyricTextEncoderTest
    {
        [TestCase(new[] { "[0,0]:karaoke" }, "karaoke")] // only one lyric.
        [TestCase(new[] { "[0,0]:か", "[0,0]:ら", "[0,0]:お", "[0,0]:け" }, "か\nら\nお\nけ")] // multi lyric.
        public void TestEncodeBeatmapToPureText(string[] lyrics, string expected)
        {
            var encoder = new LyricTextEncoder();
            var beatmap = new KaraokeBeatmap
            {
                HitObjects = TestCaseTagHelper.ParseLyrics(lyrics).OfType<KaraokeHitObject>().ToList()
            };

            string actual = encoder.Encode(beatmap);
            Assert.AreEqual(expected, actual);
        }
    }
}
