// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmap : Beatmap<KaraokeHitObject>
    {
        public BeatmapSetOnlineLanguage[] AvailableTranslates { get; set; } = { };

        public SingerMetadata SingerMetadata { get; set; } = new SingerMetadata();

        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int singers = SingerMetadata.Singers.Count;
            int lyrics = HitObjects.Count(s => s is LyricLine);

            var defaultStatistic = new List<BeatmapStatistic>
            {
                new BeatmapStatistic
                {
                    Name = @"Singer",
                    Content = singers.ToString(),
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.User }
                },
                new BeatmapStatistic
                {
                    Name = @"Lyric",
                    Content = lyrics.ToString(),
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.AlignLeft }
                },
            };

            var scorable = this.IsScorable();

            if (scorable)
            {
                int notes = HitObjects.Count(s => s is Note note && note.Display);
                defaultStatistic.Add(new BeatmapStatistic
                {
                    Name = @"Note",
                    Content = notes.ToString(),
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.Music }
                });
            }
            else
            {
                defaultStatistic.Add(new BeatmapStatistic
                {
                    Name = @"This beatmap is not scorable.",
                    Content = @"This beatmap is not scorable.",
                    CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.Times }
                });
            }

            return defaultStatistic.ToArray();
        }
    }
}
