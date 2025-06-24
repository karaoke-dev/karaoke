// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Replays;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Replays;

public partial class TestSceneAutoGeneration : OsuTestScene
{
    [Test]
    public void TestSingleShortNote()
    {
        // |     |
        // |  -  |
        // |     |

        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "karaoke!", 1000, 50);

        var beatmap = new KaraokeBeatmap();
        beatmap.HitObjects.Add(new Note
        {
            Text = "karaoke!",
            Display = true,
            Tone = new Tone(0, true),
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
        });

        var generated = new KaraokeAutoGenerator(beatmap).Generate();

        Assert.That(generated.Frames.Count == 2, "Replay must have 2 frames, start and end.");
        Assert.That(generated.Frames[0].Time, Is.EqualTo(1000), "Incorrect time");
        Assert.That(generated.Frames[1].Time, Is.EqualTo(1051), "Incorrect time");
        Assert.That(checkMatching(generated.Frames[0], new Tone(0, true)), "Frame1 should sing.");
        Assert.That(checkMatching(generated.Frames[1], null), "Frame2 should release sing.");
    }

    [Test]
    public void TestSingleNoteWithLongTime()
    {
        // |     |
        // | *** |
        // |     |

        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "karaoke!", 1000, 1000);

        var beatmap = new KaraokeBeatmap();
        beatmap.HitObjects.Add(new Note
        {
            Text = "karaoke!",
            Display = true,
            Tone = new Tone(0, true),
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
        });

        var generated = new KaraokeAutoGenerator(beatmap).Generate();

        Assert.That(generated.Frames.Count, Is.EqualTo(11), "Replay must have 11 frames,Start, duration(9 frames) and end.");
        Assert.That(generated.Frames[0].Time, Is.EqualTo(1000), "Incorrect hit time");
        Assert.That(generated.Frames[10].Time, Is.EqualTo(2001), "Incorrect time");
        Assert.That(checkMatching(generated.Frames[0], new Tone(0, true)), "Fist frame should sing.");
        Assert.That(checkMatching(generated.Frames[10], null), "Last frame should not sing.");
    }

    [Test]
    public void TestNoteStair()
    {
        // |      |
        // |    - |
        // | -    |

        var firstReferencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "karaoke!", 1000, 50);
        var secondReferencedLyric = TestCaseNoteHelper.CreateLyricForNote(3, "karaoke!", 1050, 50);

        var beatmap = new KaraokeBeatmap();
        beatmap.HitObjects.Add(new Note
        {
            Text = "kara",
            Display = true,
            Tone = new Tone(0, true),
            ReferenceLyricId = firstReferencedLyric.ID,
            ReferenceLyric = firstReferencedLyric,
        });
        beatmap.HitObjects.Add(new Note
        {
            Text = "oke!",
            Display = true,
            Tone = new Tone(1, true),
            ReferenceLyricId = secondReferencedLyric.ID,
            ReferenceLyric = secondReferencedLyric,
        });

        var generated = new KaraokeAutoGenerator(beatmap).Generate();

        Assert.That(generated.Frames.Count, Is.EqualTo(3), "Replay must have 3 frames, note1's start, note2's start and note2's end.");
        Assert.That(generated.Frames[0].Time, Is.EqualTo(1000), "Incorrect time");
        Assert.That(generated.Frames[1].Time, Is.EqualTo(1050), "Incorrect time");
        Assert.That(generated.Frames[2].Time, Is.EqualTo(1101), "Incorrect time");
        Assert.That(checkMatching(generated.Frames[0], new Tone(0, true)), "Frame1 should sing.");
        Assert.That(checkMatching(generated.Frames[1], new Tone(1, true)), "Frame2 should sing.");
        Assert.That(checkMatching(generated.Frames[2], null), "Frame3 should release sing.");
    }

    private static bool checkMatching(ReplayFrame frame, Tone? tone)
    {
        if (frame is not KaraokeReplayFrame karaokeReplayFrame)
            throw new InvalidCastException($"{nameof(frame)} is not karaoke replay frame.");

        if (!karaokeReplayFrame.Sound)
            return !tone.HasValue;

        if (tone == null)
            throw new ArgumentNullException(nameof(tone));

        return karaokeReplayFrame.Scale == tone.Value.Scale + (tone.Value.Half ? 0.5f : 0);
    }
}
