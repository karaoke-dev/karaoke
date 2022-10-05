// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Info.Badge
{
    public class SingerInfo : Container
    {
        private readonly Lyric lyric;
        private readonly SingerDisplay singerDisplay;

        private readonly IBindableList<int> singerIndexesBindable;

        public SingerInfo(Lyric lyric)
        {
            this.lyric = lyric;
            singerIndexesBindable = lyric.SingersBindable.GetBoundCopy();

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
            singerIndexesBindable.BindCollectionChanged((_, _) =>
            {
                if (beatmap.PlayableBeatmap is not KaraokeBeatmap karaokeBeatmap)
                    return;

                var singers = karaokeBeatmap.Singers.Where(singer => LyricUtils.ContainsSinger(lyric, singer)).ToList();
                singerDisplay.Current.Value = singers;
            }, true);
        }
    }
}
