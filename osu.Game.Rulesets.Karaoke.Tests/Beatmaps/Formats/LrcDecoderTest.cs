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
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class LrcDecoderTest
    {
        [Test]
        public void TestDecodeLyric()
        {
            const string lyric_text = "[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]";
            var beatmap = decodeLrcLine(lyric_text);

            // Get first beatmap
            var lyric = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault();

            // Check lyric
            Assert.AreEqual(lyric?.Text, "からおけ");
            Assert.AreEqual(lyric?.StartTime, 1000);
            Assert.AreEqual(lyric?.EndTime, 5000);

            // Check time tag
            var tags = TimeTagsUtils.ToDictionary(lyric?.TimeTags);
            var checkedTags = tags.ToArray();
            Assert.AreEqual(tags.Count, 5);
            Assert.AreEqual(checkedTags.Length, 5);
            Assert.AreEqual(string.Join(',', tags.Select(x => x.Key.Index)), "0,1,2,3,4");
            Assert.AreEqual(string.Join(',', tags.Select(x => x.Value)), "1000,2000,3000,4000,5000");
        }

        [Test]
        public void TestDecodeLyricWithDuplicatedTimeTag()
        {
            const string wrong_lyric_text = "[00:04.00]か[00:04.00]ら[00:05.00]お[00:06.00]け[00:07.00]";
            Assert.Throws<FormatException>(() => decodeLrcLine(wrong_lyric_text));
        }

        [Test]
        [Ignore("Waiting for lyric parser update.")]
        public void TestDecodeLyricWithTimeTagNotOrder()
        {
            const string wrong_lyric_text = "[00:04.00]か[00:03.00]ら[00:02.00]お[00:01.00]け[00:00.00]";
            Assert.Throws<FormatException>(() => decodeLrcLine(wrong_lyric_text));
        }

        private Beatmap decodeLrcLine(string line)
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
