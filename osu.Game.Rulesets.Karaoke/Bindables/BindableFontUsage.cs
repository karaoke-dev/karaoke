// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;

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
            if (!(input is string str) || string.IsNullOrEmpty(str))
            {
                Value = default;
                return;
            }

            // because FontUsage.ToString() will have "," symbol.
            str = str.Replace(",", "");
            var regex = new Regex(@"\b(?<key>font|family|weight|size|italics|fixedWidth)(?<op>[=]+)(?<value>("".*"")|(\S*))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var dictionary = regex.Matches(str).ToDictionary(k => k.Groups["key"].Value, v => v.Groups["value"].Value);

            if (dictionary.ContainsKey("Font"))
            {
                var font = dictionary["Font"];
                var family = font.Contains('-') ? font.Split('-').FirstOrDefault() : font;
                var weight = font.Contains('-') ? font.Split('-').LastOrDefault() : "";
                var size = float.Parse(dictionary["Size"]);
                var italics = dictionary["Italics"].ToLower() == "true";
                var fixedWidth = dictionary["FixedWidth"].ToLower() == "true";
                Value = new FontUsage(family, size, weight, italics, fixedWidth);
            }
            else
            {
                var family = dictionary["family"];
                var weight = dictionary["weight"];
                var size = float.Parse(dictionary["size"]);
                var italics = dictionary["italics"].ToLower() == "true";
                var fixedWidth = dictionary["fixedWidth"].ToLower() == "true";
                Value = new FontUsage(family, size, weight, italics, fixedWidth);
            }
        }

        // IDK why not being called in here while saving.
        public override string ToString() => $"family={Value.Family} weight={Value.Weight} size={Value.Size} italics={Value.Italics} fixedWidth={Value.FixedWidth}";
    }
}
