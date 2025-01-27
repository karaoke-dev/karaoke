// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Extensions;

public static class ScrollContainerExtensions
{
    /// <summary>
    /// Extend the <see cref="ScrollContainer{T}"/> to scroll into view with spacing.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="d"></param>
    /// <param name="p"></param>
    /// <param name="animated"></param>
    /// <typeparam name="T"></typeparam>
    public static void ScrollIntoViewWithSpacing<T>(this ScrollContainer<T> container, Drawable d, MarginPadding p, bool animated = true)
        where T : Drawable
    {
        double childPos0 = Math.Clamp(container.GetChildPosInContent(d, -new Vector2(p.Left, p.Top)), 0, container.AvailableContent);
        double childPos1 = Math.Clamp(container.GetChildPosInContent(d, d.DrawSize + new Vector2(p.Right, p.Bottom)), 0, container.AvailableContent);

        int scrollDim = container.ScrollDirection == Direction.Horizontal ? 0 : 1;
        double minPos = Math.Min(childPos0, childPos1);
        double maxPos = Math.Max(childPos0, childPos1);

        if (minPos < container.Current || (minPos > container.Current && d.DrawSize[scrollDim] > container.DisplayableContent))
            container.ScrollTo(minPos, animated);
        else if (maxPos > container.Current + container.DisplayableContent)
            container.ScrollTo(maxPos - container.DisplayableContent, animated);
    }
}
