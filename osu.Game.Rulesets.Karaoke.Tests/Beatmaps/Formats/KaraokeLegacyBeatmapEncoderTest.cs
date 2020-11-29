// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class KaraokeLegacyBeatmapEncoderTest
    {
        [Test]
        public void TestEncodeBeatmapLyric()
        {
            // Because encoder is not fully implemented, this is just test and not crash during encoding.
            const int start_time = 1000;
            const int duration = 2500;

            var beatmap = new Beatmap
            {
                HitObjects = new List<HitObject>
                {
                    new Lyric
                    {
                        StartTime = start_time,
                        Duration = duration,
                        Text = "カラオケ！",
                        TimeTags = TimeTagsUtils.ToTimeTagList(new Dictionary<TimeTagIndex, double>
                        {
                            { new TimeTagIndex(0), start_time + 500 },
                            { new TimeTagIndex(1), start_time + 600 },
                            { new TimeTagIndex(2), start_time + 1000 },
                            { new TimeTagIndex(3), start_time + 1500 },
                            { new TimeTagIndex(4), start_time + 2000 },
                        }),
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
