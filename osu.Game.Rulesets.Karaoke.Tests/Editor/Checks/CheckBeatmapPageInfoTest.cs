// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckBeatmapPageInfo;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckBeatmapPageInfoTest : BeatmapPropertyCheckTest<CheckBeatmapPageInfo>
{
    [Test]
    public void TestCheckLessThanTwoPages()
    {
        var pages = new List<Page>
        {
            new(),
        };

        var lyrics = new List<Lyric>
        {
            new(),
        };

        // should have at least two pages.
        var beatmap = createTestingBeatmap(null, null);
        AssertNotOk<IssueTemplateLessThanTwoPages>(getContext(beatmap));

        // should have at least two pages.
        var beatmap2 = createTestingBeatmap(pages, null);
        AssertNotOk<IssueTemplateLessThanTwoPages>(getContext(beatmap2));

        // should have at least two pages.
        var beatmap3 = createTestingBeatmap(null, lyrics);
        AssertNotOk<IssueTemplateLessThanTwoPages>(getContext(beatmap3));

        // should have at least two pages.
        var beatmap4 = createTestingBeatmap(pages, lyrics);
        AssertNotOk<IssueTemplateLessThanTwoPages>(getContext(beatmap4));
    }

    [Test]
    public void TestCheckPageIntervalTooShort()
    {
        var pages = new List<Page>
        {
            new()
            {
                Time = 0,
            },
            new()
            {
                Time = MIN_INTERVAL - 1,
            },
        };

        var lyrics = new List<Lyric>
        {
            // create a lyric that between two time-tags
            new()
            {
                TimeTags = new List<TimeTag>
                {
                    new(new TextIndex(), 500)
                }
            },
        };

        var beatmap = createTestingBeatmap(pages, lyrics);
        AssertNotOk<BeatmapPageIssue, IssueTemplatePageIntervalTooShort>(getContext(beatmap));
    }

    [Test]
    public void TestCheckPageIntervalTooLong()
    {
        var pages = new List<Page>
        {
            new()
            {
                Time = 0,
            },
            new()
            {
                Time = MAX_INTERVAL + 1,
            },
        };

        var lyrics = new List<Lyric>
        {
            // create a lyric that between two time-tags
            new()
            {
                TimeTags = new List<TimeTag>
                {
                    new(new TextIndex(), 1000)
                }
            },
        };

        var beatmap = createTestingBeatmap(pages, lyrics);
        AssertNotOk<BeatmapPageIssue, IssueTemplatePageIntervalTooLong>(getContext(beatmap));
    }

    [Test]
    public void TestCheckPageIntervalShouldHaveAtLeastOneLyric()
    {
        var pages = new List<Page>
        {
            new()
            {
                Time = 0,
            },
            new()
            {
                Time = MIN_INTERVAL,
            },
        };

        var beatmap = createTestingBeatmap(pages, null);
        AssertNotOk<BeatmapPageIssue, IssueTemplatePageIntervalShouldHaveAtLeastOneLyric>(getContext(beatmap));
    }

    [Test]
    public void TestCheckLyricNotWrapIntoTime()
    {
        var pages = new List<Page>
        {
            new()
            {
                Time = 0,
            },
            new()
            {
                Time = MIN_INTERVAL,
            },
        };

        var timeTag = new TimeTag(new TextIndex(), 2000);

        var lyrics = new List<Lyric>
        {
            // create a lyric that between two time-tags
            new()
            {
                TimeTags = new List<TimeTag>
                {
                    new(new TextIndex(), MIN_INTERVAL - 1)
                }
            },
            // another lyric's time is adjustable.
            new()
            {
                TimeTags = new List<TimeTag>
                {
                    timeTag
                }
            },
        };

        // should be OK
        var beatmap = createTestingBeatmap(pages, lyrics);
        AssertOk(getContext(beatmap));

        // out of range.
        timeTag.Time = -1;
        var beatmap2 = createTestingBeatmap(pages, lyrics);
        AssertNotOk<LyricIssue, IssueTemplateLyricNotWrapIntoTime>(getContext(beatmap2));

        // out of range.
        timeTag.Time = MIN_INTERVAL + 1;
        var beatmap3 = createTestingBeatmap(pages, lyrics);
        AssertNotOk<LyricIssue, IssueTemplateLyricNotWrapIntoTime>(getContext(beatmap3));
    }

    private static IBeatmap createTestingBeatmap(IEnumerable<Page>? pages, IEnumerable<Lyric>? lyrics)
    {
        var karaokeBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>()
        };
        karaokeBeatmap.PageInfo.Pages.AddRange(pages ?? new List<Page>());
        return new EditorBeatmap(karaokeBeatmap);
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap)
        => new(beatmap, new TestWorkingBeatmap(beatmap));
}
