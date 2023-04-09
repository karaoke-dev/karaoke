// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapClassicStageChangeHandler : BeatmapPropertyChangeHandler, IBeatmapClassicStageChangeHandler
{
    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    #region Layout definition

    public void EditLayoutDefinition(Action<ClassicStageDefinition> action)
    {
        performStageInfoChanged(x =>
        {
            action(x.StageDefinition);
        });
    }

    #endregion

    #region Timing info

    public void AddTimingPoint(Action<ClassicLyricTimingPoint>? action = null)
    {
        performTimingInfoChanged(timingInfo =>
        {
            timingInfo.AddTimingPoint(action);
        });
    }

    public void RemoveTimingPoint(ClassicLyricTimingPoint timePoint)
    {
        performTimingInfoChanged(timingInfo =>
        {
            timingInfo.RemoveTimingPoint(timePoint);
        });
    }

    public void RemoveRangeOfTimingPoints(IEnumerable<ClassicLyricTimingPoint> timePoints)
    {
        performTimingInfoChanged(timingInfo =>
        {
            foreach (var timePoint in timePoints)
            {
                timingInfo.RemoveTimingPoint(timePoint);
            }
        });
    }

    public void ShiftingTimingPoints(IEnumerable<ClassicLyricTimingPoint> timePoints, double offset)
    {
        performTimingInfoChanged(timingInfo =>
        {
            foreach (var timePoint in timePoints)
            {
                timePoint.Time += offset;
            }
        });
    }

    public void AddLyricIntoTimingPoint(ClassicLyricTimingPoint timePoint)
    {
        performTimingInfoChanged(timingInfo =>
        {
            var selectedLyric = beatmap.SelectedHitObjects.OfType<Lyric>();

            foreach (var lyric in selectedLyric)
            {
                timingInfo.AddToMapping(timePoint, lyric);
            }
        });
    }

    public void RemoveLyricFromTimingPoint(ClassicLyricTimingPoint timePoint)
    {
        performTimingInfoChanged(timingInfo =>
        {
            var selectedLyric = beatmap.SelectedHitObjects.OfType<Lyric>();

            foreach (var lyric in selectedLyric)
            {
                timingInfo.RemoveFromMapping(timePoint, lyric);
            }
        });
    }

    private static bool checkTimingPointExist(ClassicLyricTimingInfo timingInfo, ClassicLyricTimingPoint timingPoint)
    {
        return timingInfo.Timings.Contains(timingPoint);
    }

    #endregion

    private void performStageInfoChanged(Action<ClassicStageInfo> action)
    {
        PerformBeatmapChanged(beatmap =>
        {
            if (beatmap.CurrentStageInfo is not ClassicStageInfo classicStageInfo)
                throw new NotSupportedException($"Current stage info in the beatmap should be {nameof(ClassicStageInfo)}");

            action(classicStageInfo);
        });
    }

    private void performTimingInfoChanged(Action<ClassicLyricTimingInfo> action)
    {
        performStageInfoChanged(stageInfo =>
        {
            action(stageInfo.LyricTimingInfo);

            InvalidateAllHitObjectWorkingProperty(LyricWorkingProperty.Timing);
        });
    }
}
