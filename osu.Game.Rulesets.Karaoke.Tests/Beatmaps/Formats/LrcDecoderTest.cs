// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class LrcDecoderTest
    {
        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", "からおけ", 1000, 5000)]
        public void TestLyricTextAndTime(string lyricText, string expectedText, double expectedStartTime, double expectedEndTime)
        {
            var beatmap = decodeLrcLine(lyricText);

            // Get first lyric from beatmap
            var actual = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault();
            Assert.AreEqual(expectedText, actual?.Text);
            Assert.AreEqual(expectedStartTime, actual?.StartTime);
            Assert.AreEqual(expectedEndTime, actual?.EndTime);
        }

        [TestCase("[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        public void TestLyricTimeTag(string text, string[] timeTags)
        {
            // Get first lyric from beatmap
            var beatmap = decodeLrcLine(text);
            var lyric = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault();

            // Check time tag
            var expected = TestCaseTagHelper.ParseTimeTags(timeTags);
            var actual = lyric?.TimeTags;
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase("[00:04.00]か[00:04.00]ら[00:05.00]お[00:06.00]け[00:07.00]")]
        public void TestDecodeLyricWithDuplicatedTimeTag(string text)
        {
            Assert.Throws<FormatException>(() => decodeLrcLine(text));
        }

        [Ignore("Waiting for lyric parser update.")]
        [TestCase("[00:04.00]か[00:03.00]ら[00:02.00]お[00:01.00]け[00:00.00]")]
        public void TestDecodeLyricWithTimeTagNotOrder(string text)
        {
            Assert.Throws<FormatException>(() => decodeLrcLine(text));
        }

        private static Beatmap decodeLrcLine(string line)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new LineBufferedReader(stream))
            {
                // Create stream
                writer.Write(line);
                writer.Flush();
                stream.Position = 0;

                // Create karaoke note decoder
                var decoder = new LrcDecoder();
                return decoder.Decode(reader);
            }
        }
    }
}
