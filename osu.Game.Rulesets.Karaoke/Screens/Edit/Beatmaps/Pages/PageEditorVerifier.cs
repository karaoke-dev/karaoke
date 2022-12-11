// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public partial class PageEditorVerifier : Component, IPageEditorVerifier
{
    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    [Resolved, AllowNull]
    private IBindable<WorkingBeatmap> workingBeatmap { get; set; }

    [Resolved, AllowNull]
    private EditorClock clock { get; set; }

    private readonly BindableList<Issue> issues = new();
    private readonly PageBeatmapVerifier verifier = new();

    public IBindableList<Issue> Issues => issues;

    protected override void LoadComplete()
    {
        base.LoadComplete();
        Refresh();
    }

    public void Refresh()
    {
        issues.Clear();
        issues.AddRange(getIssues());
    }

    private IEnumerable<Issue> getIssues()
    {
        var context = new BeatmapVerifierContext(beatmap, workingBeatmap.Value);
        return verifier.Run(context);
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

    private class PageBeatmapVerifier : IBeatmapVerifier
    {
        private readonly List<ICheck> checks = new()
        {
            new CheckBeatmapPageInfo(),
        };

        public IEnumerable<Issue> Run(BeatmapVerifierContext context) => checks.SelectMany(check => check.Run(context));
    }
}
