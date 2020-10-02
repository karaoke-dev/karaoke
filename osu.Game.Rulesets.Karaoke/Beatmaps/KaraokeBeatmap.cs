// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmap : Beatmap<KaraokeHitObject>
    {
        public IDictionary<string, List<string>> Translates { get; set; } = new Dictionary<string, List<string>>();

        public IDictionary<int, Singer> Singers { get; set; } = new Dictionary<int, Singer>
        {
            {
                0,
                new Singer
                {
                    Name = "Default singer",
                    Romaji = "",
                    EnglishName = "",
                    Color = Color4.Blue
                }
            }
        };

        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int singers = Singers.Count();
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
