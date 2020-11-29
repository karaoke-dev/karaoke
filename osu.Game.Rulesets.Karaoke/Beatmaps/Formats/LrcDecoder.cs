// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
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
using osu.Game.Rulesets.Karaoke.Utils;

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
            for (int i = 0; i < result.Lines.Length; i++)
            {
                // Empty lines should not be imported
                var line = result.Lines[i];
                if (string.IsNullOrEmpty(line.Text))
                    continue;

                try
                {
                    // todo : check list ls sorted by time.
                    var lrcTimeTag = line.TimeTags;
                    var timeTags = line.TimeTags.Where(x => x.Check).ToDictionary(k =>
                    {
                        var index = (int)Math.Ceiling((double)(Array.IndexOf(lrcTimeTag, k) - 1) / 2);
                        var state = (Array.IndexOf(lrcTimeTag, k) - 1) % 2 == 0 ? TimeTagIndex.IndexState.Start : TimeTagIndex.IndexState.End;
                        return new TimeTagIndex(index, state);
                    }, v => (double)v.Time);

                    var startTime = timeTags.FirstOrDefault(x => x.Value > 0).Value;
                    var duration = timeTags.LastOrDefault(x => x.Value > 0).Value - startTime;

                    var lyric = new Lyric
                    {
                        Text = line.Text,
                        // Start time and end time should be re-assigned
                        StartTime = startTime,
                        Duration = duration,
                        TimeTags = TimeTagsUtils.ToTimeTagList(timeTags),
                        RubyTags = result.QueryRubies(line.Text).Select(ruby => new RubyTag
                        {
                            Text = ruby.Ruby.Ruby,
                            StartIndex = ruby.StartIndex,
                            EndIndex = ruby.EndIndex
                        }).ToArray()
                    };
                    lyric.InitialWorkingTime();
                    output.HitObjects.Add(lyric);
                }
                catch (Exception ex)
                {
                    var message = $"Parsing lyric '{line.Text}' got error in line:{i}" +
                                  "Please check time tag should be ordered and not duplicated." +
                                  "Then re-import again.";
                    throw new FormatException(message, ex);
                }
            }
        }
    }
}
