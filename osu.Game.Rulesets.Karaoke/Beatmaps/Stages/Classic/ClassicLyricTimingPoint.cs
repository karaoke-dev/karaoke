// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicLyricTimingPoint : IDeepCloneable<ClassicLyricTimingPoint>, IComparable<ClassicLyricTimingPoint>
{
    [JsonIgnore]
    public readonly Bindable<double> TimeBindable = new();

    public double Time
    {
        get => TimeBindable.Value;
        set => TimeBindable.Value = value;
    }

    [JsonIgnore]
    public BindableList<int> LyricIdsBindable = new();

    public IList<int> LyricIds
    {
        get => LyricIdsBindable;
        set
        {
            LyricIdsBindable.Clear();
            LyricIdsBindable.AddRange(value);
        }
    }

    public double? GetLyricTime(Lyric lyric)
    {
        if (LyricIds.Contains(lyric.ID))
            return Time;

        return null;
    }

    public ClassicLyricTimingPoint DeepClone()
    {
        return new ClassicLyricTimingPoint
        {
            Time = Time,
            LyricIds = LyricIds,
        };
    }

    public int CompareTo(ClassicLyricTimingPoint? other) => Time.CompareTo(other?.Time);
}
