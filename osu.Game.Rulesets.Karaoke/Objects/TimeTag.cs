// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Objects;

public class TimeTag : IDeepCloneable<TimeTag>
{
    /// <summary>
    /// Invoked when <see cref="Time"/> of this <see cref="TimeTag"/> is changed.
    /// </summary>
    public event Action? TimingChanged;

    /// <summary>
    /// Invoked when <see cref="FirstSyllable"/> or <see cref="RomanisedSyllable"/> of this <see cref="TimeTag"/> is changed.
    /// </summary>
    public event Action? SyllableChanged;

    public TimeTag(TextIndex index, double? time = null)
    {
        Index = index;
        Time = time;

        TimeBindable.ValueChanged += _ => TimingChanged?.Invoke();
        FirstSyllableBindable.ValueChanged += _ => SyllableChanged?.Invoke();
        RomanisedSyllableBindable.ValueChanged += _ => SyllableChanged?.Invoke();
    }

    /// <summary>
    /// Time tag's index.
    /// Notice that this index means index of characters.
    /// </summary>
    public TextIndex Index { get; }

    [JsonIgnore]
    public readonly Bindable<double?> TimeBindable = new();

    /// <summary>
    /// Time
    /// </summary>
    public double? Time
    {
        get => TimeBindable.Value;
        set => TimeBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<bool> FirstSyllableBindable = new();

    /// <summary>
    /// Mark if this romanised syllable is the first letter of the romanisation.
    /// </summary>
    /// <example>
    /// There's the Japanese lyric:<br/>
    /// 枯れた世界に輝く<br/>
    /// There's the romanisation:<br/>
    /// kareta sekai ni kagayaku.<br/>
    /// And it will be separated as:<br/>
    /// ka|re|ta se|kai ni ka|ga|ya|ku.<br/>
    /// If this is the first or(4th) time-tag, then this value should be true.<br/>
    /// If this ts the 2th or 3th time-tag, then this value should be false.<br/>
    /// </example>
    public bool FirstSyllable
    {
        get => FirstSyllableBindable.Value;
        set => FirstSyllableBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<string?> RomanisedSyllableBindable = new();

    /// <summary>
    /// Romanised syllable
    /// </summary>
    /// <example>
    /// Ka, ra, o, ke.
    /// </example>
    public string? RomanisedSyllable
    {
        get => RomanisedSyllableBindable.Value;
        set => RomanisedSyllableBindable.Value = value;
    }

    public TimeTag DeepClone()
    {
        return new TimeTag(Index, Time)
        {
            FirstSyllable = FirstSyllable,
            RomanisedSyllable = RomanisedSyllable,
        };
    }
}
