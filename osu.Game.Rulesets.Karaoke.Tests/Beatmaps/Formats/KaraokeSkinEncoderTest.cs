// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class KaraokeSkinEncoderTest
    {
        private static IEnumerable<string> allSkins => TestResources.GetStore().GetAvailableResources()
                                                                    .Where(res => res.EndsWith(".skin")).Select(Path.GetFileNameWithoutExtension);

        [TestCaseSource(nameof(allSkins))]
        public void TestDecodeEncodedSkin(string name)
        {
            // Get first decode and second decode result.
            var decoded = decode(name, out var encoded);

            // Check each property's number.
            Assert.That(decoded.Fonts.Count, Is.EqualTo(encoded.Fonts.Count));
            Assert.That(decoded.Layouts.Count, Is.EqualTo(encoded.Layouts.Count));
            Assert.That(decoded.NoteSkins.Count, Is.EqualTo(encoded.NoteSkins.Count));

            // Check each property's value.
            Assert.That(encoded.Serialize(), Is.EqualTo(decoded.Serialize()));
        }

        private KaraokeSkin decode(string filename, out KaraokeSkin encoded)
        {
            using (var stream = TestResources.OpenSkinResource(filename))
            using (var sr = new LineBufferedReader(stream))
            {
                // Read file and decode to file
                var legacyDecoded = new KaraokeSkinDecoder().Decode(sr);

                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms))
                using (var sr2 = new LineBufferedReader(ms))
                {
                    // Then encode file to stream
                    var encodeResult = new KaraokeSkinEncoder().Encode(legacyDecoded);
                    sw.WriteLine(encodeResult);
                    sw.Flush();

                    ms.Position = 0;

                    // Decode result from stream
                    encoded = new KaraokeSkinDecoder().Decode(sr2);
                    return legacyDecoded;
                }
            }
        }
    }
}
