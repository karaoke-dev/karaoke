// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;

public sealed partial class DrawableTimeTag : CompositeDrawable, IHasCustomTooltip<TimeTag>
{
    private readonly IBindable<double?> bindableTime = new Bindable<double?>();

    private readonly DrawableTextIndex drawableTextIndex;

    public Func<TimeTag, OsuColour, Color4>? TimeTagColourFunc;

    public DrawableTimeTag()
    {
        InternalChild = drawableTextIndex = new DrawableTextIndex
        {
            RelativeSizeAxes = Axes.Both,
        };
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        bindableTime.BindValueChanged(x =>
        {
            if (timeTag == null)
                return;

            drawableTextIndex.Colour = TimeTagColourFunc?.Invoke(timeTag, colours) ?? GetDefaultTimeTagColour(colours, timeTag);
        }, true);
    }

    private TimeTag? timeTag;

    public TimeTag? TimeTag
    {
        get => timeTag;
        set
        {
            if (timeTag == value)
                return;

            bindableTime.UnbindBindings();
            timeTag = value;
            Alpha = timeTag == null ? 0 : 1;

            if (timeTag == null)
                return;

            bindableTime.BindTo(timeTag.TimeBindable);
            drawableTextIndex.State = timeTag.Index.State;
        }
    }

    public TimeTag TooltipContent => timeTag ?? new TimeTag(new TextIndex());

    public ITooltip<TimeTag> GetCustomTooltip() => new TimeTagTooltip();

    public static Color4 GetDefaultTimeTagColour(OsuColour colours, TimeTag timeTag)
    {
        bool hasTime = timeTag.Time.HasValue;
        if (!hasTime)
            return colours.Gray7;

        return TextIndexUtils.GetValueByState(timeTag.Index, colours.Yellow, colours.YellowDarker);
    }
}
