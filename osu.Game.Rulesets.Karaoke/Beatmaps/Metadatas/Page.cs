// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

public class Page : IDeepCloneable<Page>, IComparable<Page>
{
    [JsonIgnore]
    public readonly Bindable<double> TimeBindable = new();

    public double Time
    {
        get => TimeBindable.Value;
        set => TimeBindable.Value = value;
    }

    public Page DeepClone()
    {
        return new Page
        {
            Time = Time,
        };
    }

    public int CompareTo(Page? other) => Time.CompareTo(other?.Time);

    public override int GetHashCode() => Time.GetHashCode();
}
