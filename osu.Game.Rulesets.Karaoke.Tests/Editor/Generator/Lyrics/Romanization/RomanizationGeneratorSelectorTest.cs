// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romanization;

public class RomanizationGeneratorSelectorTest : BaseLyricGeneratorSelectorTest<RomanizationGeneratorSelector, IReadOnlyDictionary<TimeTag, RomanizationGenerateResult>>
{
    [TestCase(17, "花火大会", true)]
    [TestCase(17, "我是中文", true)] // only change the language code to decide should be able to generate or not.
    [TestCase(17, "", false)] // will not able to make the romanization if lyric is empty.
    [TestCase(17, "   ", false)]
    [TestCase(17, null, false)]
    [TestCase(1028, "はなび", false)] // Should not be able to generate if language is not supported.
    public void TestCanGenerate(int lcid, string text, bool canGenerate)
    {
        var selector = CreateSelector();
        var lyric = new Lyric
        {
            Language = new CultureInfo(lcid),
            Text = text,
            TimeTags = new[]
            {
                new TimeTag(new TextIndex()),
            },
        };

        CheckCanGenerate(lyric, canGenerate, selector);
    }

    [TestCase(17, "はなび", new[] { "[0,start]" }, new[] { "^hana bi" })] // Japanese
    [TestCase(1041, "花火大会", new[] { "[0,start]", "[3,end]" }, new[] { "^hanabi taikai", "" })] // Japanese
    public void TestGenerate(int lcid, string text, string[] timeTagStrings, string[] expectedRomanizedSyllables)
    {
        var selector = CreateSelector();

        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagStrings);
        var lyric = new Lyric
        {
            Language = new CultureInfo(lcid),
            Text = text,
            TimeTags = timeTags,
        };

        var expected = RomanizationGenerateResultHelper.ParseRomanizationGenerateResults(timeTags, expectedRomanizedSyllables);
        CheckGenerateResult(lyric, expected, selector);
    }

    protected override void AssertEqual(IReadOnlyDictionary<TimeTag, RomanizationGenerateResult> expected, IReadOnlyDictionary<TimeTag, RomanizationGenerateResult> actual)
    {
        TimeTagAssert.ArePropertyEqual(expected.Select(x => x.Key).ToArray(), actual.Select(x => x.Key).ToArray());
        Assert.AreEqual(expected.Select(x => x.Value), actual.Select(x => x.Value));
    }
}
