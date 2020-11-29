// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class LrcEncoderTest
    {
        private static IEnumerable<string> allLrcFileNames => TestResources.GetStore().GetAvailableResources()
                                                                           .Where(res => res.EndsWith(".lrc")).Select(Path.GetFileNameWithoutExtension);

        [TestCaseSource(nameof(allLrcFileNames))]
        public void TestDecodeEncodedBeatmap(string fileName)
        {
            var decoded = decode(fileName, out var encoded);

            // Note : this test case does not cover ruby and romaji properties
            Assert.That(decoded.HitObjects.Count, Is.EqualTo(encoded.HitObjects.Count));
            Assert.That(encoded.Serialize(), Is.EqualTo(decoded.Serialize()));
        }

        private Beatmap decode(string filename, out Beatmap encoded)
        {
            using (var stream = TestResources.OpenLrcResource(filename))
            using (var sr = new LineBufferedReader(stream))
            {
                // Read file and decode to file
                var legacyDecoded = new LrcDecoder().Decode(sr);

                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms))
                using (var sr2 = new LineBufferedReader(ms))
                {
                    // Then encode file to stream
                    var encodeResult = new LrcEncoder().Encode(legacyDecoded);
                    sw.WriteLine(encodeResult);
                    sw.Flush();

                    ms.Position = 0;

                    encoded = new LrcDecoder().Decode(sr2);
                    return legacyDecoded;
                }
            }
        }
    }
}
