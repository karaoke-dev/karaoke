// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using LyricMaker.Extensions;
using LyricMaker.Parser;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LrcDecoder : Decoder<KaraokeBeatmap>
    {
        public static void Register()
        {
            // Lrc decoder looks like [mm:ss:__]
            AddDecoder<Beatmap>("[", m => new LrcDecoder());
        }

        protected override void ParseStreamInto(LineBufferedReader stream, KaraokeBeatmap output)
        {
            // Clear all hitobjects
            output.HitObjects.Clear();

            var lyricText = stream.ReadToEnd();
            var result = new LrcParser().Decode(lyricText);

            // Convert line
            foreach (var line in result.Lines)
            {
                var lyric = line.Text;
                output.HitObjects.Add(new LyricLine
                {
                    Text = lyric,
                    // Start time and end time should be re-assigned
                    StartTime = line.TimeTags.FirstOrDefault(x => x.Time > 0).Time,
                    EndTime = line.TimeTags.LastOrDefault(x => x.Time > 0).Time,
                    TimeTags = line.TimeTags.Select(tag => new TimeTag
                    {
                        StartTime = tag.Time,
                        Check = tag.Check,
                        KeyUp = tag.KeyUp
                    }).ToArray(),
                    // Get ruby from each line
                    RubyTags = result.QueryRubies(lyric).Select(ruby => new RubyTag
                    {
                        Ruby = ruby.Ruby.Ruby,
                        StartIndex = ruby.StartIndex,
                        EndIndex = ruby.EndIndex
                    }).ToArray()
                });
            }
        }
    }
}
