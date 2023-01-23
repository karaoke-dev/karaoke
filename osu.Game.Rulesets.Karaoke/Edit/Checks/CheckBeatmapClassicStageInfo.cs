// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckBeatmapClassicStageInfo : CheckBeatmapStageInfo<ClassicStageInfo>
{
    public const double MIN_ROW_HEIGHT = 30;
    public const double MAX_ROW_HEIGHT = 200;

    public const int MIN_LINE_SIZE = 0;
    public const int MAX_LINE_SIZE = 4;

    public const double MIN_TIMING_INTERVAL = 3000;
    public const double MAX_TIMING_INTERVAL = 10000;

    protected override string Description => "Check invalid info in the classic stage info.";

    public override IEnumerable<IssueTemplate> StageTemplates => new IssueTemplate[]
    {
        new IssueTemplateInvalidRowHeight(this),
        new IssueTemplateLessThanTwoTimingPoints(this),
        new IssueTemplateTimingIntervalTooShort(this),
        new IssueTemplateTimingIntervalTooLong(this),
        new IssueTemplateTimingInfoHitObjectNotExist(this),
        new IssueTemplateTimingInfoMappingHasNoTiming(this),
        new IssueTemplateTimingInfoTimingNotExist(this),
        new IssueTemplateTimingInfoLyricNotHaveTwoTiming(this),
        new IssueTemplateLyricLayoutInvalidLineNumber(this)
    };

    public CheckBeatmapClassicStageInfo()
    {
        RegisterCategory(x => x.StyleCategory, 0);
        RegisterCategory(x => x.LyricLayoutCategory, 2);
    }

    public override IEnumerable<Issue> CheckStageInfo(ClassicStageInfo stageInfo, IReadOnlyList<KaraokeHitObject> hitObjects)
    {
        var issues = new List<Issue>();

        issues.AddRange(checkLyricLayoutDefinition(stageInfo.LyricLayoutDefinition));
        issues.AddRange(checkLyricTimingInfo(stageInfo.LyricTimingInfo, hitObjects.OfType<Lyric>().ToArray()));

        return issues;
    }

    private IEnumerable<Issue> checkLyricLayoutDefinition(ClassicLyricLayoutDefinition layoutDefinition)
    {
        if (layoutDefinition.LineHeight is < MIN_ROW_HEIGHT or > MAX_ROW_HEIGHT)
            yield return new IssueTemplateInvalidRowHeight(this).Create();
    }

    private IEnumerable<Issue> checkLyricTimingInfo(ClassicLyricTimingInfo timingInfo, IReadOnlyList<Lyric> hitObjects)
    {
        var timings = timingInfo.Timings;
        var mappings = timingInfo.Mappings;

        if (timings.Count < 2)
        {
            yield return new IssueTemplateLessThanTwoTimingPoints(this).Create();

            yield break;
        }

        // check timing interval.
        for (int i = 1; i < timings.Count; i++)
        {
            var previous = timings[i - 1];
            var current = timings[i];

            double previousTime = previous.Time;
            double currentTime = current.Time;

            if (currentTime - previousTime < MIN_TIMING_INTERVAL)
                yield return new IssueTemplateTimingIntervalTooShort(this).Create(previous, current);

            if (currentTime - previousTime > MAX_TIMING_INTERVAL)
                yield return new IssueTemplateTimingIntervalTooLong(this).Create(previous, current);
        }

        // check have non-matched ids.
        foreach (var mapping in mappings)
        {
            // mapping lyric should be exist.
            if (hitObjects.All(x => x.ID != mapping.Key))
                yield return new IssueTemplateTimingInfoHitObjectNotExist(this).Create();

            // mapping timing should be exist.
            if (mapping.Value.Length == 0)
                yield return new IssueTemplateTimingInfoMappingHasNoTiming(this).Create();

            // mapping timing should be exist.
            if (mapping.Value.Length != 0 && timings.All(x => !mapping.Value.Contains(x.ID)))
                yield return new IssueTemplateTimingInfoTimingNotExist(this).Create();
        }

        // check mapping roles.
        foreach (var hitObject in hitObjects)
        {
            int timingAmounts = timingInfo.GetLyricTimingPoints(hitObject).Count();

            // should have exactly 2 matched timing point in the lyric.
            if (timingAmounts != 0 && timingAmounts != 2)
                yield return new IssueTemplateTimingInfoLyricNotHaveTwoTiming(this).Create();
        }
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

    #region stage definition

    #endregion

    #region timing info

    public class IssueTemplateLessThanTwoTimingPoints : IssueTemplate
    {
        public IssueTemplateLessThanTwoTimingPoints(ICheck check)
            : base(check, IssueType.Warning, "Should have at least two timing points.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateTimingIntervalTooShort : IssueTemplate
    {
        public IssueTemplateTimingIntervalTooShort(ICheck check)
            : base(check, IssueType.Warning, "Interval between two timing points are too short.")
        {
        }

        public Issue Create(ClassicLyricTimingPoint startTimingPoint, ClassicLyricTimingPoint endTimingPoint)
            => new BeatmapClassicLyricTimingPointIssue(startTimingPoint, endTimingPoint, this);
    }

    public class IssueTemplateTimingIntervalTooLong : IssueTemplate
    {
        public IssueTemplateTimingIntervalTooLong(ICheck check)
            : base(check, IssueType.Warning, "Interval between two timing points are too long.")
        {
        }

        public Issue Create(ClassicLyricTimingPoint startTimingPoint, ClassicLyricTimingPoint endTimingPoint)
            => new BeatmapClassicLyricTimingPointIssue(startTimingPoint, endTimingPoint, this);
    }

    public class IssueTemplateTimingInfoHitObjectNotExist : IssueTemplate
    {
        public IssueTemplateTimingInfoHitObjectNotExist(ICheck check)
            : base(check, IssueType.Warning, "Maybe caused by hit-object has been deleted. Don't worry, go to the stage editor and will be easy to fix them.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateTimingInfoMappingHasNoTiming : IssueTemplate
    {
        public IssueTemplateTimingInfoMappingHasNoTiming(ICheck check)
            : base(check, IssueType.Error, "Mapping should have the timing in the value. Should be the internal error.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateTimingInfoTimingNotExist : IssueTemplate
    {
        public IssueTemplateTimingInfoTimingNotExist(ICheck check)
            : base(check, IssueType.Error, "It's caused by stage element has been deleted, but still remain the mapping data.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateTimingInfoLyricNotHaveTwoTiming : IssueTemplate
    {
        public IssueTemplateTimingInfoLyricNotHaveTwoTiming(ICheck check)
            : base(check, IssueType.Warning, "Lyric should have exactly two timing. One is for start time and another one is for end time.")
        {
        }

        public Issue Create() => new(this);
    }

    #endregion

    #region element

    public class IssueTemplateLyricLayoutInvalidLineNumber : IssueTemplate
    {
        public IssueTemplateLyricLayoutInvalidLineNumber(ICheck check)
            : base(check, IssueType.Warning, $"Line number should be in the range of {MIN_LINE_SIZE} and {MAX_LINE_SIZE}.")
        {
        }

        public Issue Create() => new(this);
    }

    #endregion
}
