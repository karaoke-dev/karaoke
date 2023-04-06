// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

public class PreviewLyricLayout : StageElement, IComparable<PreviewLyricLayout>
{
    public PreviewLyricLayout(int id)
        : base(id)
    {
    }

    /// <summary>
    /// <see cref="Lyric"/>'s timing with row index.
    /// </summary>
    public IDictionary<int, double> Timings { get; set; } = new Dictionary<int, double>();

    /// <summary>
    /// <see cref="Lyric"/>'s start time.
    /// </summary>
    public double StartTime { get; set; }

    /// <summary>
    /// <see cref="Lyric"/>'s end time.
    /// </summary>
    public double EndTime { get; set; }

    public int CompareTo(PreviewLyricLayout? other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            x => x.ID);
    }
}
