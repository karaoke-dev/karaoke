// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;

public class BeatmapClassicLyricTimingPointIssue : Issue
{
    public ClassicLyricTimingPoint StartTimingPoint;

    public ClassicLyricTimingPoint EndTimingPoint;

    public BeatmapClassicLyricTimingPointIssue(ClassicLyricTimingPoint startTimingPoint, ClassicLyricTimingPoint endTimingPoint, IssueTemplate template, params object[] args)
        : base(template, args)
    {
        StartTimingPoint = startTimingPoint;
        EndTimingPoint = endTimingPoint;

        Time = Math.Min(StartTimingPoint.Time, EndTimingPoint.Time);
    }
}
