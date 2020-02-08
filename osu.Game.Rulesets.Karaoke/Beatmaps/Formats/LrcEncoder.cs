// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using System;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LrcEncoder
    {
        public string Encode(Beatmap output)
        {
            var lyric = new LyricMaker.Model.Lyric
            {
                Lines = output.HitObjects.OfType<LyricLine>().Select(encodeLyric).ToArray(),
            };
            var encodeResult = new LyricMaker.Parser.LrcParser().Encode(lyric);
            return encodeResult;
        }

        private LyricMaker.Model.LyricLine encodeLyric(LyricLine lyric) =>
            new LyricMaker.Model.LyricLine
            {
                Text = lyric.Text,
                TimeTags = convertTimeTag(lyric.Text, lyric.TimeTags).ToArray(),
            };

        private IEnumerable<LyricMaker.Model.Tags.TimeTag> convertTimeTag(string text, IReadOnlyDictionary<TimeTagIndex, double> tags)
        {
            if (tags == null || !tags.Any())
                throw new ArgumentNullException($"{nameof(tags)} cannot be null.");

            var totalTags = text.Length * 2 + 2;

            for (int i = 0; i < totalTags; i++)
            {
                var lastTag = tags.LastOrDefault();

                if (lastTag.Key.Index * 2 == i)
                {
                    yield return new LyricMaker.Model.Tags.TimeTag
                    {
                        Time = (int)lastTag.Value,
                        Check = true,
                        KeyUp = true
                    };

                    continue;
                }

                var tag = tags.FirstOrDefault(x => x.Key.Index * 2 + 1 == i);

                if (tag.Value > 0 && tag.Key != lastTag.Key)
                {
                    yield return new LyricMaker.Model.Tags.TimeTag
                    {
                        Time = (int)tag.Value,
                        Check = true,
                        KeyUp = true
                    };

                    continue;
                }

                yield return new LyricMaker.Model.Tags.TimeTag();
            }
        }
    }
}
