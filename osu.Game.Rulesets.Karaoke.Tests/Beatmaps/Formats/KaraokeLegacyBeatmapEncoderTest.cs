﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats;

[TestFixture]
public class KaraokeLegacyBeatmapEncoderTest
{
    [Test]
    public void TestEncodeBeatmapLyric()
    {
        // Because encoder is not fully implemented, so just test not crash during encoding.
        const int start_time = 1000;

        var beatmap = new Beatmap
        {
            HitObjects = new List<HitObject>
            {
                new Lyric
                {
                    Text = "カラオケ！",
                    TimeTags = new List<TimeTag>
                    {
                        new(new TextIndex(0), start_time + 500),
                        new(new TextIndex(1), start_time + 600)
                        {
                            FirstSyllable = true,
                            RomanisedSyllable = "ra",
                        },
                        new(new TextIndex(2), start_time + 1000),
                        new(new TextIndex(3), start_time + 1500)
                        {
                            FirstSyllable = true,
                            RomanisedSyllable = "ke",
                        },
                        new(new TextIndex(4), start_time + 2000),
                    },
                    RubyTags = new[]
                    {
                        new RubyTag
                        {
                            StartIndex = 0,
                            EndIndex = 0,
                            Text = "か",
                        },
                        new RubyTag
                        {
                            StartIndex = 2,
                            EndIndex = 2,
                            Text = "お",
                        },
                    },
                },
            },
        };

        using var ms = new MemoryStream();
        using var sw = new StreamWriter(ms);

        var encoder = new KaraokeLegacyBeatmapEncoder();
        string encodeResult = encoder.Encode(beatmap);
        sw.WriteLine(encodeResult);
    }
}
