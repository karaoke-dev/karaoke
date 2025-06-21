// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Replays;

public partial class TestSceneAutoGenerationBySinger : OsuTestScene
{
    [Test]
    public void TestSingDemoSong()
    {
        var beatmap = new KaraokeBeatmap();

        var data = TestResources.OpenTrackResource("demo");
        var generated = new KaraokeAutoGeneratorBySinger(beatmap, data).Generate();

        // Get generated frame and compare frame
        var expected = getCompareResultFromName("demo");
        var actual = generated.Frames.OfType<KaraokeReplayFrame>().ToList();

        // Check total frames.
        Assert.That(actual.Count, Is.EqualTo(expected.Count), $"Replay frame should have {expected.Count}.");

        // Compare generated frame with result;
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.That(actual[i].Time, Is.EqualTo(expected[i].Time));
            Assert.That(actual[i].Sound, Is.EqualTo(expected[i].Sound));

            if (!expected[i].Sound)
                continue;

            float convertedScale = beatmap.PitchToScale(expected[i].Pitch);
            Assert.That(actual[i].Scale, Is.EqualTo(convertedScale));
        }
    }

    private static IReadOnlyList<TestKaraokeReplayFrame> getCompareResultFromName(string name)
    {
        var data = TestResources.OpenResource($"Testing/Track/{name}.json");

        using var reader = new StreamReader(data);
        string str = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<TestKaraokeReplayFrame[]>(str) ?? Array.Empty<TestKaraokeReplayFrame>();
    }

    private struct TestKaraokeReplayFrame
    {
        [JsonProperty]
        public double Time { get; set; }

        [JsonProperty]
        public float Pitch { get; set; }

        [JsonProperty]
        public bool Sound { get; set; }
    }
}
