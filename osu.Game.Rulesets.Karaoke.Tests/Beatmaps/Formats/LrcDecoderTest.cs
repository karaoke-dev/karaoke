// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using System.IO;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class LrcDecoderTest
    {
        [Test]
        public void TestDecodeNote()
        {
            const string lyric_text = "[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]";

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new LineBufferedReader(stream))
            {
                // Create stream
                writer.Write(lyric_text);
                writer.Flush();
                stream.Position = 0;

                // Create karaoke note decoder
                var decoder = new LrcDecoder();
                var beatmap = decoder.Decode(reader);

                // Get first beatmap
                var lyric = beatmap.HitObjects.OfType<LyricLine>().FirstOrDefault();

                // lyric
                Assert.AreEqual(lyric?.Text, "からおけ");
                Assert.AreEqual(lyric?.StartTime, 1000);
                Assert.AreEqual(lyric?.EndTime, 5000);

                // time tag
                var tags = lyric?.TimeTags;
                var checkedTags = tags.ToArray();
                Assert.AreEqual(tags.Count, 5);
                Assert.AreEqual(checkedTags.Length, 5);
                Assert.AreEqual(string.Join(',', tags.Select(x => x.Key.Index)), "0,1,2,3,4");
                Assert.AreEqual(string.Join(',', tags.Select(x => x.Value)), "1000,2000,3000,4000,5000");

                // TODO : move to encode test case
                var encoder = new LrcEncoder();
                var result = encoder.Encode(beatmap).Replace("\r\n", "");

                Assert.AreEqual(lyric_text, result);
            }
        }
    }
}
