// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapPagesChangeHandler : BeatmapPropertyChangeHandler, IBeatmapPagesChangeHandler
{
    [Resolved, AllowNull]
    private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; }

    public LocalisableString? GetNotGeneratableMessage()
    {
        var config = getGeneratorConfig();
        var generator = new PageGenerator(config);
        return generator.GetInvalidMessage(KaraokeBeatmap);
    }

    public void AutoGenerate()
    {
        var config = getGeneratorConfig();
        var generator = new PageGenerator(config);
        var pages = generator.Generate(KaraokeBeatmap);

        performPageInfoChanged(pageInfo =>
        {
            if (config.ClearExistPages)
                pageInfo.Pages.Clear();

            pageInfo.Pages.AddRange(pages);
        });
    }

    private PageGeneratorConfig getGeneratorConfig()
        => generatorConfigManager.Get<PageGeneratorConfig>(KaraokeRulesetEditGeneratorSetting.BeatmapPageGeneratorConfig);

    public void Add(Page page)
    {
        performPageInfoChanged(pageInfo =>
        {
            if (checkPageExist(pageInfo, page))
                throw new InvalidOperationException($"Should not add duplicated {nameof(page)} into the {nameof(pageInfo)}.");

            pageInfo.Pages.Add(page);
        });
    }

    public void Remove(Page page)
    {
        performPageInfoChanged(pageInfo =>
        {
            if (!checkPageExist(pageInfo, page))
                throw new InvalidOperationException($"{nameof(page)} does ont in the {nameof(pageInfo)}.");

            pageInfo.Pages.Remove(page);
        });
    }

    public void RemoveRange(IEnumerable<Page> pages)
    {
        performPageInfoChanged(pageInfo =>
        {
            foreach (var page in pages.ToArray())
            {
                if (!checkPageExist(pageInfo, page))
                    throw new InvalidOperationException($"{nameof(page)} does ont in the {nameof(pageInfo)}.");

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
                if (!checkPageExist(pageInfo, page))
                    throw new InvalidOperationException($"{nameof(page)} does ont in the {nameof(pageInfo)}.");

                page.Time += offset;
            }
        });
    }

    private static bool checkPageExist(PageInfo pageInfo, Page page)
    {
        return pageInfo.Pages.Contains(page);
    }

    private void performPageInfoChanged(Action<PageInfo> action)
    {
        PerformBeatmapChanged(beatmap =>
        {
            action(beatmap.PageInfo);
        });
    }
}
