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

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LrcEncoder
    {
        public string Encode(Beatmap output)
        {
            var lyric = new Lyric
            {
                Lines = output.HitObjects.OfType<Objects.Lyric>().Select(encodeLyric).ToArray(),
            };
            var encodeResult = new LrcParser().Encode(lyric);
            return encodeResult;
        }

        private LyricLine encodeLyric(Objects.Lyric lyric) =>
            new LyricLine
            {
                Text = lyric.Text,
                // Note : saving to lyrics will lose some tags with no value.
                TimeTags = convertTimeTag(lyric.Text, TimeTagsUtils.ToDictionary(lyric.TimeTags)).ToArray(),
            };

        private IEnumerable<TimeTag> convertTimeTag(string text, IReadOnlyDictionary<TimeTagIndex, double> tags)
        {
            if (tags == null || !tags.Any())
                throw new ArgumentNullException($"{nameof(tags)} cannot be null.");

            var totalTags = text.Length * 2 + 2;

            for (int i = 0; i < totalTags; i++)
            {
                var (lastTag, lastTagTime) = tags.LastOrDefault();

                if (lastTag.Index * 2 == i)
                {
                    yield return new TimeTag
                    {
                        Time = (int)lastTagTime,
                        Check = true,
                        KeyUp = true
                    };

                    continue;
                }

                var (firstTag, firstTagTime) = tags.FirstOrDefault(x => x.Key.Index * 2 + 1 == i);

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

                yield return new TimeTag();
            }
        }
    }
}
