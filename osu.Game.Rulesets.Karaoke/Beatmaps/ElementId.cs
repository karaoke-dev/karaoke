// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

/// <summary>
/// As unique identifier for the elements in the <see cref="KaraokeBeatmap"/>
/// Like how <see cref="Guid"/> works.
/// </summary>
public readonly struct ElementId : IComparable, IComparable<ElementId>, IEquatable<ElementId>
{
    public static readonly ElementId Empty;

    private const int length = 7;

    private readonly string? id;

    public ElementId(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"id should not be empty", nameof(id));
        }

        if (id.Length != length)
        {
            throw new ArgumentException($"id length must be {length}.", nameof(id));
        }

        if (!checkFormat(id))
        {
            throw new ArgumentException($"id format is not correct", nameof(id));
        }

        this.id = id;
    }

    // char should be 0~9 and a~f
    private static bool checkFormat(string id)
        => id.Where(c => c is < '0' or > '9').All(c => c >= 'a' && c <= 'f');

    public static ElementId NewElementId()
    {
        // take 7 digits
        string str = Guid.NewGuid().ToString("N");
        string id = str[..length];
        return new ElementId(id);
    }

    public int CompareTo(ElementId other)
    {
        return string.Compare(id, other.id, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is not ElementId elementId)
        {
            throw new ArgumentException("Compared object should be the same type.", nameof(obj));
        }

        return CompareTo(elementId);
    }

    public bool Equals(ElementId other)
    {
        return id == other.id;
    }

    public override bool Equals(object? obj)
    {
        return obj is ElementId other && Equals(other);
    }

    public static bool operator ==(ElementId id1, ElementId id2) => id1.Equals(id2);

    public static bool operator !=(ElementId id1, ElementId id2) => !id1.Equals(id2);

    public override int GetHashCode()
    {
        return (id ?? string.Empty).GetHashCode();
    }

    public override string ToString()
    {
        return id ?? string.Empty;
    }
}
