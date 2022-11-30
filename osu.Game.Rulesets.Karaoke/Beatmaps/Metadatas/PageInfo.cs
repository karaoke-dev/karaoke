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
        return Pages.LastOrDefault(x => x.Time <= time);
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
