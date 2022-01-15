// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo
{
    public class SingerInfo : Container
    {
        private readonly SingerDisplay singerDisplay;

        private readonly Lyric lyric;

        public SingerInfo(Lyric lyric)
        {
            this.lyric = lyric;
            AutoSizeAxes = Axes.Both;

            Child = singerDisplay = new SingerDisplay
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            };
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap)
        {
            lyric.SingersBindable.BindCollectionChanged((_, _) =>
            {
                if (beatmap.PlayableBeatmap is not KaraokeBeatmap karaokeBeatmap)
                    return;

                var singers = karaokeBeatmap.Singers?.Where(x => lyric.SingersBindable?.Contains(x.ID) ?? false).ToList();

                if (singers?.Any() ?? false)
                {
                    singerDisplay.Current.Value = singers;
                }
                else
                {
                    singerDisplay.Current.Value = new List<Singer>
                    {
                        new()
                        {
                            Name = "No singer"
                        }
                    };
                }
            }, true);
        }
    }
}
