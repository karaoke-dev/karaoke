// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Stages.Classic;

public class ClassicLyricTimingPoint : IDeepCloneable<ClassicLyricTimingPoint>, IComparable<ClassicLyricTimingPoint>, IHasPrimaryKey
{
    [JsonProperty]
    public ElementId ID { get; private set; } = ElementId.NewElementId();

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

    public int CompareTo(ClassicLyricTimingPoint? other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            t => t.Time,
            t => t.ID);
    }
}
