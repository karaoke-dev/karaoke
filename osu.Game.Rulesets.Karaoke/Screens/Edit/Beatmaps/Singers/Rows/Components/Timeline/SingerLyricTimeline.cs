// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows.Components.Timeline;

[Cached]
public partial class SingerLyricTimeline : EditableTimeline
{
    private const float timeline_height = 38;

    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    public readonly Singer Singer;

    public SingerLyricTimeline(Singer singer)
    {
        Singer = singer;

        RelativeSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load(ISingerScreenScrollingInfoProvider scrollingInfoProvider, OsuColour colours)
    {
        AddInternal(new Box
        {
            Name = "Background",
            Depth = 1,
            RelativeSizeAxes = Axes.X,
            Height = timeline_height,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Colour = colours.Gray3,
        });

        BindableZoom.BindTo(scrollingInfoProvider.BindableZoom);
        BindableCurrent.BindTo(scrollingInfoProvider.BindableCurrent);
    }

    protected override Container CreateMainContainer()
    {
        return base.CreateMainContainer().With(c => c.Height = timeline_height);
    }

    protected override IEnumerable<Drawable> CreateBlueprintContainer()
    {
        yield return new SingerLyricEditorBlueprintContainer();
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        const float preempt_time = 1000;

        var firstLyric = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault(x => x.LyricStartTime > 0);
        double? lyricStartTime = firstLyric?.LyricStartTime;
        if (lyricStartTime == null)
            return;

        float position = PositionAtTime(lyricStartTime.Value - preempt_time);
        ScrollTo(position, false);
    }
}
