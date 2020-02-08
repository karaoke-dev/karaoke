// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class KaraokeLegacyBeatmapEncoderTest
    {
        [Test]
        public void TestEncodeBeatmapLyric()
        {
            // Because encoder is not fully implemented, so just test not crash during encoding.
            var startTime = 1000;
            var endTime = startTime + 2500;

            var beatmap = new Beatmap
            {
                HitObjects = new List<HitObject>
                {
                    new LyricLine
                    {
                        StartTime = startTime,
                        EndTime = endTime,
                        Text = "カラオケ！",
                        TimeTags = new Dictionary<TimeTagIndex, double>
                        {
                            { new TimeTagIndex(0), startTime + 500 },
                            { new TimeTagIndex(1), startTime + 600 },
                            { new TimeTagIndex(2), startTime + 1000 },
                            { new TimeTagIndex(3), startTime + 1500 },
                            { new TimeTagIndex(4), startTime + 2000 },
                        },
                        RubyTags = new[]
                        {
                            new RubyTag
                            {
                                StartIndex = 0,
                                EndIndex = 1,
                                Text = "か"
                            },
                            new RubyTag
                            {
                                StartIndex = 2,
                                EndIndex = 3,
                                Text = "お"
                            }
                        },
                        RomajiTags = new[]
                        {
                            new RomajiTag
                            {
                                StartIndex = 1,
                                EndIndex = 2,
                                Text = "ra"
                            },
                            new RomajiTag
                            {
                                StartIndex = 3,
                                EndIndex = 4,
                                Text = "ke"
                            }
                        },
                        TranslateText = "karaoke"
                    }
                }
            };

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            {
                var encoder = new KaraokeLegacyBeatmapEncoder();
                var encodeResult = encoder.Encode(beatmap);
                sw.WriteLine(encodeResult);
            }
        }
    }
}
