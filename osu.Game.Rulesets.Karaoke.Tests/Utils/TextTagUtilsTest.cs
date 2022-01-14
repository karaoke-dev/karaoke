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
        [TestCase("[0,1]:ka", "[0,1]:ka")]
        [TestCase("[0,1]:", "[0,1]:")]
        [TestCase("[0,0]:ka", "[0,0]:ka")] // ignore at same index
        [TestCase("[-1,1]:ka", "[-1,1]:ka")] // ignore negative index
        [TestCase("[3,1]:ka", "[1,3]:ka")]
        [TestCase("[3,-1]:ka", "[-1,3]:ka")] // fix but ignore negative index.
        public void TestFixTimeTagPosition(string textTag, string actualTag)
        {
            // test ruby tag.
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(TextTagUtils.FixTimeTagPosition(rubyTag), actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRomaji = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(TextTagUtils.FixTimeTagPosition(romajiTag), actualRomaji);
        }

        [TestCase("[0,1]:ka", "karaoke", 1, "[1,2]:ka")]
        [TestCase("[0,1]:", "karaoke", 1, "[1,2]:")]
        [TestCase("[0,1]:ka", "karaoke", -1, "[0,0]:ka")]
        [TestCase("[0,1]:ka", "", -1, "[0,0]:ka")]
        [TestCase("[0,1]:ka", null, -1, "[0,0]:ka")]
        [TestCase("[0,1]:ka", null, 1, "[0,0]:ka")]
        [TestCase("[1,0]:ka", "karaoke", 1, "[2,1]:ka")] // do not check order in here.
        public void TestGetShiftingIndex(string textTag, string lyric, int shifting, string actualTag)
        {
            // test ruby tag.
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(generateShiftingTag(rubyTag, lyric, shifting), actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRomaji = TestCaseTagHelper.ParseRubyTag(actualTag);
            TextTagAssert.ArePropertyEqual(generateShiftingTag(romajiTag, lyric, shifting), actualRomaji);

            static T generateShiftingTag<T>(T textTag, string lyric, int shifting) where T : ITextTag, new()
            {
                (int startIndex, int endIndex) = TextTagUtils.GetShiftingIndex(textTag, lyric, shifting);
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
        [TestCase("[-1,-2]:ka", "ka(-2 ~ -1)")]
        [TestCase("[2,1]:ka", "ka(1 ~ 2)")]
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
