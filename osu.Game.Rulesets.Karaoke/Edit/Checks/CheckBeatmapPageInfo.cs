// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckBeatmapPageInfo : CheckBeatmapProperty<PageInfo, Lyric>
{
    public const double MIN_INTERVAL = 3000;

    public const double MAX_INTERVAL = 10000;

    protected override string Description => "Check invalid page in the beatmap";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateLessThanTwoPages(this),
        new IssueTemplatePageIntervalTooShort(this),
        new IssueTemplatePageIntervalTooLong(this),
        new IssueTemplatePageIntervalShouldHaveAtLeastOneLyric(this),
        new IssueTemplateLyricNotWrapIntoTime(this),
    };

    protected override PageInfo GetPropertyFromBeatmap(KaraokeBeatmap karaokeBeatmap)
        => karaokeBeatmap.PageInfo;

    protected override IEnumerable<Issue> CheckProperty(PageInfo property)
    {
        var pages = property.Pages;

        if (pages.Count < 2)
        {
            yield return new IssueTemplateLessThanTwoPages(this).Create();

            yield break;
        }

        for (int i = 1; i < pages.Count; i++)
        {
            var previous = pages[i - 1];
            var current = pages[i];

            double previousTime = previous.Time;
            double currentTime = current.Time;

            if (currentTime - previousTime < MIN_INTERVAL)
                yield return new IssueTemplatePageIntervalTooShort(this).Create(previous, current);

            if (currentTime - previousTime > MAX_INTERVAL)
                yield return new IssueTemplatePageIntervalTooLong(this).Create(previous, current);
        }
    }

    protected override IEnumerable<Issue> CheckHitObjects(PageInfo property, IReadOnlyList<Lyric> hitObject)
    {
        var pages = property.Pages;
        if (pages.Count < 2)
            yield break;

        var availablePagesInObject = hitObject.ToDictionary(k => k, v => v.TimeValid ? property.GetPageAt(v.StartTime) : null);

        var missingHitObjectPages = pages.Where(page => !availablePagesInObject.ContainsValue(page)).ToArray();

        for (int i = 1; i < missingHitObjectPages.Length; i++)
        {
            var previous = missingHitObjectPages[i - 1];
            var current = missingHitObjectPages[i];

            yield return new IssueTemplatePageIntervalShouldHaveAtLeastOneLyric(this).Create(previous, current);
        }

        foreach (var lyric in availablePagesInObject.Where(x => x.Value == null).Select(x => x.Key))
        {
            yield return new IssueTemplateLyricNotWrapIntoTime(this).Create(lyric);
        }
    }

    public class IssueTemplateLessThanTwoPages : IssueTemplate
    {
        public IssueTemplateLessThanTwoPages(ICheck check)
            : base(check, IssueType.Warning, "Should have at least two pages.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplatePageIntervalTooShort : IssueTemplate
    {
        public IssueTemplatePageIntervalTooShort(ICheck check)
            : base(check, IssueType.Warning, "Interval between two pages are too short.")
        {
        }

        public Issue Create(Page startPage, Page endPage)
            => new BeatmapPageIssue(startPage, endPage, this);
    }

    public class IssueTemplatePageIntervalTooLong : IssueTemplate
    {
        public IssueTemplatePageIntervalTooLong(ICheck check)
            : base(check, IssueType.Warning, "Interval between two pages are too long.")
        {
        }

        public Issue Create(Page startPage, Page endPage)
            => new BeatmapPageIssue(startPage, endPage, this);
    }

    public class IssueTemplatePageIntervalShouldHaveAtLeastOneLyric : IssueTemplate
    {
        public IssueTemplatePageIntervalShouldHaveAtLeastOneLyric(ICheck check)
            : base(check, IssueType.Negligible, "Should have at least one lyric between two pages.")
        {
        }

        public Issue Create(Page startPage, Page endPage)
            => new BeatmapPageIssue(startPage, endPage, this);
    }

    public class IssueTemplateLyricNotWrapIntoTime : IssueTemplate
    {
        public IssueTemplateLyricNotWrapIntoTime(ICheck check)
            : base(check, IssueType.Negligible, "Lyric is not wrap by the page.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }
}
