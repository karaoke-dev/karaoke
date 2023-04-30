// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Objects;

public struct Tone : IComparable<Tone>, IComparable<int>, IEquatable<Tone>, IEquatable<int>
{
    public int Scale { get; set; }

    public bool Half { get; set; }

    public Tone(int scale, bool half = false)
    {
        Scale = scale;
        Half = half;
    }

    public int CompareTo(Tone other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            t => t.Scale,
            t => t.Half);
    }

    public int CompareTo(int other)
    {
        return CompareTo(new Tone(other));
    }

    public bool Equals(Tone other) => Scale == other.Scale && Half == other.Half;

    public bool Equals(int other) => Scale == other && Half == false;

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Tone tone => Equals(tone),
            int intValue => Equals(intValue),
            _ => false
        };
    }

    public override int GetHashCode() => HashCode.Combine(Scale, Half);

    public static Tone operator +(Tone left, Tone right) => add(left, right);

    public static Tone operator +(Tone tone1, int scale) => tone1 + new Tone { Scale = scale };

    private static Tone add(Tone tone1, Tone tone2) => new()
    {
        Scale = tone1.Scale + tone2.Scale + (tone1.Half && tone2.Half ? 1 : 0),
        Half = tone1.Half ^ tone2.Half
    };

    public static Tone operator -(Tone tone1, Tone tone2) => subtract(tone1, tone2);

    public static Tone operator -(Tone tone1, int scale) => tone1 - new Tone { Scale = scale };

    private static Tone subtract(Tone tone1, Tone tone2) => tone1 + -tone2;

    public static Tone operator -(Tone tone) => negate(tone);

    private static Tone negate(Tone tone) => tone with
    {
        Scale = -tone.Scale + (tone.Half ? -1 : 0)
    };

    public static bool operator ==(Tone tone1, Tone tone2) => tone1.Equals(tone2);

    public static bool operator !=(Tone tone1, Tone tone2) => !tone1.Equals(tone2);

    public static bool operator ==(Tone tone1, int tone2) => tone1.Equals(tone2);

    public static bool operator !=(Tone tone1, int tone2) => !tone1.Equals(tone2);

    public static bool operator >(Tone tone1, Tone tone2) => tone1.CompareTo(tone2) > 0;

    public static bool operator >=(Tone tone1, Tone tone2) => tone1.CompareTo(tone2) >= 0;

    public static bool operator <(Tone tone1, Tone tone2) => tone1.CompareTo(tone2) < 0;

    public static bool operator <=(Tone tone1, Tone tone2) => tone1.CompareTo(tone2) <= 0;

    public static bool operator >(Tone tone1, int tone2) => tone1.CompareTo(tone2) > 0;

    public static bool operator >=(Tone tone1, int tone2) => tone1.CompareTo(tone2) >= 0;

    public static bool operator <(Tone tone1, int tone2) => tone1.CompareTo(tone2) < 0;

    public static bool operator <=(Tone tone1, int tone2) => tone1.CompareTo(tone2) <= 0;

    public override string ToString() => $"Scale:{Scale}, Half:{Half}";
}
