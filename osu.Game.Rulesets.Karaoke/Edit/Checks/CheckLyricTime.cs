// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckLyricTime : CheckHitObjectProperty<Lyric>
{
    protected override string Description => "Lyric with invalid time.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateTimeOverlapping(this),
        new IssueTemplateStartTimeInvalid(this),
        new IssueTemplateEndTimeInvalid(this),
    };

    protected override IEnumerable<Issue> Check(Lyric lyric)
    {
        if (LyricUtils.CheckIsTimeOverlapping(lyric))
            yield return new IssueTemplateTimeOverlapping(this).Create(lyric);

        if (LyricUtils.CheckIsStartTimeInvalid(lyric))
            yield return new IssueTemplateStartTimeInvalid(this).Create(lyric);

        if (LyricUtils.CheckIsEndTimeInvalid(lyric))
            yield return new IssueTemplateEndTimeInvalid(this).Create(lyric);
    }

    public class IssueTemplateTimeOverlapping : IssueTemplate
    {
        public IssueTemplateTimeOverlapping(ICheck check)
            : base(check, IssueType.Problem, "Lyric's start is larger than end-time.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }

    public class IssueTemplateStartTimeInvalid : IssueTemplate
    {
        public IssueTemplateStartTimeInvalid(ICheck check)
            : base(check, IssueType.Problem, "Lyric's start time is larger than time-tag's time.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }

    public class IssueTemplateEndTimeInvalid : IssueTemplate
    {
        public IssueTemplateEndTimeInvalid(ICheck check)
            : base(check, IssueType.Problem, "Lyric's end time is smaller than time-tag's time.")
        {
        }

        public Issue Create(Lyric lyric)
            => new LyricIssue(lyric, this);
    }
}
