// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Rulesets.Mods;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class KaraokeLegacyBeatmapDecoderTest
    {
        public KaraokeLegacyBeatmapDecoderTest()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();
        }

        [Test]
        public void TestDecodeBeatmapLyric()
        {
            using (var resStream = TestResources.OpenBeatmapResource("karaoke-file-samples"))
            using (var stream = new LineBufferedReader(resStream))
            {
                var decoder = Decoder.GetDecoder<Beatmap>(stream);
                Assert.AreEqual(typeof(KaraokeLegacyBeatmapDecoder), decoder.GetType());

                var working = new TestWorkingBeatmap(decoder.Decode(stream));

                Assert.AreEqual(1, working.BeatmapInfo.BeatmapVersion);
                Assert.AreEqual(1, working.Beatmap.BeatmapInfo.BeatmapVersion);
                Assert.AreEqual(1, working.GetPlayableBeatmap(new KaraokeRuleset().RulesetInfo, Array.Empty<Mod>()).BeatmapInfo.BeatmapVersion);

                // Test lyric part decode result
                var lyrics = working.Beatmap.HitObjects.OfType<LyricLine>();
                Assert.AreEqual(54, lyrics.Count());

                // Test note decode part
                var notes = working.Beatmap.HitObjects.OfType<Note>().Where(x => x.Display).ToList();
                Assert.AreEqual(36, notes.Count);

                testNote("た", 0, note: notes[0]);
                testNote("だ", 0, note: notes[1]);
                testNote("か", 0, note: notes[2]); // 風,か
                testNote("ぜ", 0, note: notes[3]); // 風,ぜ
                testNote("に", 1, note: notes[4]);
                testNote("揺", 2, note: notes[5]);
                testNote("ら", 3, note: notes[6]);
                testNote("れ", 4, true, notes[7]);
                testNote("て", 3, note: notes[8]);
            }
        }

        [Test]
        public void TestDecodeNote()
        {
            using (var resStream = TestResources.OpenBeatmapResource("karaoke-note-samples"))
            using (var stream = new LineBufferedReader(resStream))
            {
                // Create karaoke beatmap decoder
                var lrcDecoder = new KaraokeLegacyBeatmapDecoder();
                var beatmap = lrcDecoder.Decode(stream);

                // Get notes
                var notes = beatmap.HitObjects.OfType<Note>().ToList();

                testNote("か", 1, note: notes[0]);
                testNote("ら", 2, true, notes[1]);
                testNote("お", 3, note: notes[2]);
                testNote("け", 3, true, notes[3]);
                testNote("け", 4, note: notes[4]);
            }
        }

        private void testNote(string text, int tone, bool half = false, Note note = null)
        {
            Assert.AreEqual(text, note?.Text);
            Assert.AreEqual(tone, note?.Tone.Scale);
        }

        [Test]
        public void TestDecodeStyle()
        {
            using (var resStream = TestResources.OpenBeatmapResource("karaoke-style-samples"))
            using (var stream = new LineBufferedReader(resStream))
            {
                // Create karaoke beatmap decoder
                var lrcDecoder = new KaraokeLegacyBeatmapDecoder();
                var beatmap = lrcDecoder.Decode(stream);

                // Get lyric
                var lyric = beatmap.HitObjects.OfType<LyricLine>().FirstOrDefault();

                // Check is not null
                Assert.IsTrue(lyric != null);

                // Check layout and font index
                Assert.AreEqual(lyric.LayoutIndex, 2);
                Assert.AreEqual(lyric.FontIndex, 3);
            }
        }

        [Test]
        public void TestDecodeTranslate()
        {
            using (var resStream = TestResources.OpenBeatmapResource("karaoke-translate-samples"))
            using (var stream = new LineBufferedReader(resStream))
            {
                // Create karaoke beatmap decoder
                var lrcDecoder = new KaraokeLegacyBeatmapDecoder();
                var beatmap = lrcDecoder.Decode(stream);

                // Get translate
                var translates = beatmap.GetTranslates();

                // Check is not null
                Assert.IsTrue(translates != null);

                // Check translate count
                Assert.AreEqual(translates.Count, 2);

                // Check chinese translate
                Assert.AreEqual(translates["zh-TW"].Count, 2);
                Assert.AreEqual(translates["zh-TW"][0], "卡拉OK");
                Assert.AreEqual(translates["zh-TW"][1], "喜歡");

                // Check english translate
                Assert.AreEqual(translates["en-US"].Count, 2);
                Assert.AreEqual(translates["en-US"][0], "karaoke");
                Assert.AreEqual(translates["en-US"][1], "like it");
            }
        }
    }
}
