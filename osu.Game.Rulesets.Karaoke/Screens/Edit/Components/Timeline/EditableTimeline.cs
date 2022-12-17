// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Containers;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

[Cached]
public partial class EditableTimeline : BindableScrollContainer
{
    [Resolved, AllowNull]
    private EditorClock editorClock { get; set; }

    public EditableTimeline()
    {
        ZoomDuration = 200;
        ZoomEasing = Easing.OutQuint;
        ScrollbarVisible = false;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddRange(new Drawable[]
        {
            CreateMainContainer().With(c =>
            {
                c.RelativeSizeAxes = Axes.X;
                c.Depth = float.MaxValue;
                c.Children = CreateBlueprintContainer().ToList();
            })
        });
    }

    protected virtual Container CreateMainContainer() =>
        new()
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        };

    protected virtual IEnumerable<Drawable> CreateBlueprintContainer()
    {
        yield return new EditableTimelineBlueprintContainer<Lyric>();
    }

    public double TimeAtPosition(float x)
    {
        return x / Content.DrawWidth * editorClock.TrackLength;
    }

    public float PositionAtTime(double time)
    {
        return (float)(time / editorClock.TrackLength * Content.DrawWidth);
    }
}
