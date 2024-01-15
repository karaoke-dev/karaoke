// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romanization;

public class RomanizationGenerateResultHelper
{
    /// <summary>
    /// Convert the string format into the <see cref="RomanizationGenerateResult"/>.
    /// </summary>
    /// <example>
    /// karaoke
    /// ^karaoke
    /// </example>
    /// <param name="timeTag">Origin time-tag</param>
    /// <param name="str">Generate result string format</param>
    /// <returns><see cref="RomanizationGenerateResult"/>Romanization generate result.</returns>
    public static KeyValuePair<TimeTag, RomanizationGenerateResult> ParseRomanizationGenerateResult(TimeTag timeTag, string str)
    {
        var result = new RomanizationGenerateResult
        {
            FirstSyllable = str.StartsWith("^", StringComparison.Ordinal),
            RomanizedSyllable = str.Replace("^", ""),
        };

        return new KeyValuePair<TimeTag, RomanizationGenerateResult>(timeTag, result);
    }

    public static IReadOnlyDictionary<TimeTag, RomanizationGenerateResult> ParseRomanizationGenerateResults(IList<TimeTag> timeTags, IList<string> strings)
    {
        if (timeTags.Count != strings.Count)
            throw new InvalidOperationException();

        return parseRomanizationGenerateResults(timeTags, strings).ToDictionary(k => k.Key, v => v.Value);

        static IEnumerable<KeyValuePair<TimeTag, RomanizationGenerateResult>> parseRomanizationGenerateResults(IList<TimeTag> timeTags, IList<string> strings)
        {
            for (int i = 0; i < timeTags.Count; i++)
            {
                yield return ParseRomanizationGenerateResult(timeTags[i], strings[i]);
            }
        }
    }
}
