// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Audio;
using osu.Framework.Threading;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Replays
{
    public class TestSceneAutoGenerationBySinger : OsuTestScene
    {
        private AudioManager manager;

        [SetUp]
        public void SetUp()
        {
            var thread = new AudioThread();
            manager = new AudioManager(thread, null, null);
            thread.Start();
        }

        [Test]
        public void TestSingDemoSong()
        {
            // need to check track's length
            var track = TestResources.OpenTrackInfo(manager, "demo");
            //Assert.AreEqual((int)track.Length, 4365);

            var data = TestResources.OpenTrackResource("demo");
            var generated = new KaraokeAutoGeneratorBySinger(null, track, data).Generate();

            // todo : run some test case.
            
            Assert.IsTrue(generated.Frames.Count == 55, "Replay frame should have 55.");
        }
    }
}
