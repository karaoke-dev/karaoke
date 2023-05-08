// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public partial class PageEditorVerifier : EditorVerifier, IPageEditorVerifier
{
    [Resolved]
    private EditorClock clock { get; set; } = null!;

    protected override IEnumerable<ICheck> CreateChecks() => new ICheck[] { new CheckBeatmapPageInfo() };

    protected override void LoadComplete()
    {
        base.LoadComplete();
        Refresh();
    }

    public override void Refresh()
    {
        ClearChecks();
        AddChecks(getIssues());
    }

    private IEnumerable<Issue> getIssues()
    {
        return CreateIssues();
    }

    public void Navigate(Issue issue)
    {
        if (issue.Time.HasValue)
            clock.Seek(issue.Time.Value);

        switch (issue)
        {
            case LyricIssue lyricIssue:
                // todo: select the lyric.
                var lyric = lyricIssue.Lyric;
                break;

            case BeatmapPageIssue beatmapPageIssue:
                // todo: select the pages.
                break;

            case Issue:
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
