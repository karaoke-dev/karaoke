// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Rulesets.Mods;
using osu.Game.Tests.Beatmaps;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class KaraokeBeatmapDecoderTest
    {
        public KaraokeBeatmapDecoderTest()
        {
            // It's a tricky to let lazer to read karaoke testing beatmap
            KaroakeLegacyBeatmapDecoder.Register();
        }

        [Test]
        public void TestDecodeBeatmapLyric()
        {
            using (var resStream = TestResources.OpenBeatmapResource("karaoke-file-samples"))
            using (var stream = new LineBufferedReader(resStream))
            {
                var decoder = Decoder.GetDecoder<Beatmap>(stream);
                Assert.AreEqual(typeof(KaroakeLegacyBeatmapDecoder), decoder.GetType());

                var working = new TestWorkingBeatmap(decoder.Decode(stream));

                Assert.AreEqual(1, working.BeatmapInfo.BeatmapVersion);
                Assert.AreEqual(1, working.Beatmap.BeatmapInfo.BeatmapVersion);
                Assert.AreEqual(1, working.GetPlayableBeatmap(new KaraokeRuleset().RulesetInfo, Array.Empty<Mod>()).BeatmapInfo.BeatmapVersion);

                var lyrics = working.Beatmap.HitObjects.OfType<LyricLine>();
                Assert.AreEqual(54, lyrics.Count());


                var notes = working.Beatmap.HitObjects.OfType<KaraokeNote>().Where(x => x.Display).ToList();
                Assert.AreEqual(36, notes.Count());

                TestKaroakeNote("た", 1, note: notes[0]);
                TestKaroakeNote("だ", 1, note: notes[1]);
                TestKaroakeNote("風", 2, note: notes[2]);//か
                TestKaroakeNote("風", 2, note: notes[3]);//ぜ
                TestKaroakeNote("に", 3, note: notes[4]);
                TestKaroakeNote("揺", 4, note: notes[5]);
                TestKaroakeNote("ら", 5, note: notes[6]);
                TestKaroakeNote("れ", 5, true ,note: notes[7]);
                TestKaroakeNote("て", 5, note: notes[8]);
            }
        }

        private void TestKaroakeNote(string text, int tone,bool half = false, KaraokeNote note = null)
        {
            Assert.AreEqual(text, note.Text);
            Assert.AreEqual(tone, note.Tone);
        }
    }
}
