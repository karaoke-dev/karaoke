// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TextTagsUtilsTest
    {
        [TestCase(nameof(ValidTextTagWithSorted), TextTagsUtils.Sorting.Asc, new[] { 0, 1, 1, 2, 2, 3 })]
        [TestCase(nameof(ValidTextTagWithSorted), TextTagsUtils.Sorting.Desc, new[] { 2, 3, 1, 2, 0, 1 })]
        [TestCase(nameof(ValidTextTagWithUnsorted), TextTagsUtils.Sorting.Asc, new[] { 0, 1, 1, 2, 2, 3 })]
        [TestCase(nameof(ValidTextTagWithUnsorted), TextTagsUtils.Sorting.Desc, new[] { 2, 3, 1, 2, 0, 1 })]
        public void TestSort(string testCase, TextTagsUtils.Sorting sorting, int[] results)
        {
            var textTags = getValueByMethodName(testCase);

            var sortedTextTags = TextTagsUtils.Sort(textTags, sorting);

            for (int i = 0; i < sortedTextTags.Length; i++)
            {
                // result would be start, end, start, end...
                Assert.AreEqual(sortedTextTags[i].StartIndex, results[i * 2]);
                Assert.AreEqual(sortedTextTags[i].EndIndex, results[i * 2 + 1]);
            }
        }

        [TestCase(nameof(ValidTextTagWithSorted), TextTagsUtils.Sorting.Asc, new int[] { })]
        [TestCase(nameof(ValidTextTagWithUnsorted), TextTagsUtils.Sorting.Asc, new int[] { })]
        [TestCase(nameof(InvalidTextTagWithSameStartAndEndIndex), TextTagsUtils.Sorting.Asc, new[] { 0 })]
        [TestCase(nameof(InvalidTextTagWithWrongIndex), TextTagsUtils.Sorting.Asc, new[] { 0 })]
        [TestCase(nameof(InvalidTextTagWithNegativeIndex), TextTagsUtils.Sorting.Asc, new[] { 0 })]
        [TestCase(nameof(InvalidTextTagWithEndLargerThenNextStart), TextTagsUtils.Sorting.Asc, new[] { 1 })]
        [TestCase(nameof(InvalidTextTagWithEndLargerThenNextStart), TextTagsUtils.Sorting.Desc, new[] { 0 })]
        [TestCase(nameof(InvalidTextTagWithWrapNextTextTag), TextTagsUtils.Sorting.Asc, new[] { 1 })]
        [TestCase(nameof(InvalidTextTagWithWrapNextTextTag), TextTagsUtils.Sorting.Desc, new[] { 1 })]
        [TestCase(nameof(InvalidTextTagWithSandwichTextTag), TextTagsUtils.Sorting.Asc, new[] { 1 })]
        [TestCase(nameof(InvalidTextTagWithSandwichTextTag), TextTagsUtils.Sorting.Desc, new[] { 1 })]
        public void TestFindOverlapping(string testCase, TextTagsUtils.Sorting sorting, int[] errorIndex)
        {
            var textTags = getValueByMethodName(testCase);

            // run all and find invalid indexes.
            var invalidTextTag = TextTagsUtils.FindOverlapping(textTags, sorting);
            var invalidIndexes = invalidTextTag.Select(v => textTags.IndexOf(v)).ToArray();
            Assert.AreEqual(invalidIndexes, errorIndex);
        }

        [TestCase(new[] { "[0,1]:ka" }, "[0,1]:ka")]
        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, "[0,4]:karaoke")]
        public void TestCombine(string[] textTags, string actualTextTag)
        {
            var rubyTags = TestCaseTagHelper.ParseRubyTags(textTags);
            var actualRubyTag = TestCaseTagHelper.ParseRubyTag(actualTextTag);
            Assert.AreEqual(TextTagsUtils.Combine(rubyTags), actualRubyTag);
        }

        private RubyTag[] getValueByMethodName(string methodName)
        {
            Type thisType = GetType();
            var theMethod = thisType.GetMethod(methodName);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return theMethod.Invoke(this, null) as RubyTag[];
        }

        #region valid source

        public static RubyTag[] ValidTextTagWithSorted()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 1 },
                new RubyTag { StartIndex = 1, EndIndex = 2 },
                new RubyTag { StartIndex = 2, EndIndex = 3 }
            };

        public static RubyTag[] ValidTextTagWithUnsorted()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 1 },
                new RubyTag { StartIndex = 2, EndIndex = 3 },
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        #endregion

        #region invalid source

        public static RubyTag[] InvalidTextTagWithWrongIndex()
            => new[]
            {
                new RubyTag { StartIndex = 1, EndIndex = 0 },
            };

        public static RubyTag[] InvalidTextTagWithNegativeIndex()
            => new[]
            {
                new RubyTag { StartIndex = -1, EndIndex = 0 },
            };

        public static RubyTag[] InvalidTextTagWithSameStartAndEndIndex()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 0 }, // Same number.
            };

        public static RubyTag[] InvalidTextTagWithStartTimeExceedLyricSize()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = lyric.Length + 1 }, // Same number.
            };

        public static RubyTag[] InvalidTextTagWithEndTimeExceedLyricSize()
            => new[]
            {
                new RubyTag { StartIndex = lyric.Length + 1, EndIndex = lyric.Length + 2 }, // Same number.
            };

        public static RubyTag[] InvalidTextTagWithEndLargerThenNextStart()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 2 }, // End is larger than second start.
                new RubyTag { StartIndex = 1, EndIndex = 3 }
            };

        public static RubyTag[] InvalidTextTagWithWrapNextTextTag()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 3 }, // Wrap second text tag.
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        public static RubyTag[] InvalidTextTagWithSandwichTextTag()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 2 },
                new RubyTag { StartIndex = 1, EndIndex = 3 },
                new RubyTag { StartIndex = 2, EndIndex = 4 }
            };

        #endregion
    }
}
