// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows.Components.Timeline;

public partial class LyricTimelineSelectionBlueprint : EditableLyricTimelineSelectionBlueprint
{
    private readonly IBindableList<ElementId> singersBindable;

    public LyricTimelineSelectionBlueprint(Lyric item)
        : base(item)
    {
        singersBindable = Item.SingerIdsBindable.GetBoundCopy();
    }

    [BackgroundDependencyLoader]
    private void load(SingerLyricTimeline timeline)
    {
        singersBindable.BindCollectionChanged((_, _) =>
        {
            // Check is lyric contains this singer, or default singer
            Selectable = lyricInCurrentSinger(Item, timeline.Singer);
        }, true);

        static bool lyricInCurrentSinger(Lyric lyric, Singer singer)
        {
            if (singer == DefaultLyricPlacementColumn.DefaultSinger)
                return !lyric.SingerIds.Any();

            return LyricUtils.ContainsSinger(lyric, singer);
        }
    }
}
