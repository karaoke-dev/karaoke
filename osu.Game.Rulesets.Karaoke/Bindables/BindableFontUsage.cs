// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;

namespace osu.Game.Rulesets.Karaoke.Bindables
{
    public class BindableFontUsage : Bindable<FontUsage>
    {
        public BindableFontUsage(FontUsage value = default)
            : base(value)
        {
            MinFontSize = DefaultMinFontSize;
            MaxFontSize = DefaultMaxFontSize;
        }

        public float MinFontSize { get; set; }
        public float MaxFontSize { get; set; }

        protected float DefaultMinFontSize => 0;

        protected float DefaultMaxFontSize => 200;

        public override FontUsage Value
        {
            get => base.Value;
            set => base.Value = value.With(size: Math.Clamp(value.Size, MinFontSize, MaxFontSize));
        }

        public override void BindTo(Bindable<FontUsage> them)
        {
            if (them is BindableFontUsage other)
            {
                MinFontSize = Math.Max(MinFontSize, other.MinFontSize);
                MaxFontSize = Math.Min(MaxFontSize, other.MaxFontSize);
            }

            base.BindTo(them);
        }

        public override void Parse(object input)
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

            if (dictionary.ContainsKey("Font"))
            {
                string font = dictionary["Font"];
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

        // IDK why not being called in here while saving.
        public override string ToString() => $"family={Value.Family} weight={Value.Weight} size={Value.Size} italics={Value.Italics} fixedWidth={Value.FixedWidth}";
    }
}
