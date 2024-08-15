// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using LrcParser.Model;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Utils;
using Lyric = osu.Game.Rulesets.Karaoke.Objects.Lyric;
using RubyTag = osu.Game.Rulesets.Karaoke.Objects.RubyTag;
using TextIndex = osu.Framework.Graphics.Sprites.TextIndex;

namespace osu.Game.Rulesets.Karaoke.Integration.Formats;

public class LrcParserUtils
{
    public static Song ConvertToSong(Beatmap beatmap)
    {
        // Note : save to lyric will lost some tags with no value.
        return new Song
        {
            Lyrics = beatmap.HitObjects.OfType<Lyric>().Select(encodeLyric).ToList(),
        };

        static LrcParser.Model.Lyric encodeLyric(Lyric lyric) =>
            new()
            {
                Text = lyric.Text,
                TimeTags = convertTimeTag(lyric.TimeTags),
                RubyTags = convertRubyTag(lyric.RubyTags),
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
            var state = TextIndexUtils.GetValueByState(textIndex, IndexState.Start, IndexState.End);

            return new LrcParser.Model.TextIndex(index, state);
        }

        static List<LrcParser.Model.RubyTag> convertRubyTag(IEnumerable<RubyTag> rubyTags)
            => rubyTags.Select(x => new LrcParser.Model.RubyTag
            {
                Text = x.Text,
                StartCharIndex = x.StartIndex,
                EndCharIndex = x.EndIndex,
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
                             .Select(x => TextIndexUtils.GetValueByState(x.Key, x.FirstOrDefault, x.LastOrDefault))
                             .ToDictionary(
                                 k => k?.Index ?? throw new ArgumentNullException(nameof(k)),
                                 v => v?.Time ?? throw new ArgumentNullException(nameof(v)));
    }

    public static IEnumerable<Lyric> ConvertToLyrics(Song song)
    {
        return song.Lyrics.Select((lrcLyric, index) =>
        {
            var lrcTimeTags = lrcLyric.TimeTags.Select(convertTimeTag).ToArray();
            var lrcRubies = lrcLyric.RubyTags.Select(convertRubyTag).ToArray();
            var lrcRubyTimeTags = lrcLyric.RubyTags.Select(convertTimeTagsFromRubyTags).SelectMany(x => x).ToArray();

            return new Lyric
            {
                Order = index + 1, // should create default order.
                Text = lrcLyric.Text,
                TimeTags = TimeTagsUtils.Sort(lrcTimeTags.Concat(lrcRubyTimeTags)),
                RubyTags = lrcRubies,
            };
        });

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
                StartIndex = rubyTag.StartCharIndex,
                EndIndex = rubyTag.EndCharIndex,
            };

        static TimeTag[] convertTimeTagsFromRubyTags(LrcParser.Model.RubyTag rubyTag)
        {
            int startIndex = rubyTag.StartCharIndex;
            return rubyTag.TimeTags.Select(x => convertTimeTag(new KeyValuePair<LrcParser.Model.TextIndex, int?>(new LrcParser.Model.TextIndex(startIndex), x.Value))).ToArray();
        }
    }
}
