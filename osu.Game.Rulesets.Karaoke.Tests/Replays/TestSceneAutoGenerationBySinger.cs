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

            Assert.IsTrue(karoakeFrames.Take(7).All(x => x.Scale > 6.8 && x.Scale < 7.9), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(7).Take(7).All(x => x.Scale > 9.1 && x.Scale < 10.6), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(14).Take(6).All(x => x.Scale > 11.8 && x.Scale < 12.3), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(20).Take(7).All(x => x.Scale > 13.1 && x.Scale < 13.72), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(27).Take(6).All(x => x.Scale > 16.14 && x.Scale < 16.72), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(33).Take(1).All(x => x.Scale > 17.42 && x.Scale < 17.43), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(34).Take(6).All(x => x.Scale > 19.57 && x.Scale < 20.15), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(40).Take(7).All(x => x.Scale >= 22 && x.Scale < 24.15), "Incorrect scale range");
            Assert.IsTrue(karoakeFrames.Skip(47).Take(7).All(x => x.Scale > 25.1 && x.Scale < 26.8), "Incorrect scale range");
        }
    }
}
