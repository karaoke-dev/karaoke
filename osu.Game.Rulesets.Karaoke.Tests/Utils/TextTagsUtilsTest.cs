// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils;

[TestFixture]
public class TextTagsUtilsTest
{
    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, TextTagsUtils.Sorting.Asc, new[] { "[0]:ka", "[1]:ra", "[2]:o" })]
    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, TextTagsUtils.Sorting.Desc, new[] { "[2]:o", "[1]:ra", "[0]:ka" })]
    [TestCase(new[] { "[0]:ka", "[2]:o", "[1]:ra" }, TextTagsUtils.Sorting.Asc, new[] { "[0]:ka", "[1]:ra", "[2]:o" })]
    [TestCase(new[] { "[0]:ka", "[2]:o", "[1]:ra" }, TextTagsUtils.Sorting.Desc, new[] { "[2]:o", "[1]:ra", "[0]:ka" })]
    public void TestSort(string[] textTags, TextTagsUtils.Sorting sorting, string[] expectedTextTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedTextTags);
        var actual = TextTagsUtils.Sort(TestCaseTagHelper.ParseRubyTags(textTags), sorting);
        TextTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0,6]:ka" }, "karaoke", new string[] { })]
    [TestCase(new[] { "[-1]:ka" }, "karaoke", new[] { "[-1]:ka" })]
    [TestCase(new[] { "[7]:ka" }, "karaoke", new[] { "[7]:ka" })]
    public void TestFindOutOfRange(string[] textTags, string lyric, string[] expectedTextTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedTextTags);
        var actual = TextTagsUtils.FindOutOfRange(TestCaseTagHelper.ParseRubyTags(textTags), lyric);
        TextTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, TextTagsUtils.Sorting.Asc, new string[] { })]
    [TestCase(new[] { "[0]:ka", "[2]:o", "[1]:ra" }, TextTagsUtils.Sorting.Asc, new string[] { })]
    [TestCase(new[] { "[1,0]:ka" }, TextTagsUtils.Sorting.Asc, new[] { "[1,0]:ka" })] // no need to fix the case if text-tag index is not ordered.
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra" }, TextTagsUtils.Sorting.Asc, new[] { "[1,2]:ra" })]
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra" }, TextTagsUtils.Sorting.Desc, new[] { "[0,1]:ka" })]
    [TestCase(new[] { "[0,2]:ka", "[1]:ra" }, TextTagsUtils.Sorting.Asc, new[] { "[1]:ra" })]
    [TestCase(new[] { "[0,2]:ka", "[1]:ra" }, TextTagsUtils.Sorting.Desc, new[] { "[1]:ra" })]
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" }, TextTagsUtils.Sorting.Asc, new[] { "[1,2]:ra" })]
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" }, TextTagsUtils.Sorting.Desc, new[] { "[1,2]:ra" })]
    public void TestFindOverlapping(string[] textTags, TextTagsUtils.Sorting sorting, string[] expectedTextTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedTextTags);
        var actual = TextTagsUtils.FindOverlapping(TestCaseTagHelper.ParseRubyTags(textTags), sorting);
        TextTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, new string[] { })]
    [TestCase(new string[] { }, new string[] { })]
    [TestCase(new[] { "[0]:", "[1]:ra", "[2]:o" }, new[] { "[0]:" })]
    [TestCase(new[] { "[0]:", "[1]:", "[2]:" }, new[] { "[0]:", "[1]:", "[2]:" })]
    public void TestFindEmptyText(string[] textTags, string[] expectedTextTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedTextTags);
        var actual = TextTagsUtils.FindEmptyText(TestCaseTagHelper.ParseRubyTags(textTags));
        TextTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0]:ka" }, "[0]:ka")]
    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" }, "[0,3]:karaoke")]
    public void TestCombine(string[] textTags, string expectTextTag)
    {
        var rubyTags = TestCaseTagHelper.ParseRubyTags(textTags);

        var expected = TestCaseTagHelper.ParseRubyTag(expectTextTag);
        var actual = TextTagsUtils.Combine(rubyTags);
        TextTagAssert.ArePropertyEqual(expected, actual);
    }
}
