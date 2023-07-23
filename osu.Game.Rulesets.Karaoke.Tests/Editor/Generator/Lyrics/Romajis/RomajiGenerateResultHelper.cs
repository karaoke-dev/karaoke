// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romajis;

public class RomajiGenerateResultHelper
{
    /// <summary>
    /// Convert the string format into the <see cref="RomajiGenerateResult"/>.
    /// </summary>
    /// <example>
    /// karaoke
    /// ^karaoke
    /// </example>
    /// <param name="timeTag">Origin time-tag</param>
    /// <param name="str">Generate result string format</param>
    /// <returns><see cref="RomajiGenerateResult"/>Romaji generate result.</returns>
    public static KeyValuePair<TimeTag, RomajiGenerateResult> ParseRomajiGenerateResult(TimeTag timeTag, string str)
    {
        var result = new RomajiGenerateResult
        {
            InitialRomaji = str.StartsWith("^", StringComparison.Ordinal),
            RomajiText = str.Replace("^", ""),
        };

        return new KeyValuePair<TimeTag, RomajiGenerateResult>(timeTag, result);
    }

    public static IReadOnlyDictionary<TimeTag, RomajiGenerateResult> ParseRomajiGenerateResults(IList<TimeTag> timeTags, IList<string> strings)
    {
        if (timeTags.Count != strings.Count)
            throw new InvalidOperationException();

        return parseRomajiGenerateResults(timeTags, strings).ToDictionary(k => k.Key, v => v.Value);

        static IEnumerable<KeyValuePair<TimeTag, RomajiGenerateResult>> parseRomajiGenerateResults(IList<TimeTag> timeTags, IList<string> strings)
        {
            for (int i = 0; i < timeTags.Count; i++)
            {
                yield return ParseRomajiGenerateResult(timeTags[i], strings[i]);
            }
        }
    }
}
