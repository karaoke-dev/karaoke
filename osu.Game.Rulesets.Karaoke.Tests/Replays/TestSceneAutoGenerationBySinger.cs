// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Tests.Visual;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Replays
{
    public class TestSceneAutoGenerationBySinger : OsuTestScene
    {
        [Test]
        public void TestSingDemoSong()
        {
            var data = TestResources.OpenTrackResource("demo");
            var generated = new KaraokeAutoGeneratorBySinger(null, data).Generate();

            // test total frames
            Assert.IsTrue(generated.Frames.Count == 55, "Replay frame should have 55.");
            Assert.AreEqual(222, (int)generated.Frames[0].Time, "Incorrect time");
            Assert.AreEqual(4234, (int)generated.Frames[54].Time, "Incorrect time");

            // test saitenable frames
            var karoakeFrames = generated.Frames.OfType<KaraokeReplayFrame>();
            Assert.AreEqual(54, karoakeFrames.Where(x => x.Sound).Count(), "Incorrect time");
        }
    }
}
