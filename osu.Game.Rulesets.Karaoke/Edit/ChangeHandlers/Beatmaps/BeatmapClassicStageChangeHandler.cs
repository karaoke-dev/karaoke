// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapClassicStageChangeHandler : BeatmapPropertyChangeHandler, IBeatmapClassicStageChangeHandler
{
    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    #region Layout definition

    public void EditLayoutDefinition(Action<ClassicLyricLayoutDefinition> action)
    {
        performStageInfoChanged(x =>
        {
            action(x.LyricLayoutDefinition);
        });
    }

    #endregion

    #region Timing info

    public void AddTimingPoint(ClassicLyricTimingPoint timePoint)
    {
        performTimingInfoChanged(timingInfo =>
        {
            if (checkTimingPointExist(timingInfo, timePoint))
                throw new InvalidOperationException($"Should not add duplicated {nameof(timePoint)} into the {nameof(timingInfo)}.");

            timingInfo.Timings.Add(timePoint);
        });
    }

    public void RemoveTimingPoint(ClassicLyricTimingPoint timePoint)
    {
        performTimingInfoChanged(timingInfo =>
        {
            if (!checkTimingPointExist(timingInfo, timePoint))
                throw new InvalidOperationException($"{nameof(timePoint)} does ont in the {nameof(timingInfo)}.");

            timingInfo.Timings.Remove(timePoint);
        });
    }

    public void RemoveRangeOfTimingPoints(IEnumerable<ClassicLyricTimingPoint> timePoints)
    {
        performTimingInfoChanged(timingInfo =>
        {
            foreach (var timePoint in timePoints)
            {
                if (!checkTimingPointExist(timingInfo, timePoint))
                    throw new InvalidOperationException($"{nameof(timePoint)} does ont in the {nameof(timingInfo)}.");

                timingInfo.Timings.Remove(timePoint);
            }
        });
    }

    public void ShiftingTimingPoints(IEnumerable<ClassicLyricTimingPoint> timePoints, double offset)
    {
        performTimingInfoChanged(timingInfo =>
        {
            foreach (var timePoint in timePoints)
            {
                if (!checkTimingPointExist(timingInfo, timePoint))
                    throw new InvalidOperationException($"{nameof(timePoint)} does ont in the {nameof(timingInfo)}.");

                timePoint.Time += offset;
            }
        });
    }

    public void AddLyricIntoTimingPoint(ClassicLyricTimingPoint timePoint)
    {
        performTimingInfoChanged(timingInfo =>
        {
            if (!checkTimingPointExist(timingInfo, timePoint))
                throw new InvalidOperationException($"{nameof(timePoint)} does ont in the {nameof(timingInfo)}.");

            var selectedLyric = beatmap.SelectedHitObjects.OfType<Lyric>();

            foreach (var lyric in selectedLyric)
            {
                if (timePoint.LyricIds.Contains(lyric.ID))
                    continue;

                timePoint.LyricIds.Add(lyric.ID);
            }
        });
    }

    public void RemoveLyricFromTimingPoint(ClassicLyricTimingPoint timePoint)
    {
        performTimingInfoChanged(timingInfo =>
        {
            if (!checkTimingPointExist(timingInfo, timePoint))
                throw new InvalidOperationException($"{nameof(timePoint)} does ont in the {nameof(timingInfo)}.");

            var selectedLyric = beatmap.SelectedHitObjects.OfType<Lyric>();

            foreach (var lyric in selectedLyric)
            {
                timePoint.LyricIds.Remove(lyric.ID);
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
            var stage = getStageInfo(beatmap);
            action(stage);
        });

        ClassicStageInfo getStageInfo(KaraokeBeatmap beatmap)
            => beatmap.StageInfos.OfType<ClassicStageInfo>().First();
    }

    private void performTimingInfoChanged(Action<ClassicLyricTimingInfo> action)
    {
        performStageInfoChanged(stageInfo =>
        {
            action(stageInfo.LyricTimingInfo);
        });
    }
}
