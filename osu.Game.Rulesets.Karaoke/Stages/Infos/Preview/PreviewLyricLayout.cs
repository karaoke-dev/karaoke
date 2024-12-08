// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

public class PreviewLyricLayout : StageElement
{
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
}
