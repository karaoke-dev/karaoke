// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Components.Timeline;

public partial class PagesTimeLine : EditableTimeline
{
    public const float TIMELINE_HEIGHT = 38;

    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    [BackgroundDependencyLoader]
    private void load(OsuColour colour, IPageStateProvider pageStateProvider)
    {
        AddInternal(new Box
        {
            Name = "Background",
            Depth = 1,
            RelativeSizeAxes = Axes.X,
            Height = TIMELINE_HEIGHT,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Colour = colour.Gray3,
        });

        BindableZoom.BindTo(pageStateProvider.BindableZoom);
        BindableCurrent.BindTo(pageStateProvider.BindableCurrent);
    }

    protected override Container CreateMainContainer()
    {
        return base.CreateMainContainer().With(c => c.Height = TIMELINE_HEIGHT);
    }

    protected override IEnumerable<Drawable> CreateBlueprintContainer()
    {
        yield return new LyricBlueprintContainer();
        yield return new PageBlueprintContainer();
    }
}
