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
            var actualRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(generateFixedTag(rubyTag, lyric), actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRomaji = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(generateFixedTag(romajiTag, lyric), actualRomaji);

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
            var actualRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(generateShiftingTag(rubyTag, lyric, offset), actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRomaji = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(generateShiftingTag(romajiTag, lyric, offset), actualRomaji);

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
        public void TestOutOfRange(string textTag, string lyric, bool outOfRange)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            Assert.AreEqual(TextTagUtils.OutOfRange(rubyTag, lyric), outOfRange);
        }

        [TestCase("[0,1]:ka", false)]
        [TestCase("[0,1]:", true)]
        public void TestEmptyText(string textTag, bool emptyText)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            Assert.AreEqual(TextTagUtils.EmptyText(rubyTag), emptyText);
        }

        [TestCase("[0,1]:ka", "ka(0 ~ 1)")]
        [TestCase("[0,1]:", "empty(0 ~ 1)")]
        [TestCase("[-1,1]:ka", "ka(-1 ~ 1)")]
        [TestCase("[-1,-1]:ka", "ka(-1 ~ -1)")]
        [TestCase("[-1,-2]:ka", "ka(-1 ~ -2)")] // will not fix the order in display.
        [TestCase("[2,1]:ka", "ka(2 ~ 1)")] // will not fix the order in display.
        public void TestPositionFormattedString(string textTag, string actual)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            Assert.AreEqual(TextTagUtils.PositionFormattedString(rubyTag), actual);
        }

        [TestCase("[0,1]:ka", "カラオケ", "カ")]
        [TestCase("[0,4]:karaoke", "カラオケ", "カラオケ")]
        [TestCase("[-1,0]:", "カラオケ", "")]
        [TestCase("[4,5]:", "カラオケ", "")]
        [TestCase("[4,0]:karaoke", "カラオケ", "カラオケ")] // should not have those state but still give it a value.
        [TestCase("[0,4]:karaoke", "", "")]
        [TestCase("[0,4]:karaoke", null, null)]
        public void TestGetTextFromLyric(string textTag, string lyric, string actual)
        {
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            Assert.AreEqual(TextTagUtils.GetTextFromLyric(rubyTag, lyric), actual);
        }
    }
}
