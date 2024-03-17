// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation.Ja;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romanisation.Ja;

public class JaRomanisationGeneratorTest : BaseRomanisationGeneratorTest<JaRomanisationGenerator, JaRomanisationGeneratorConfig>
{
    [TestCase("花火大会", new[] { "[0,start]", "[3,end]" }, true)]
    [TestCase("花火大会", new[] { "[0,start]" }, true)]
    [TestCase("花火大会", new[] { "[3,end]" }, false)] // not able to generate the has no start time-tag.
    [TestCase("花火大会", new string[] { }, false)] // not able to make the romanisation if has no time-tag.
    [TestCase("", new string[] { }, false)] // not able to make the romanisation if lyric is empty.
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
    public void TestGenerate(string text, string[] timeTagStrings, string[] expectedRomanisedSyllables)
    {
        var config = GeneratorEmptyConfig();

        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = timeTags,
        };

        CheckGenerateResult(lyric, expectedRomanisedSyllables, config);
    }

    [TestCase("はなび", new[] { "[0,start]" }, new[] { "^HANA BI" })]
    [TestCase("花火大会", new[] { "[0,start]", "[2,start]", "[3,end]" }, new[] { "^HANABI", "TAIKAI", "" })]
    public void TestGenerateWithUppercase(string text, string[] timeTagStrings, string[] expectedRomanisedSyllables)
    {
        var config = GeneratorEmptyConfig(x => x.Uppercase.Value = true);

        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = timeTags,
        };

        CheckGenerateResult(lyric, expectedRomanisedSyllables, config);
    }

    [TestCase("花", new[] { "[0,start]", "[0,end]" }, new[] { "[0]:hana" }, new[] { "^hana", "" })]
    [TestCase("花火", new[] { "[0,start]", "[1,end]" }, new[] { "[0]:hana", "[1]:bi" }, new[] { "^hana bi", "" })]
    [TestCase("花火", new[] { "[0,start]", "[1,start]", "[1,end]" }, new[] { "[0]:hana", "[1]:bi" }, new[] { "^hana", "bi", "" })]
    [TestCase("花火", new[] { "[0,start]", "[0,start]", "[1,start]", "[1,end]" }, new[] { "[0]:hana", "[1]:bi" }, new[] { "^hana", "", "bi", "" })]
    [TestCase("はなび", new[] { "[0,start]", "[1,start]", "[2,start]", "[2,end]" }, new[] { "[0]:hana", "[2]:bi" }, new[] { "^hana", "", "bi", "" })]
    public void TestConvertToRomanisationGenerateResult(string text, string[] timeTagStrings, string[] romanisationParams, string[] expectedResults)
    {
        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var results = parseRomanisationGenerateResults(romanisationParams);

        var expected = RomanisationGenerateResultHelper.ParseRomanisationGenerateResults(timeTags, expectedResults);
        var actual = JaRomanisationGenerator.Convert(timeTags, results);

        AssertEqual(expected, actual);
        return;

        static JaRomanisationGenerator.RomanisationGeneratorParameter[] parseRomanisationGenerateResults(IEnumerable<string> strings)
            => strings.Select(parseRomanisationGenerateResult).ToArray();

        static JaRomanisationGenerator.RomanisationGeneratorParameter parseRomanisationGenerateResult(string str)
        {
            // because format is same as the ruby-tag testing format, so just use the ruby helper.
            var tag = TestCaseTagHelper.ParseRubyTag(str);
            return new JaRomanisationGenerator.RomanisationGeneratorParameter
            {
                StartIndex = tag.StartIndex,
                EndIndex = tag.EndIndex,
                RomanisedSyllable = tag.Text,
            };
        }
    }
}
