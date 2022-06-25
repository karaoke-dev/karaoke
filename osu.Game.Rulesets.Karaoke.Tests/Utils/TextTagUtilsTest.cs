// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TextTagUtilsTest
    {
        [TestCase("[0,1]:ka", "karaoke", "[0,1]:ka")]
        [TestCase("[0,1]:", "karaoke", "[0,1]:")]
        [TestCase("[0,0]:ka", "karaoke", "[0,0]:ka")] // ignore at same index
        [TestCase("[-1,1]:ka", "karaoke", "[0,1]:ka")]
        [TestCase("[3,1]:ka", "karaoke", "[1,3]:ka")]
        [TestCase("[3,-1]:ka", "karaoke", "[0,3]:ka")]
        public void TestGetFixedIndex(string textTag, string lyric, string actualTag)
        {
            // test ruby tag.
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            var expectedRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            var actualRubyTag = generateFixedTag(rubyTag, lyric);
            TextTagAssert.ArePropertyEqual(expectedRubyTag, actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);

            var expectedRomajiTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            var actualRomajiTag = generateFixedTag(romajiTag, lyric);
            TextTagAssert.ArePropertyEqual(expectedRomajiTag, actualRomajiTag);

            static T generateFixedTag<T>(T textTag, string lyric) where T : ITextTag, new()
            {
                (int startIndex, int endIndex) = TextTagUtils.GetFixedIndex(textTag, lyric);
                return new T
                {
                    Text = textTag.Text,
                    StartIndex = startIndex,
                    EndIndex = endIndex
                };
            }
        }

        [TestCase("[0,1]:ka", "karaoke", 1, "[1,2]:ka")]
        [TestCase("[0,1]:", "karaoke", 1, "[1,2]:")]
        [TestCase("[0,1]:ka", "karaoke", -1, "[0,0]:ka")]
        [TestCase("[0,1]:ka", "", -1, "[0,0]:ka")]
        [TestCase("[0,1]:ka", null, -1, "[0,0]:ka")]
        [TestCase("[0,1]:ka", null, 1, "[0,0]:ka")]
        [TestCase("[1,0]:ka", "karaoke", 0, "[0,1]:ka")] // will auto fix the position
        [TestCase("[1,0]:ka", "karaoke", 1, "[1,2]:ka")]
        public void TestGetShiftingIndex(string textTag, string lyric, int offset, string actualTag)
        {
            // test ruby tag.
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            var expectedRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            var actualRubyTag = generateShiftingTag(rubyTag, lyric, offset);
            TextTagAssert.ArePropertyEqual(expectedRubyTag, actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);

            var expectedRomajiTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            var actualRomajiTag = generateShiftingTag(romajiTag, lyric, offset);
            TextTagAssert.ArePropertyEqual(expectedRomajiTag, actualRomajiTag);

            static T generateShiftingTag<T>(T textTag, string lyric, int offset) where T : ITextTag, new()
            {
                (int startIndex, int endIndex) = TextTagUtils.GetShiftingIndex(textTag, lyric, offset);
                return new T
                {
                    Text = textTag.Text,
                    StartIndex = startIndex,
                    EndIndex = endIndex
                };
            }
        }

        [TestCase("[0,1]:ka", "karaoke", false)]
        [TestCase("[0,1]:ka", "", true)]
        [TestCase("[0,1]:ka", null, true)]
        [TestCase("[0,-1]:ka", "karaoke", true)]
        [TestCase("[1,0]:ka", "karaoke", false)] // should not be counted as out of range if index is not ordered.
        [TestCase("[0,0]:ka", "", true)] // should be counted as out of range if lyric is empty
        [TestCase("[0,0]:ka", null, true)] // should be counted as out of range if lyric is null
        public void TestOutOfRange(string textTag, string lyric, bool expected)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            bool actual = TextTagUtils.OutOfRange(rubyTag, lyric);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[0,1]:ka", 0, true)]
        [TestCase("[0,1]:ka", 1, false)]
        [TestCase("[0,1]:ka", -1, true)] // should be ok with negative value because we only check if valid with current text-tag index.
        [TestCase("[2,1]:ka", 0, true)]
        [TestCase("[2,1]:ka", 1, false)]
        [TestCase("[2,1]:ka", 2, false)]
        public void TestValidNewStartIndex(string textTag, int newStartIndex, bool expected)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            bool actual = TextTagUtils.ValidNewStartIndex(rubyTag, newStartIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[0,1]:ka", 1, true)]
        [TestCase("[0,1]:ka", 0, false)]
        [TestCase("[0,1]:ka", 1000, true)] // should be ok with large value because we only check if valid with current text-tag index.
        [TestCase("[2,1]:ka", 0, false)]
        [TestCase("[2,1]:ka", 1, false)]
        [TestCase("[2,1]:ka", 2, false)]
        [TestCase("[2,1]:ka", 3, true)]
        public void TestValidNewEndIndex(string textTag, int newEndIndex, bool expected)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            bool actual = TextTagUtils.ValidNewEndIndex(rubyTag, newEndIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("karaoke", 0, false)]
        [TestCase("karaoke", 7, false)]
        [TestCase("karaoke", -1, true)]
        [TestCase("karaoke", 8, true)]
        [TestCase("", -1, true)]
        [TestCase("", 0, true)]
        [TestCase("", 1, true)]
        public void TestOutOfRange(string lyric, int index, bool expected)
        {
            bool actual = TextTagUtils.OutOfRange(lyric, index);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[0,1]:ka", false)]
        [TestCase("[0,1]:", true)]
        public void TestEmptyText(string textTag, bool expected)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            bool actual = TextTagUtils.EmptyText(rubyTag);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[0,1]:ka", "ka(0 ~ 1)")]
        [TestCase("[0,1]:", "empty(0 ~ 1)")]
        [TestCase("[-1,1]:ka", "ka(-1 ~ 1)")]
        [TestCase("[-1,-1]:ka", "ka(-1 ~ -1)")]
        [TestCase("[-1,-2]:ka", "ka(-1 ~ -2)")] // will not fix the order in display.
        [TestCase("[2,1]:ka", "ka(2 ~ 1)")] // will not fix the order in display.
        public void TestPositionFormattedString(string textTag, string expected)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            string actual = TextTagUtils.PositionFormattedString(rubyTag);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[0,1]:ka", "カラオケ", "カ")]
        [TestCase("[0,4]:karaoke", "カラオケ", "カラオケ")]
        [TestCase("[-1,0]:", "カラオケ", "")]
        [TestCase("[4,5]:", "カラオケ", "")]
        [TestCase("[4,0]:karaoke", "カラオケ", "カラオケ")] // should not have those state but still give it a value.
        [TestCase("[0,4]:karaoke", "", "")]
        public void TestGetTextFromLyric(string textTag, string lyric, string expected)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);

            string actual = TextTagUtils.GetTextFromLyric(rubyTag, lyric);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[0,1]:ka")]
        [TestCase("[1,0]:ka")] // Should be able to convert even if time-tag is invalid.
        [TestCase("[-1,1]:ka")] // Should be able to convert even if time-tag is invalid.
        public void TestToPositionText(string textTag)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actual = TextTagUtils.ToPositionText(rubyTag);

            Assert.AreEqual(rubyTag.Text, actual.Text);
            Assert.AreEqual(rubyTag.StartIndex, actual.StartIndex);
            Assert.AreEqual(rubyTag.EndIndex, actual.EndIndex);
        }
    }
}
