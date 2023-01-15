// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public partial class PageEditorVerifier : EditorVerifier, IPageEditorVerifier
{
    [Resolved, AllowNull]
    private EditorClock clock { get; set; }

    public IBindableList<Issue> Issues => GetIssues();

    protected override IEnumerable<ICheck> CreateChecks() => new ICheck[] { new CheckBeatmapPageInfo() };

    protected override void LoadComplete()
    {
        base.LoadComplete();
        Refresh();
    }

    public void Refresh()
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
        switch (issue)
        {
            case LyricIssue lyricIssue:
                // todo: select the lyric.
                var lyric = lyricIssue.Lyric;
                clock.Seek(lyric.LyricStartTime);
                break;

            case BeatmapPageIssue beatmapPageIssue:
                // todo: select the pages.
                clock.Seek(beatmapPageIssue.StartPage.Time);
                break;

            case Issue:
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
