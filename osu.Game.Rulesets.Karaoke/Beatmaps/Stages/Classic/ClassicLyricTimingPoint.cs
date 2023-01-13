// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Text.Json.Serialization;
using osu.Framework.Bindables;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicLyricTimingPoint : IDeepCloneable<ClassicLyricTimingPoint>, IComparable<ClassicLyricTimingPoint>
{
    public ClassicLyricTimingPoint()
    {
    }

    public ClassicLyricTimingPoint(int id)
    {
        ID = id;
    }

    public int ID { get; protected set; }

    [JsonIgnore]
    public readonly Bindable<double> TimeBindable = new();

    public double Time
    {
        get => TimeBindable.Value;
        set => TimeBindable.Value = value;
    }

    public ClassicLyricTimingPoint DeepClone()
    {
        return new ClassicLyricTimingPoint
        {
            Time = Time,
        };
    }

    public int CompareTo(ClassicLyricTimingPoint? other) => Time.CompareTo(other?.Time);
}
