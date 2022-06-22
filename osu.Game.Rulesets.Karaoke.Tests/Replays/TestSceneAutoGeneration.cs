// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Replays;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Replays
{
    public class TestSceneAutoGeneration : OsuTestScene
    {
        [Test]
        public void TestSingleShortNote()
        {
            // |     |
            // |  -  |
            // |     |

            var beatmap = new KaraokeBeatmap();
            beatmap.HitObjects.Add(new Note
            {
                Display = true,
                StartTime = 1000,
                Duration = 50,
                Text = "karaoke!",
                Tone = new Tone(0, true)
            });

            var generated = new KaraokeAutoGenerator(beatmap).Generate();

            Assert.IsTrue(generated.Frames.Count == 2, "Replay must have 2 frames, start and end.");
            Assert.AreEqual(1000, generated.Frames[0].Time, "Incorrect time");
            Assert.AreEqual(1051, generated.Frames[1].Time, "Incorrect time");
            Assert.IsTrue(checkMatching(generated.Frames[0], new Tone(0, true)), "Frame1 should sing.");
            Assert.IsTrue(checkMatching(generated.Frames[1], null), "Frame2 should release sing.");
        }

        [Test]
        public void TestSingleNoteWithLongTime()
        {
            // |     |
            // | *** |
            // |     |

            var beatmap = new KaraokeBeatmap();
            beatmap.HitObjects.Add(new Note
            {
                Display = true,
                StartTime = 1000,
                Duration = 1000,
                Text = "karaoke!",
                Tone = new Tone(0, true)
            });

            var generated = new KaraokeAutoGenerator(beatmap).Generate();

            Assert.AreEqual(11, generated.Frames.Count, "Replay must have 11 frames,Start, duration(9 frames) and end.");
            Assert.AreEqual(1000, generated.Frames[0].Time, "Incorrect hit time");
            Assert.AreEqual(2001, generated.Frames[10].Time, "Incorrect time");
            Assert.IsTrue(checkMatching(generated.Frames[0], new Tone(0, true)), "Fist frame should sing.");
            Assert.IsTrue(checkMatching(generated.Frames[10], null), "Last frame should not sing.");
        }

        [Test]
        public void TestNoteStair()
        {
            // |      |
            // |    - |
            // | -    |

            var beatmap = new KaraokeBeatmap();
            beatmap.HitObjects.Add(new Note
            {
                Display = true,
                StartTime = 1000,
                Duration = 50,
                Text = "kara",
                Tone = new Tone(0, true)
            });
            beatmap.HitObjects.Add(new Note
            {
                Display = true,
                StartTime = 1050,
                Duration = 50,
                Text = "oke!",
                Tone = new Tone(1, true)
            });

            var generated = new KaraokeAutoGenerator(beatmap).Generate();

            Assert.AreEqual(3, generated.Frames.Count, "Replay must have 3 frames, note1's start, note2's start and note2's end.");
            Assert.AreEqual(1000, generated.Frames[0].Time, "Incorrect time");
            Assert.AreEqual(1050, generated.Frames[1].Time, "Incorrect time");
            Assert.AreEqual(1101, generated.Frames[2].Time, "Incorrect time");
            Assert.IsTrue(checkMatching(generated.Frames[0], new Tone(0, true)), "Frame1 should sing.");
            Assert.IsTrue(checkMatching(generated.Frames[1], new Tone(1, true)), "Frame2 should sing.");
            Assert.IsTrue(checkMatching(generated.Frames[2], null), "Frame3 should release sing.");
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
}
