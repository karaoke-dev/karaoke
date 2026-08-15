// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Skinning.Fonts;

public readonly struct FontInfo : IEquatable<FontInfo>
{
    public string FontName { get; }

    public string Family { get; }

    public string? Weight { get; }

    public FontFormat FontFormat { get; }

    public FontInfo(string fontName, FontFormat fontFormat)
    {
        FontName = fontName;
        FontFormat = fontFormat;

        string[] parts = fontName.Split('-');

        switch (parts.Length)
        {
            case 1:
                Family = parts[0];
                Weight = null;
                break;

            default:
                Family = string.Join('-', parts.Take(parts.Length - 1));
                Weight = fontName.Split('-').LastOrDefault();
                break;
        }
    }

    // note: Family and Weight are both derived from FontName, so they are not part of the equality.
    public bool Equals(FontInfo other)
        => FontName == other.FontName && FontFormat == other.FontFormat;

    public override bool Equals(object? obj)
        => obj is FontInfo other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(FontName, (int)FontFormat);
}

public enum FontFormat
{
    Internal,

    Fnt,

    Ttf,
}
