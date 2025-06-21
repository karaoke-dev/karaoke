// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Integration.Formats;

public class LrcParserUtilsTest
{
    [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, new double[] { 1100, 2000, 2100, 3000 })]
    [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1100, 2000, 2100, 3000 })]
    [TestCase(new[] { "[0,start]", "[0,start]", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1100, 2000 })]
    [TestCase(new[] { "[0,start]:1000", "[0,start]:1100", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1000, 2000 })]
    [TestCase(new[] { "[0,start]", "[0,end]", "[0,start]", "[1,start]", "[1,end]" }, new double[] { })]
    [TestCase(new[] { "[0,start]:2000", "[0,end]:1000" }, new double[] { 2000, 2000 })]
    [TestCase(new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2000", "[1,end]:3000" }, new double[] { 1100, 2100, 2100, 3000 })]
    [TestCase(new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:2000", "[1,end]:3000" }, new double[] { 1000, 5000, 5000, 5000 })]
    [TestCase(new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:0", "[1,end]:3000" }, new double[] { 1000, 2000, 2000, 3000 })]
    //[TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, new double[] { 4000, 4000, 4000, 4000 })]
    public void TestToDictionary(string[] timeTagTexts, double[] expected)
    {
        var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

        double[] actual = LrcParserUtils.ToDictionary(timeTags).Values.ToArray();
        Assert.That(expected, Is.EqualTo(actual));
    }
}
