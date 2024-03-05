// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;

namespace osu.Game.Rulesets.Karaoke.Bindables;

public class BindableFontUsage : RangeConstrainedBindable<FontUsage>
{
    private const int default_min_font_size = 1;
    private const int default_max_font_size = 72;

    protected override FontUsage DefaultMinValue => Default.With(size: default_min_font_size);
    protected override FontUsage DefaultMaxValue => Default.With(size: default_max_font_size);

    public BindableFontUsage(FontUsage value = default)
        : base(value)
    {
    }

    public float MinFontSize
    {
        get => MinValue.Size;
        set => MinValue = MinValue.With(size: value);
    }

    public float MaxFontSize
    {
        get => MaxValue.Size;
        set => MaxValue = MaxValue.With(size: value);
    }

    // IDK why not being called in here while saving.
    public override string ToString(string? format, IFormatProvider? formatProvider)
        => $"family={Value.Family} weight={Value.Weight} size={Value.Size} italics={Value.Italics} fixedWidth={Value.FixedWidth}";

    public override void Parse(object? input, IFormatProvider provider)
    {
        if (input is not string str || string.IsNullOrEmpty(str))
        {
            Value = default;
            return;
        }

        // because FontUsage.ToString() will have "," symbol.
        str = str.Replace(",", string.Empty);
        var regex = new Regex(@"\b(?<key>font|family|weight|size|italics|fixedWidth)(?<op>[=]+)(?<value>("".*"")|(\S*))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var dictionary = regex.Matches(str).ToDictionary(k => k.GetGroupValue<string>("key"), v => v.GetGroupValue<string>("value"));

        if (dictionary.TryGetValue("Font", out string? font))
        {
            string? family = font.Contains('-') ? font.Split('-').FirstOrDefault() : font;
            string? weight = font.Contains('-') ? font.Split('-').LastOrDefault() : string.Empty;
            float size = float.Parse(dictionary["Size"]);
            bool italics = dictionary["Italics"].ToLower() == "true";
            bool fixedWidth = dictionary["FixedWidth"].ToLower() == "true";
            Value = new FontUsage(family, size, weight, italics, fixedWidth);
        }
        else
        {
            string family = dictionary["family"];
            string weight = dictionary["weight"];
            float size = float.Parse(dictionary["size"]);
            bool italics = dictionary["italics"].ToLower() == "true";
            bool fixedWidth = dictionary["fixedWidth"].ToLower() == "true";
            Value = new FontUsage(family, size, weight, italics, fixedWidth);
        }
    }

    protected override Bindable<FontUsage> CreateInstance() => new BindableFontUsage();

    protected sealed override FontUsage ClampValue(FontUsage value, FontUsage minValue, FontUsage maxValue)
    {
        return value.With(size: Math.Clamp(value.Size, minValue.Size, maxValue.Size));
    }

    protected sealed override bool IsValidRange(FontUsage min, FontUsage max)
        => min.Size <= max.Size;
}
