// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using LrcParser.Model;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Objects;
using Lyric = osu.Game.Rulesets.Karaoke.Objects.Lyric;
using RubyTag = osu.Game.Rulesets.Karaoke.Objects.RubyTag;
using TextIndex = osu.Framework.Graphics.Sprites.TextIndex;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LrcDecoder : Decoder<Beatmap>
    {
        public static void Register()
        {
            // Lrc decoder looks like [mm:ss:__]
            AddDecoder<Beatmap>("[", _ => new LrcDecoder());
        }

        protected override void ParseStreamInto(LineBufferedReader stream, Beatmap output)
        {
            // Clear all hitobjects
            output.HitObjects.Clear();

            string lyricText = stream.ReadToEnd();
            var result = new LrcParser.Parser.Lrc.LrcParser().Decode(lyricText);

            foreach (var lrcLyric in result.Lyrics)
            {
                var lrcTimeTags = lrcLyric.TimeTags.Select(convertTimeTag).ToList();
                var lrcRubies = lrcLyric.RubyTags.Select(convertRubyTag).ToArray();

                double? startTime = lrcTimeTags.Select(x => x.Time).Min();
                double? endTime = lrcTimeTags.Select(x => x.Time).Max();
                double? duration = endTime - startTime;

                var lyric = new Lyric
                {
                    ID = output.HitObjects.Count, // id is star from zero.
                    Order = output.HitObjects.Count + 1, // should create default order.
                    Text = lrcLyric.Text,
                    // Start time and end time should be re-assigned
                    StartTime = startTime ?? 0,
                    Duration = duration ?? 0,
                    TimeTags = lrcTimeTags,
                    RubyTags = lrcRubies
                };
                lyric.InitialWorkingTime();
                output.HitObjects.Add(lyric);
            }

            static TimeTag convertTimeTag(KeyValuePair<LrcParser.Model.TextIndex, int?> timeTag)
                => new(convertTextIndex(timeTag.Key), timeTag.Value);

            static TextIndex convertTextIndex(LrcParser.Model.TextIndex textIndex)
            {
                int index = textIndex.Index;
                var state = textIndex.State == IndexState.Start ? TextIndex.IndexState.Start : TextIndex.IndexState.End;

                return new TextIndex(index, state);
            }

            static RubyTag convertRubyTag(LrcParser.Model.RubyTag rubyTag)
                => new()
                {
                    Text = rubyTag.Text,
                    StartIndex = rubyTag.StartIndex,
                    EndIndex = rubyTag.EndIndex
                };
        }
    }
}
