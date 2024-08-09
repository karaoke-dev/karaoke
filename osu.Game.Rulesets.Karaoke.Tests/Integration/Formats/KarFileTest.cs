// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Integration.Formats;

public class KarFileTest
{
    private static IEnumerable<string> allKarFileNames => TestResources.GetStore().GetAvailableResources()
                                                                       .Where(res => res.EndsWith(".kar", StringComparison.Ordinal)).Select(x => Path.GetFileNameWithoutExtension(x!));

    [TestCaseSource(nameof(allKarFileNames))]
    public void TestDecodeEncodedBeatmap(string fileName)
    {
        var decoded = decode(fileName, out var encoded);

        // Note : this test case does not cover ruby property
        Assert.That(decoded.HitObjects.Count, Is.EqualTo(encoded.HitObjects.Count));
        Assert.That(encoded.Serialize(), Is.EqualTo(decoded.Serialize()));
    }

    private static Beatmap decode(string filename, out Beatmap encoded)
    {
        using var stream = TestResources.OpenKarResource(filename);
        using var sr = new LineBufferedReader(stream);

        // Read file and decode to file
        var legacyDecoded = new Beatmap
        {
            HitObjects = new KarDecoder().Decode(sr.ReadToEnd()).OfType<HitObject>().ToList(),
        };

        using var ms = new MemoryStream();
        using var sw = new StreamWriter(ms);
        using var sr2 = new LineBufferedReader(ms);

        // Then encode file to stream
        string encodeResult = new KarEncoder().Encode(legacyDecoded);
        sw.WriteLine(encodeResult);
        sw.Flush();

        ms.Position = 0;

        encoded = new Beatmap
        {
            HitObjects = new KarDecoder().Decode(sr2.ReadToEnd()).OfType<HitObject>().ToList(),
        };
        return legacyDecoded;
    }
}
