// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class RegexExtensions
    {
        public static TType GetGroupValue<TType>(this Match match, string key, bool useDefaultValueIfEmpty = true)
        {
            var value = match.Groups[key]?.Value;

            // if got empty value, should change to null.
            return TypeUtils.ChangeType<TType>(string.IsNullOrEmpty(value) ? null : value);
        }
    }
}
