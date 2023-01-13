// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public interface IBeatmapClassicStageChangeHandler
{
    #region Layout definition

    void EditLayoutDefinition(Action<ClassicLyricLayoutDefinition> action);

    #endregion

    #region Timing info

    void AddTimingPoint(Action<ClassicLyricTimingPoint>? action = null);

    void RemoveTimingPoint(ClassicLyricTimingPoint timePoint);

    void RemoveRangeOfTimingPoints(IEnumerable<ClassicLyricTimingPoint> timePoints);

    void ShiftingTimingPoints(IEnumerable<ClassicLyricTimingPoint> timePoints, double offset);

    void AddLyricIntoTimingPoint(ClassicLyricTimingPoint timePoint);

    void RemoveLyricFromTimingPoint(ClassicLyricTimingPoint timePoint);

    #endregion
}
