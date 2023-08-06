// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

public class KaraokeBeatmap : Beatmap<KaraokeHitObject>
{
    public IList<CultureInfo> AvailableTranslates { get; set; } = new List<CultureInfo>();

    public SingerInfo SingerInfo { get; set; } = new();

    public PageInfo PageInfo { get; set; } = new();

    public IList<StageInfo> StageInfos { get; set; } = new List<StageInfo>();

    /// <summary>
    /// This property will not be null after <see cref="KaraokeBeatmapProcessor.PreProcess"/> is called.
    /// </summary>
    [JsonIgnore]
    public StageInfo? CurrentStageInfo { get; set; }

    public NoteInfo NoteInfo { get; set; } = new();

    public bool Scorable { get; set; }

    public override IEnumerable<BeatmapStatistic> GetStatistics()
    {
        int singers = SingerInfo.GetAllSingers().Count();
        int lyrics = HitObjects.Count(s => s is Lyric);

        var defaultStatistic = new List<BeatmapStatistic>
        {
            new()
            {
                Name = @"Singer",
                Content = singers.ToString(),
                CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.User },
            },
            new()
            {
                Name = @"Lyric",
                Content = lyrics.ToString(),
                CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.AlignLeft },
            },
        };

        if (Scorable)
        {
            int notes = HitObjects.Count(s => s is Note { Display: true });
            defaultStatistic.Add(new BeatmapStatistic
            {
                Name = @"Note",
                Content = notes.ToString(),
                CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.Music },
            });
        }
        else
        {
            defaultStatistic.Add(new BeatmapStatistic
            {
                Name = @"This beatmap is not scorable.",
                Content = @"This beatmap is not scorable.",
                CreateIcon = () => new SpriteIcon { Icon = FontAwesome.Solid.Times },
            });
        }

        return defaultStatistic.ToArray();
    }
}
