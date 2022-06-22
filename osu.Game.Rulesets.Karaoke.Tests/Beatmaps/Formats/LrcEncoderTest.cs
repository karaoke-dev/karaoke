// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class LrcEncoderTest
    {
        private static IEnumerable<string> allLrcFileNames => TestResources.GetStore().GetAvailableResources()
                                                                           .Where(res => res.EndsWith(".lrc", StringComparison.Ordinal)).Select(Path.GetFileNameWithoutExtension);

        [TestCaseSource(nameof(allLrcFileNames))]
        public void TestDecodeEncodedBeatmap(string fileName)
        {
            var decoded = decode(fileName, out var encoded);

            // Note : this test case does not cover ruby and romaji property
            Assert.That(decoded.HitObjects.Count, Is.EqualTo(encoded.HitObjects.Count));
            Assert.That(encoded.Serialize(), Is.EqualTo(decoded.Serialize()));
        }

        private static Beatmap decode(string filename, out Beatmap encoded)
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
                    string encodeResult = new LrcEncoder().Encode(legacyDecoded);
                    sw.WriteLine(encodeResult);
                    sw.Flush();

                    ms.Position = 0;

                    encoded = new LrcDecoder().Decode(sr2);
                    return legacyDecoded;
                }
            }
        }

        [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(new[] { "[0,start]:", "[0,start]:", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1100, 2000 })]
        [TestCase(new[] { "[0,start]:1000", "[0,start]:1100", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1000, 2000 })]
        [TestCase(new[] { "[0,start]:", "[0,end]:", "[0,start]:", "[1,start]:", "[1,end]:" }, new double[] { })]
        [TestCase(new[] { "[0,start]:2000", "[0,end]:1000" }, new double[] { 2000, 2000 })]
        [TestCase(new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2000", "[1,end]:3000" }, new double[] { 1100, 2100, 2100, 3000 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:2000", "[1,end]:3000" }, new double[] { 1000, 5000, 5000, 5000 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:0", "[1,end]:3000" }, new double[] { 1000, 2000, 2000, 3000 })]
        //[TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, new double[] { 4000, 4000, 4000, 4000 })]
        public void TestToDictionary(string[] timeTagTexts, double[] expected)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            double[] actual = LrcEncoder.ToDictionary(timeTags).Values.ToArray();
            Assert.AreEqual(expected, actual);
        }
    }
}
