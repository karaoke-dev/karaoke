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
            // a trick to get osu! to register karaoke beatmaps
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
                var lyrics = working.Beatmap.HitObjects.OfType<Lyric>();
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
            // Karaoke beatmap
            var beatmap = decodeBeatmap("karaoke-note-samples");

            // Get notes
            var notes = beatmap.HitObjects.OfType<Note>().ToList();

            testNote("か", 1, note: notes[0]);
            testNote("ら", 2, true, notes[1]);
            testNote("お", 3, note: notes[2]);
            testNote("け", 3, true, notes[3]);
            testNote("け", 4, note: notes[4]);
        }

        [Test]
        public void TestDecodeStyle()
        {
            // Karaoke beatmap
            var beatmap = decodeBeatmap("karaoke-style-samples");

            // Get lyric
            var lyric = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault();

            // Check is not null
            Assert.IsTrue(lyric != null);

            // Check layout and font index
            Assert.AreEqual(lyric.LayoutIndex, 2);
            Assert.AreEqual(lyric.Singers, new[] { 1, 2 });
        }

        [Test]
        public void TestDecodeTranslate()
        {
            // Karaoke beatmap
            var beatmap = decodeBeatmap("karaoke-translate-samples");

            // Get translate
            var translates = beatmap.AvailableTranslates();
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();

            // Check is not null
            Assert.IsTrue(translates != null);

            // Check translate count
            Assert.AreEqual(translates.Length, 2);

            // All lyric should have two translates
            Assert.AreEqual(lyrics[0].Translates.Count, 2);
            Assert.AreEqual(lyrics[1].Translates.Count, 2);

            // Check chinese translate
            var chineseLanguageId = translates[0].Id;
            Assert.AreEqual(lyrics[0].Translates[chineseLanguageId], "卡拉OK");
            Assert.AreEqual(lyrics[1].Translates[chineseLanguageId], "喜歡");

            // Check english translate
            var englishLanguageId = translates[1].Id;
            Assert.AreEqual(lyrics[0].Translates[englishLanguageId], "karaoke");
            Assert.AreEqual(lyrics[1].Translates[englishLanguageId], "like it");
        }

        private KaraokeBeatmap decodeBeatmap(string fileName)
        {
            using (var resStream = TestResources.OpenBeatmapResource(fileName))
            using (var stream = new LineBufferedReader(resStream))
            {
                // Create karaoke beatmap decoder
                var lrcDecoder = new KaraokeLegacyBeatmapDecoder();

                // Create initial beatmap
                var beatmap = lrcDecoder.Decode(stream);

                // Convert to karaoke beatmap
                return new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
            }
        }

        private void testNote(string text, int tone, bool half = false, Note note = null)
        {
            Assert.AreEqual(text, note?.Text);
            Assert.AreEqual(tone, note?.Tone.Scale);
        }
    }
}
