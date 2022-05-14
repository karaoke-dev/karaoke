// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using LyricMaker.Model;
using LyricMaker.Model.Tags;
using LyricMaker.Parser;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Utils;
using Lyric = osu.Game.Rulesets.Karaoke.Objects.Lyric;
using KaraokeTimeTag = osu.Game.Rulesets.Karaoke.Objects.TimeTag;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LrcEncoder
    {
        public string Encode(Beatmap output)
        {
            var lyric = new LyricMaker.Model.Lyric
            {
                Lines = output.HitObjects.OfType<Lyric>().Select(encodeLyric).ToArray(),
            };
            string encodeResult = new LrcParser().Encode(lyric);
            return encodeResult;
        }

        private LyricLine encodeLyric(Lyric lyric) =>
            new()
            {
                Text = lyric.Text,
                // Note : save to lyric will lost some tags with no value.
                TimeTags = convertTimeTag(lyric.Text, ToDictionary(lyric.TimeTags)).ToArray(),
            };

        /// <summary>
        /// Convert list of time tag to dictionary.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="applyFix">Should auto-fix or not</param>
        /// <param name="other">Fix way</param>
        /// <param name="self">Fix way</param>
        /// <returns>Time tags with dictionary format.</returns>
        internal static IReadOnlyDictionary<TextIndex, double> ToDictionary(IList<KaraokeTimeTag> timeTags, bool applyFix = true, GroupCheck other = GroupCheck.Asc,
                                                                            SelfCheck self = SelfCheck.BasedOnStart)
        {
            if (timeTags == null)
                return new Dictionary<TextIndex, double>();

            // sorted value
            var sortedTimeTags = applyFix ? TimeTagsUtils.FixOverlapping(timeTags, other, self) : TimeTagsUtils.Sort(timeTags);

            // convert to dictionary, will get start's smallest time and end's largest time.
            return sortedTimeTags.Where(x => x.Time != null).GroupBy(x => x.Index)
                                 .Select(x => TextIndexUtils.GetValueByState(x.Key, x.FirstOrDefault(), x.LastOrDefault()))
                                 .ToDictionary(
                                     k => k?.Index ?? throw new ArgumentNullException(nameof(k)),
                                     v => v?.Time ?? throw new ArgumentNullException(nameof(v)));
        }

        private IEnumerable<TimeTag> convertTimeTag(string text, IReadOnlyDictionary<TextIndex, double> tags)
        {
            // total time-tag amount in lyric maker.
            int totalTags = text.Length * 2 + 2;

            for (int i = 0; i < totalTags; i++)
            {
                // should return empty tag if no time-tag in lyric.
                if (tags.Count == 0)
                {
                    yield return new TimeTag();

                    continue;
                }

                (var lastTag, double lastTagTime) = tags.LastOrDefault();

                // create end time-tag
                if ((lastTag.Index + 1) * 2 == i)
                {
                    yield return new TimeTag
                    {
                        Time = (int)lastTagTime,
                        Check = true,
                        KeyUp = true
                    };

                    continue;
                }

                (var firstTag, double firstTagTime) = tags.FirstOrDefault(x => x.Key.Index * 2 + 1 == i);

                // create start time-tag
                if (firstTagTime > 0 && firstTag != lastTag)
                {
                    yield return new TimeTag
                    {
                        Time = (int)firstTagTime,
                        Check = true,
                        KeyUp = true
                    };

                    continue;
                }

                // if has no match tag in lyric, should return empty one.
                yield return new TimeTag();
            }
        }
    }
}
