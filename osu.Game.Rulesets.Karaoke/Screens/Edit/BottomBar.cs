// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Screens.Edit.Components;
using osu.Game.Screens.Edit.Components.Timelines.Summary;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

/// <summary>
/// Copy the Component from the <see cref="osu.Game.Screens.Edit"/>
/// </summary>
public partial class BottomBar : CompositeDrawable
{
    private readonly Box background;

    public BottomBar()
    {
        Anchor = Anchor.BottomLeft;
        Origin = Anchor.BottomLeft;

        RelativeSizeAxes = Axes.X;

        Height = 60;

        Masking = true;
        EdgeEffect = new EdgeEffectParameters
        {
            Colour = Color4.Black.Opacity(0.2f),
            Type = EdgeEffectType.Shadow,
            Radius = 10f,
        };

        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
            new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 170),
                    new Dimension(),
                    new Dimension(GridSizeMode.Absolute, 220),
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new TimeInfoContainer { RelativeSizeAxes = Axes.Both },
                        new SummaryTimeline { RelativeSizeAxes = Axes.Both },
                        new PlaybackControl { RelativeSizeAxes = Axes.Both },
                    },
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider)
    {
        background.Colour = colourProvider.Background4;
    }
}
