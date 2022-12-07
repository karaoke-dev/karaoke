// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Lists;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

public class PageInfo : IDeepCloneable<PageInfo>
{
    public SortedList<Page> Pages = new(Comparer<Page>.Default);

    public Page? GetPageAt(double time)
    {
        if (Pages.Count < 2)
            return null;

        var page = Pages.LastOrDefault(x => x.Time <= time);

        // should not be able to get the page if time exceed the last page.
        var lastPage = Pages.LastOrDefault();
        if (page == lastPage && page?.Time < time)
            return null;

        return page;
    }

    public int? GetPageIndexAt(double time)
    {
        var page = GetPageAt(time);
        if (page == null)
            return null;

        return Pages.FindIndex(x => x == page);
    }

    public PageInfo DeepClone()
    {
        var controlPointInfo = (PageInfo)Activator.CreateInstance(GetType());

        return controlPointInfo;
    }
}
