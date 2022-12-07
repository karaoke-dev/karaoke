// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapPagesChangeHandler : BeatmapPropertyChangeHandler, IBeatmapPagesChangeHandler
{
    public void Add(Page page)
    {
        performPageInfoChanged(pageInfo =>
        {
            pageInfo.Pages.Add(page);
        });
    }

    public void Remove(Page page)
    {
        performPageInfoChanged(pageInfo =>
        {
            pageInfo.Pages.Remove(page);
        });
    }

    public void RemoveRange(IEnumerable<Page> pages)
    {
        performPageInfoChanged(pageInfo =>
        {
            foreach (var page in pages.ToArray())
            {
                pageInfo.Pages.Remove(page);
            }
        });
    }

    public void ShiftingPageTime(IEnumerable<Page> pages, double offset)
    {
        performPageInfoChanged(pageInfo =>
        {
            foreach (var page in pages)
            {
                if (!pageInfo.Pages.Contains(page))
                    throw new InvalidOperationException("All pages should be in the page info.");

                page.Time += offset;
            }
        });
    }

    private void performPageInfoChanged(Action<PageInfo> action)
    {
        PerformBeatmapChanged(beatmap =>
        {
            action(beatmap.PageInfo);
        });
    }
}
