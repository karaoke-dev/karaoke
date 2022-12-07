// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;

public class BeatmapPageIssue : Issue
{
    public Page StartPage;

    public Page EndPage;

    public BeatmapPageIssue(Page startPage, Page endPage, IssueTemplate template, params object[] args)
        : base(template, args)
    {
        StartPage = startPage;
        EndPage = endPage;
    }
}
