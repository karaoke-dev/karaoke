// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Drawables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.LyricList.Rows.Info.Badge;

public partial class SingerInfo : Container
{
    private readonly SingerDisplay singerDisplay;

    private readonly IBindableDictionary<Singer, SingerState[]> singerIndexesBindable;

    public SingerInfo(Lyric lyric)
    {
        singerIndexesBindable = lyric.SingersBindable.GetBoundCopy();

        AutoSizeAxes = Axes.Both;

        Child = singerDisplay = new SingerDisplay
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        singerIndexesBindable.BindCollectionChanged((_, _) =>
        {
            // todo: maybe should display the singer state also?
            singerDisplay.Current.Value = singerIndexesBindable.Keys.ToArray();
        }, true);
    }
}
