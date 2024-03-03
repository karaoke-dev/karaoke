// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romanisation;

public class RomanisationGenerateResultHelper
{
    /// <summary>
    /// Convert the string format into the <see cref="RomanisationGenerateResult"/>.
    /// </summary>
    /// <example>
    /// karaoke
    /// ^karaoke
    /// </example>
    /// <param name="timeTag">Origin time-tag</param>
    /// <param name="str">Generate result string format</param>
    /// <returns><see cref="RomanisationGenerateResult"/>Romanisation generate result.</returns>
    public static KeyValuePair<TimeTag, RomanisationGenerateResult> ParseRomanisationGenerateResult(TimeTag timeTag, string str)
    {
        var result = new RomanisationGenerateResult
        {
            FirstSyllable = str.StartsWith("^", StringComparison.Ordinal),
            RomanizedSyllable = str.Replace("^", ""),
        };

        return new KeyValuePair<TimeTag, RomanisationGenerateResult>(timeTag, result);
    }

    public static IReadOnlyDictionary<TimeTag, RomanisationGenerateResult> ParseRomanisationGenerateResults(IList<TimeTag> timeTags, IList<string> strings)
    {
        if (timeTags.Count != strings.Count)
            throw new InvalidOperationException();

        return parseRomanisationGenerateResults(timeTags, strings).ToDictionary(k => k.Key, v => v.Value);

        static IEnumerable<KeyValuePair<TimeTag, RomanisationGenerateResult>> parseRomanisationGenerateResults(IList<TimeTag> timeTags, IList<string> strings)
        {
            for (int i = 0; i < timeTags.Count; i++)
            {
                yield return ParseRomanisationGenerateResult(timeTags[i], strings[i]);
            }
        }
    }
}
