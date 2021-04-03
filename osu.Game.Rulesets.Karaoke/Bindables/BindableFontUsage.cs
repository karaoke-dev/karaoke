// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
        }

        public override void Parse(object input)
        {
            if (!(input is string str) || string.IsNullOrEmpty(str))
            {
                Value = default;
                return;
            }

            var regex = new Regex(@"\b(?<key>family|weight|size|italics|fixedWidth)(?<op>[=]+)(?<value>("".*"")|(\S*))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var dictionary = regex.Matches(str).ToDictionary(k => k.Groups["key"].Value.ToLower(), v => v.Groups["value"].Value);

            var family = dictionary["family"];
            var weight = dictionary["weight"];
            var size = float.Parse(dictionary["size"]);
            var italics = bool.Parse(dictionary["italics"]);
            var fixedWidth = bool.Parse(dictionary["fixedWidth"]);
            Value = new FontUsage(family, size, weight, italics, fixedWidth);
        }

        public override string ToString() => $"family={Value.Family}, weight={Value.Weight}, size={Value.Size}, italics={Value.Italics}, fixedWidth={Value.FixedWidth}";
    }
}
