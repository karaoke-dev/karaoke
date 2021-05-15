// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TextTagsUtilsTest
    {
        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" }, TextTagsUtils.Sorting.Asc, new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" })]
        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" }, TextTagsUtils.Sorting.Desc, new[] { "[2,3]:o", "[1,2]:ra", "[0,1]:ka" })]
        [TestCase(new[] { "[0,1]:ka", "[2,3]:o", "[1,2]:ra" }, TextTagsUtils.Sorting.Asc, new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" })]
        [TestCase(new[] { "[0,1]:ka", "[2,3]:o", "[1,2]:ra" }, TextTagsUtils.Sorting.Desc, new[] { "[2,3]:o", "[1,2]:ra", "[0,1]:ka" })]
        public void TestSort(string[] textTags, TextTagsUtils.Sorting sorting, string[] actualTextTags)
        {
            var sortedTextTags = TextTagsUtils.Sort(TestCaseTagHelper.ParseRubyTags(textTags), sorting);
            TextTagAssert.ArePropertyEqual(sortedTextTags, TestCaseTagHelper.ParseRubyTags(actualTextTags));
        }

        [TestCase(new[] { "[0,7]:ka" }, "karaoke", new string[] { })]
        [TestCase(new[] { "[-1,0]:ka" }, "karaoke", new[] { "[-1,0]:ka" })]
        [TestCase(new[] { "[7,8]:ka" }, "karaoke", new[] { "[7,8]:ka" })]
        public void TestFindOutOfRange(string[] textTags, string lyric, string[] actualTextTags)
        {
            var invalidTextTag = TextTagsUtils.FindOutOfRange(TestCaseTagHelper.ParseRubyTags(textTags), lyric);
            TextTagAssert.ArePropertyEqual(invalidTextTag, TestCaseTagHelper.ParseRubyTags(actualTextTags));
        }

        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o" }, TextTagsUtils.Sorting.Asc, new string[] { })]
        [TestCase(new[] { "[0,1]:ka", "[2,3]:o", "[1,2]:ra" }, TextTagsUtils.Sorting.Asc, new string[] { })]
        [TestCase(new[] { "[0,0]:ka" }, TextTagsUtils.Sorting.Asc, new[] { "[0,0]:ka" })]
        [TestCase(new[] { "[1,0]:ka" }, TextTagsUtils.Sorting.Asc, new[] { "[1,0]:ka" })]
        [TestCase(new[] { "[0,2]:ka", "[1,3]:ra" }, TextTagsUtils.Sorting.Asc, new[] { "[1,3]:ra" })]
        [TestCase(new[] { "[0,2]:ka", "[1,3]:ra" }, TextTagsUtils.Sorting.Desc, new[] { "[0,2]:ka" })]
        [TestCase(new[] { "[0,3]:ka", "[1,2]:ra" }, TextTagsUtils.Sorting.Asc, new[] { "[1,2]:ra" })]
        [TestCase(new[] { "[0,3]:ka", "[1,2]:ra" }, TextTagsUtils.Sorting.Desc, new[] { "[1,2]:ra" })]
        [TestCase(new[] { "[0,2]:ka", "[1,3]:ra", "[2,4]:o" }, TextTagsUtils.Sorting.Asc, new[] { "[1,3]:ra" })]
        [TestCase(new[] { "[0,2]:ka", "[1,3]:ra", "[2,4]:o" }, TextTagsUtils.Sorting.Desc, new[] { "[1,3]:ra" })]
        public void TestFindOverlapping(string[] textTags, TextTagsUtils.Sorting sorting, string[] actualTextTags)
        {
            var invalidTextTag = TextTagsUtils.FindOverlapping(TestCaseTagHelper.ParseRubyTags(textTags), sorting);
            TextTagAssert.ArePropertyEqual(invalidTextTag, TestCaseTagHelper.ParseRubyTags(actualTextTags));
        }

        [TestCase(new[] { "[0,1]:ka" }, "[0,1]:ka")]
        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, "[0,4]:karaoke")]
        public void TestCombine(string[] textTags, string actualTextTag)
        {
            var rubyTags = TestCaseTagHelper.ParseRubyTags(textTags);
            var actualRubyTag = TestCaseTagHelper.ParseRubyTag(actualTextTag);
            TextTagAssert.ArePropertyEqual(TextTagsUtils.Combine(rubyTags), actualRubyTag);
        }
    }
}
