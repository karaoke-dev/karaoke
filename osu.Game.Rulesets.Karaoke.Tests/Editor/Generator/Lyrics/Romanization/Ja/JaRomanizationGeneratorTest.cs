// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romanization.Ja;

public class JaRomanizationGeneratorTest : BaseRomanizationGeneratorTest<JaRomanizationGenerator, JaRomanizationGeneratorConfig>
{
    [TestCase("花火大会", new[] { "[0,start]", "[3,end]" }, true)]
    [TestCase("花火大会", new[] { "[0,start]" }, true)]
    [TestCase("花火大会", new[] { "[3,end]" }, false)] // not able to generate the has no start time-tag.
    [TestCase("花火大会", new string[] { }, false)] // not able to make the romanization if has no time-tag.
    [TestCase("", new string[] { }, false)] // not able to make the romanization if lyric is empty.
    [TestCase("   ", new string[] { }, false)]
    [TestCase(null, new string[] { }, false)]
    public void TestCanGenerate(string text, string[] timeTagStrings, bool canGenerate)
    {
        var config = GeneratorEmptyConfig();

        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = timeTags,
        };

        CheckCanGenerate(lyric, canGenerate, config);
    }

    // the generated result is not perfect, but it's OK for now.
    [TestCase("はなび", new[] { "[0,start]" }, new[] { "^hana bi" })]
    [TestCase("花火大会", new[] { "[0,start]", "[3,end]" }, new[] { "^hanabi taikai", "" })]
    [TestCase("花火大会", new[] { "[0,start]", "[2,start]", "[3,end]" }, new[] { "^hanabi", "taikai", "" })]
    [TestCase("枯れた世界に輝く",
        new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]", "[4,start]", "[5,start]", "[6,start]", "[6,start]", "[6,start]", "[7,start]", "[7,end]" },
        new[] { "^kare", "", "ta", "sekai", "", "ni", "kagayaku", "", "", "", "" })]
    public void TestGenerate(string text, string[] timeTagStrings, string[] expectedRomanizedSyllables)
    {
        var config = GeneratorEmptyConfig();

        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = timeTags,
        };

        CheckGenerateResult(lyric, expectedRomanizedSyllables, config);
    }

    [TestCase("はなび", new[] { "[0,start]" }, new[] { "^HANA BI" })]
    [TestCase("花火大会", new[] { "[0,start]", "[2,start]", "[3,end]" }, new[] { "^HANABI", "TAIKAI", "" })]
    public void TestGenerateWithUppercase(string text, string[] timeTagStrings, string[] expectedRomanizedSyllables)
    {
        var config = GeneratorEmptyConfig(x => x.Uppercase.Value = true);

        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = timeTags,
        };

        CheckGenerateResult(lyric, expectedRomanizedSyllables, config);
    }

    [TestCase("花", new[] { "[0,start]", "[0,end]" }, new[] { "[0]:hana" }, new[] { "^hana", "" })]
    [TestCase("花火", new[] { "[0,start]", "[1,end]" }, new[] { "[0]:hana", "[1]:bi" }, new[] { "^hana bi", "" })]
    [TestCase("花火", new[] { "[0,start]", "[1,start]", "[1,end]" }, new[] { "[0]:hana", "[1]:bi" }, new[] { "^hana", "bi", "" })]
    [TestCase("花火", new[] { "[0,start]", "[0,start]", "[1,start]", "[1,end]" }, new[] { "[0]:hana", "[1]:bi" }, new[] { "^hana", "", "bi", "" })]
    [TestCase("はなび", new[] { "[0,start]", "[1,start]", "[2,start]", "[2,end]" }, new[] { "[0]:hana", "[2]:bi" }, new[] { "^hana", "", "bi", "" })]
    public void TestConvertToRomanizationGenerateResult(string text, string[] timeTagStrings, string[] romanizationParams, string[] expectedResults)
    {
        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var romanizations = parseRomanizationGenerateResults(romanizationParams);

        var expected = RomanizationGenerateResultHelper.ParseRomanizationGenerateResults(timeTags, expectedResults);
        var actual = JaRomanizationGenerator.Convert(timeTags, romanizations);

        AssertEqual(expected, actual);
    }

    /// <summary>
    /// Process test case time tag string format into <see cref="TimeTag"/>
    /// </summary>
    /// <param name="str">Time tag string format</param>
    /// <returns><see cref="RomanizationGenerateResultHelper"/>Time tag object</returns>
    private static JaRomanizationGenerator.RomanizationGeneratorParameter parseRomanizationGenerateResult(string str)
    {
        // because format is same as the text-tag testing format, so just use this helper.
        var romajiTag = TestCaseTagHelper.ParseRomajiTag(str);
        return new JaRomanizationGenerator.RomanizationGeneratorParameter
        {
            StartIndex = romajiTag.StartIndex,
            EndIndex = romajiTag.EndIndex,
            RomanizedSyllable = romajiTag.Text,
        };
    }

    private static JaRomanizationGenerator.RomanizationGeneratorParameter[] parseRomanizationGenerateResults(IEnumerable<string> strings)
        => strings.Select(parseRomanizationGenerateResult).ToArray();
}
