// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Components.Timeline;

public partial class PagesTimeLine : EditableTimeline
{
    public const float TIMELINE_HEIGHT = 38;

    [BackgroundDependencyLoader]
    private void load(OsuColour colours, IPageStateProvider pageStateProvider)
    {
        AddInternal(new Box
        {
            Name = "Background",
            Depth = 1,
            RelativeSizeAxes = Axes.X,
            Height = TIMELINE_HEIGHT,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Colour = colours.Gray3,
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
