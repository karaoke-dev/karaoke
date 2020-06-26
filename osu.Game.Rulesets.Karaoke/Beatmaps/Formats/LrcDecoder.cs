﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using LyricMaker.Extensions;
using LyricMaker.Parser;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LrcDecoder : Decoder<Beatmap>
    {
        public static void Register()
        {
            // Lrc decoder looks like [mm:ss:__]
            AddDecoder<Beatmap>("[", m => new LrcDecoder());
        }

        protected override void ParseStreamInto(LineBufferedReader stream, Beatmap output)
        {
            // Clear all hitobjects
            output.HitObjects.Clear();

            var lyricText = stream.ReadToEnd();
            var result = new LrcParser().Decode(lyricText);

            // Convert line
            foreach (var line in result.Lines)
            {
                // Empty line should not be imported
                if (string.IsNullOrEmpty(line.Text))
                    continue;

                var startTime = line.TimeTags.FirstOrDefault(x => x.Time > 0).Time;
                var duration = line.TimeTags.LastOrDefault(x => x.Time > 0).Time - startTime;

                var lyric = line.Text;
                output.HitObjects.Add(new LyricLine
                {
                    Text = lyric,
                    // Start time and end time should be re-assigned
                    StartTime = startTime,
                    Duration = duration,
                    TimeTags = line.TimeTags.Where(x => x.Check).ToDictionary(k =>
                    {
                        var index = (int)Math.Ceiling((double)(Array.IndexOf(line.TimeTags, k) - 1) / 2);
                        var state = (Array.IndexOf(line.TimeTags, k) - 1) % 2 == 0 ? TimeTagIndex.IndexState.Start : TimeTagIndex.IndexState.End;

                        return new TimeTagIndex(index, state);
                    }, v => (double)v.Time),
                    RubyTags = result.QueryRubies(lyric).Select(ruby => new RubyTag
                    {
                        Text = ruby.Ruby.Ruby,
                        StartIndex = ruby.StartIndex,
                        EndIndex = ruby.EndIndex
                    }).ToArray()
                });
            }
        }
    }
}
