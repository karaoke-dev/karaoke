// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmap : Beatmap<KaraokeHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int lyrics = HitObjects.Count(s => s is LyricLine);

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Lyric",
                    Content = lyrics.ToString(),
                    Icon = FontAwesome.Regular.Circle
                },
            };
        }
    }
}
