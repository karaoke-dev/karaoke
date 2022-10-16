// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using LrcParser.Model;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using Lyric = osu.Game.Rulesets.Karaoke.Objects.Lyric;
using RubyTag = LrcParser.Model.RubyTag;
using TextIndex = osu.Framework.Graphics.Sprites.TextIndex;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LrcEncoder
    {
        public string Encode(Beatmap output)
        {
            // Note : save to lyric will lost some tags with no value.
            var song = new Song
            {
                Lyrics = output.HitObjects.OfType<Lyric>().Select(encodeLyric).ToList(),
            };
            string encodeResult = new LrcParser.Parser.Lrc.LrcParser().Encode(song);
            return encodeResult;

            static LrcParser.Model.Lyric encodeLyric(Lyric lyric) =>
                new()
                {
                    Text = lyric.Text,
                    TimeTags = convertTimeTag(lyric.TimeTags),
                    RubyTags = convertRubyTag(lyric.RubyTags)
                };

            static SortedDictionary<LrcParser.Model.TextIndex, int?> convertTimeTag(IList<TimeTag> timeTags)
            {
                // Note : save to lyric will lost some tags with duplicated index.
                var timeTagDictionary = ToDictionary(timeTags).ToDictionary(k => convertTextIndex(k.Key), v => (int?)v.Value);
                return new SortedDictionary<LrcParser.Model.TextIndex, int?>(timeTagDictionary);
            }

            static LrcParser.Model.TextIndex convertTextIndex(TextIndex textIndex)
            {
                int index = textIndex.Index;
                var state = textIndex.State == TextIndex.IndexState.Start ? IndexState.Start : IndexState.End;

                return new LrcParser.Model.TextIndex(index, state);
            }

            static List<RubyTag> convertRubyTag(IEnumerable<Objects.RubyTag> rubyTags)
                => rubyTags.Select(x => new RubyTag
                {
                    Text = x.Text,
                    StartIndex = x.StartIndex,
                    EndIndex = x.EndIndex
                }).ToList();
        }

        /// <summary>
        /// Convert list of time tag to dictionary.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="applyFix">Should auto-fix or not</param>
        /// <param name="other">Fix way</param>
        /// <param name="self">Fix way</param>
        /// <returns>Time tags with dictionary format.</returns>
        internal static IReadOnlyDictionary<TextIndex, double> ToDictionary(IList<TimeTag> timeTags, bool applyFix = true, GroupCheck other = GroupCheck.Asc,
                                                                            SelfCheck self = SelfCheck.BasedOnStart)
        {
            // sorted value
            var sortedTimeTags = applyFix ? TimeTagsUtils.FixOverlapping(timeTags, other, self) : TimeTagsUtils.Sort(timeTags);

            // convert to dictionary, will get start's smallest time and end's largest time.
            return sortedTimeTags.Where(x => x.Time != null).GroupBy(x => x.Index)
                                 .Select(x => TextIndexUtils.GetValueByState(x.Key, x.FirstOrDefault(), x.LastOrDefault()))
                                 .ToDictionary(
                                     k => k?.Index ?? throw new ArgumentNullException(nameof(k)),
                                     v => v?.Time ?? throw new ArgumentNullException(nameof(v)));
        }
    }
}
