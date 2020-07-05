// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Tests.Visual;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Replays
{
    public class TestSceneAutoGenerationBySinger : OsuTestScene
    {
        [Test]
        public void TestSingDemoSong()
        {
            var beatmap = new KaraokeBeatmap();

            var data = TestResources.OpenTrackResource("demo");
            var generated = new KaraokeAutoGeneratorBySinger(beatmap, data).Generate();

            // Get generated frame and compare frame
            var karaokeFrames = generated.Frames.OfType<KaraokeReplayFrame>().ToList();
            var compareFrame = getCompareResultFromName("demo");

            // Check total frames.
            Assert.AreEqual(karaokeFrames.Count, compareFrame.Count, $"Replay frame should have {compareFrame.Count}.");

            // Compare generated frame with result;
            for (int i = 0; i < compareFrame.Count; i++)
            {
                Assert.AreEqual(karaokeFrames[i].Time, compareFrame[i].Time);
                Assert.AreEqual(karaokeFrames[i].Sound, compareFrame[i].Sound);

                if (!compareFrame[i].Sound)
                    continue;

                var convertedScale = beatmap.PitchToScale(compareFrame[i].Pitch);
                Assert.AreEqual(karaokeFrames[i].Scale, convertedScale);
            }
        }

        private static IList<TestKaraokeReplayFrame> getCompareResultFromName(string name)
        {
            var data = TestResources.OpenResource($"Testing/Track/{name}.json");

            using (var reader = new StreamReader(data))
            {
                string str = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<TestKaraokeReplayFrame>>(str);
            }
        }

        private class TestKaraokeReplayFrame
        {
            public double Time { get; set; }

            public float Pitch { get; set; }

            public bool Sound { get; set; }
        }
    }
}
