// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Utils;

[TestFixture]
public class RubyTagsUtilsTest
{
    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, RubyTagsUtils.Sorting.Asc, new[] { "[0]:ka", "[1]:ra", "[2]:o" })]
    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, RubyTagsUtils.Sorting.Desc, new[] { "[2]:o", "[1]:ra", "[0]:ka" })]
    [TestCase(new[] { "[0]:ka", "[2]:o", "[1]:ra" }, RubyTagsUtils.Sorting.Asc, new[] { "[0]:ka", "[1]:ra", "[2]:o" })]
    [TestCase(new[] { "[0]:ka", "[2]:o", "[1]:ra" }, RubyTagsUtils.Sorting.Desc, new[] { "[2]:o", "[1]:ra", "[0]:ka" })]
    public void TestSort(string[] rubyTags, RubyTagsUtils.Sorting sorting, string[] expectedRubyTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedRubyTags);
        var actual = RubyTagsUtils.Sort(TestCaseTagHelper.ParseRubyTags(rubyTags), sorting);
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0,6]:ka" }, "karaoke", new string[] { })]
    [TestCase(new[] { "[-1]:ka" }, "karaoke", new[] { "[-1]:ka" })]
    [TestCase(new[] { "[7]:ka" }, "karaoke", new[] { "[7]:ka" })]
    public void TestFindOutOfRange(string[] rubyTags, string lyric, string[] expectedRubyTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedRubyTags);
        var actual = RubyTagsUtils.FindOutOfRange(TestCaseTagHelper.ParseRubyTags(rubyTags), lyric);
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, RubyTagsUtils.Sorting.Asc, new string[] { })]
    [TestCase(new[] { "[0]:ka", "[2]:o", "[1]:ra" }, RubyTagsUtils.Sorting.Asc, new string[] { })]
    [TestCase(new[] { "[1,0]:ka" }, RubyTagsUtils.Sorting.Asc, new[] { "[1,0]:ka" })] // no need to fix the case if text-tag index is not ordered.
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra" }, RubyTagsUtils.Sorting.Asc, new[] { "[1,2]:ra" })]
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra" }, RubyTagsUtils.Sorting.Desc, new[] { "[0,1]:ka" })]
    [TestCase(new[] { "[0,2]:ka", "[1]:ra" }, RubyTagsUtils.Sorting.Asc, new[] { "[1]:ra" })]
    [TestCase(new[] { "[0,2]:ka", "[1]:ra" }, RubyTagsUtils.Sorting.Desc, new[] { "[1]:ra" })]
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" }, RubyTagsUtils.Sorting.Asc, new[] { "[1,2]:ra" })]
    [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" }, RubyTagsUtils.Sorting.Desc, new[] { "[1,2]:ra" })]
    public void TestFindOverlapping(string[] rubyTags, RubyTagsUtils.Sorting sorting, string[] expectedRubyTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedRubyTags);
        var actual = RubyTagsUtils.FindOverlapping(TestCaseTagHelper.ParseRubyTags(rubyTags), sorting);
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o" }, new string[] { })]
    [TestCase(new string[] { }, new string[] { })]
    [TestCase(new[] { "[0]:", "[1]:ra", "[2]:o" }, new[] { "[0]:" })]
    [TestCase(new[] { "[0]:", "[1]:", "[2]:" }, new[] { "[0]:", "[1]:", "[2]:" })]
    public void TestFindEmptyText(string[] rubyTags, string[] expectedRubyTags)
    {
        var expected = TestCaseTagHelper.ParseRubyTags(expectedRubyTags);
        var actual = RubyTagsUtils.FindEmptyText(TestCaseTagHelper.ParseRubyTags(rubyTags));
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }

    [TestCase(new[] { "[0]:ka" }, "[0]:ka")]
    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" }, "[0,3]:karaoke")]
    public void TestCombine(string[] tags, string expectRubyTag)
    {
        var rubyTags = TestCaseTagHelper.ParseRubyTags(tags);

        var expected = TestCaseTagHelper.ParseRubyTag(expectRubyTag);
        var actual = RubyTagsUtils.Combine(rubyTags);
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }
}
