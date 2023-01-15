// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckBeatmapClassicStageInfo;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

public partial class StageEditorVerifier : EditorVerifier<StageEditorEditCategory>, IStageEditorVerifier
{
    protected override IEnumerable<ICheck> CreateChecks(StageEditorEditCategory type) =>
        type switch
        {
            StageEditorEditCategory.Layout => new ICheck[] { new CheckBeatmapClassicStageInfo() },
            StageEditorEditCategory.Timing => new ICheck[] { new CheckBeatmapClassicStageInfo() },
            StageEditorEditCategory.Style => new ICheck[] { new CheckBeatmapClassicStageInfo() },
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    protected override StageEditorEditCategory ClassifyIssue(Issue issue)
    {
        switch (issue.Template)
        {
            case IssueTemplateInvalidRowHeight:
                // just showing this issue on this section. will go to another screen if click this issue.
                return StageEditorEditCategory.Layout;

            case IssueTemplateLessThanTwoTimingPoints:
            case IssueTemplateTimingIntervalTooShort:
            case IssueTemplateTimingIntervalTooLong:
            case IssueTemplateTimingInfoHitObjectNotExist:
            case IssueTemplateTimingInfoMappingHasNoTiming:
            case IssueTemplateTimingInfoTimingNotExist:
            case IssueTemplateTimingInfoLyricNotHaveTwoTiming:
                return StageEditorEditCategory.Timing;

            case IssueTemplateLyricLayoutInvalidLineNumber:
                return StageEditorEditCategory.Layout;

            default:
                throw new NotSupportedException();
        }
    }

    public override void Refresh()
    {
        var allIssues = CreateIssues();
        var groupByEditModeIssues = allIssues.GroupBy(ClassifyIssue).ToDictionary(x => x.Key, x => x.ToArray());

        foreach (var editorMode in Enum.GetValues<StageEditorEditCategory>())
        {
            ClearChecks(editorMode);

            if (groupByEditModeIssues.TryGetValue(editorMode, out var issues))
                AddChecks(editorMode, issues);
        }
    }

    public void Navigate(Issue issue)
    {
        // todo: should switch to another screen if got the IssueTemplateInvalidRowHeight issue.
        // todo: doing something.
    }
}
