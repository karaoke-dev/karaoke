// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckBeatmapClassicStageInfo : CheckBeatmapStageInfo<ClassicStageInfo>
{
    public const double MIN_ROW_HEIGHT = 30;
    public const double MAX_ROW_HEIGHT = 200;

    public const int MIN_LINE_SIZE = 0;
    public const int MAX_LINE_SIZE = 4;

    protected override string Description => "Check invalid info in the classic stage info.";

    public override IEnumerable<IssueTemplate> StageTemplates => new IssueTemplate[]
    {
        new IssueTemplateInvalidRowHeight(this),
        new IssueTemplateLyricLayoutInvalidLineNumber(this)
    };

    public CheckBeatmapClassicStageInfo()
    {
        RegisterCategory(x => x.StyleCategory, 0);
        RegisterCategory(x => x.LyricLayoutCategory, 2);
    }

    public override IEnumerable<Issue> CheckStageInfo(ClassicStageInfo stageInfo)
    {
        var layoutDefinition = stageInfo.LyricLayoutDefinition;
        if (layoutDefinition.LineHeight is < MIN_ROW_HEIGHT or > MAX_ROW_HEIGHT)
            yield return new IssueTemplateInvalidRowHeight(this).Create();
    }

    protected override IEnumerable<Issue> CheckElement<TStageElement>(TStageElement element)
    {
        switch (element)
        {
            case ClassicLyricLayout classicLyricLayout:
                if (classicLyricLayout.Line is < MIN_LINE_SIZE or > MAX_LINE_SIZE)
                    yield return new IssueTemplateLyricLayoutInvalidLineNumber(this).Create();

                break;

            case ClassicStyle:
                // todo: might need to check if skin resource is exist?
                break;

            default:
                throw new InvalidOperationException("Unknown stage element type.");
        }
    }

    public class IssueTemplateInvalidRowHeight : IssueTemplate
    {
        public IssueTemplateInvalidRowHeight(ICheck check)
            : base(check, IssueType.Warning, $"Row height should be in the range of {MIN_ROW_HEIGHT} and {MAX_ROW_HEIGHT}.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateLyricLayoutInvalidLineNumber : IssueTemplate
    {
        public IssueTemplateLyricLayoutInvalidLineNumber(ICheck check)
            : base(check, IssueType.Warning, $"Line number should be in the range of {MIN_LINE_SIZE} and {MAX_LINE_SIZE}.")
        {
        }

        public Issue Create() => new(this);
    }
}
