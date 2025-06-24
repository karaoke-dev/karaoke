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
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Rulesets.Mods;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats;

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
        using var resStream = TestResources.OpenBeatmapResource("karaoke-file-samples");
        using var stream = new LineBufferedReader(resStream);

        var decoder = Decoder.GetDecoder<Beatmap>(stream);
        Assert.That(decoder.GetType(), Is.EqualTo(typeof(KaraokeLegacyBeatmapDecoder)));

        var working = new TestWorkingBeatmap(decoder.Decode(stream));

        Assert.That(working.Beatmap.BeatmapVersion, Is.EqualTo(1));
        Assert.That(working.GetPlayableBeatmap(new KaraokeRuleset().RulesetInfo, Array.Empty<Mod>()).BeatmapVersion, Is.EqualTo(1));

        // Test lyric part decode result
        var lyrics = working.Beatmap.HitObjects.OfType<Lyric>();
        Assert.That(lyrics.Count(), Is.EqualTo(54));

        // Test note decode part
        var notes = working.Beatmap.HitObjects.OfType<Note>().Where(x => x.Display).ToList();
        Assert.That(notes.Count, Is.EqualTo(36));

        testNote("た", 0, actualNote: notes[0]);
        testNote("だ", 0, actualNote: notes[1]);
        testNote("か", 0, actualNote: notes[2]); // 風,か
        testNote("ぜ", 0, actualNote: notes[3]); // 風,ぜ
        testNote("に", 1, actualNote: notes[4]);
        testNote("揺", 2, actualNote: notes[5]);
        testNote("ら", 3, actualNote: notes[6]);
        testNote("れ", 4, actualNote: notes[7]);
        testNote("て", 3, actualNote: notes[8]);
    }

    [Test]
    public void TestDecodeNotes()
    {
        // Karaoke beatmap
        var beatmap = decodeBeatmap("karaoke-note-samples");

        // Get notes
        var notes = beatmap.HitObjects.OfType<Note>().ToList();

        testNote("か", 1, actualNote: notes[0]);
        testNote("ら", 2, true, notes[1]);
        testNote("お", 3, actualNote: notes[2]);
        testNote("け", 3, true, notes[3]);
        testNote("け", 4, actualNote: notes[4]);
    }

    [Test]
    public void TestDecodeTranslations()
    {
        // Karaoke beatmap
        var beatmap = decodeBeatmap("karaoke-translation-samples");

        // Get translations
        var translations = beatmap.AvailableTranslationLanguages();
        var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();

        // Check is not null
        Assert.That(translations, Is.Not.Null);

        // Check translations count
        Assert.That(translations.Count, Is.EqualTo(2));

        // All lyric should have two translations
        Assert.That(lyrics[0].Translations.Count, Is.EqualTo(2));
        Assert.That(lyrics[1].Translations.Count, Is.EqualTo(2));

        // Check chinese translations
        var chineseLanguageId = translations[0];
        Assert.That(lyrics[0].Translations[chineseLanguageId], Is.EqualTo("卡拉OK"));
        Assert.That(lyrics[1].Translations[chineseLanguageId], Is.EqualTo("喜歡"));

        // Check english translations
        var englishLanguageId = translations[1];
        Assert.That(lyrics[0].Translations[englishLanguageId], Is.EqualTo("karaoke"));
        Assert.That(lyrics[1].Translations[englishLanguageId], Is.EqualTo("like it"));
    }

    private static KaraokeBeatmap decodeBeatmap(string fileName)
    {
        using var resStream = TestResources.OpenBeatmapResource(fileName);
        using var stream = new LineBufferedReader(resStream);

        // Create karaoke beatmap decoder
        var decoder = new KaraokeLegacyBeatmapDecoder();

        // Create initial beatmap
        var beatmap = decoder.Decode(stream);

        // Convert to karaoke beatmap
        return (KaraokeBeatmap)new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert();
    }

    private static void testNote(string expectedText, int expectedTone, bool expectedHalf = false, Note actualNote = default!)
    {
        Assert.That(actualNote.Text, Is.EqualTo(expectedText));
        Assert.That(actualNote.Tone.Scale, Is.EqualTo(expectedTone));
        Assert.That(actualNote.Tone.Half, Is.EqualTo(expectedHalf));
    }

    [TestCase(0, 1, new double[] { 1000, 3000 })]
    [TestCase(0, 0.5, new double[] { 1000, 1500 })]
    [TestCase(0.5, 0.5, new double[] { 2500, 1500 })]
    [TestCase(0.3, 1, null)] // start + duration should not exceed 1
    public void TestSliceNoteTime(double startPercentage, double durationPercentage, double[]? expected)
    {
        var referencedLyric = new Lyric
        {
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[1,start]:4000" }),
        };

        // start time will be 1000, and duration will be 3000.
        var note = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = 0,
        };

        if (expected != null)
        {
            var sliceNote = KaraokeLegacyBeatmapDecoder.SliceNote(note, startPercentage, durationPercentage);
            Assert.That(sliceNote.StartTime, Is.EqualTo(expected[0]));
            Assert.That(sliceNote.Duration, Is.EqualTo(expected[1]));
        }
        else
        {
            Assert.That(() => KaraokeLegacyBeatmapDecoder.SliceNote(note, startPercentage, durationPercentage), Throws.Exception);
        }
    }
}
