// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
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
            Assert.AreEqual(TextTagUtils.FixTimeTagPosition(rubyTag), actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRomaji = TestCaseTagHelper.ParseRubyTag(actualTag);
            Assert.AreEqual(TextTagUtils.FixTimeTagPosition(romajiTag), actualRomaji);
        }

        [TestCase("[0,1]:ka", 1, "[1,2]:ka")]
        [TestCase("[0,1]:", 1, "[1,2]:")]
        [TestCase("[0,1]:ka", -1, "[-1,0]:ka")] // do not check out of range in here.
        [TestCase("[1,0]:ka", 1, "[2,1]:ka")] // do not check order in here.
        public void TestShifting(string textTag, int shifting, string actualTag)
        {
            // test ruby tag.
            var rubyTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRubyTag = TestCaseTagHelper.ParseRubyTag(actualTag);
            Assert.AreEqual(TextTagUtils.Shifting(rubyTag, shifting), actualRubyTag);

            // test romaji tag.
            var romajiTag = TestCaseTagHelper.ParseRubyTag(textTag);
            var actualRomaji = TestCaseTagHelper.ParseRubyTag(actualTag);
            Assert.AreEqual(TextTagUtils.Shifting(romajiTag, shifting), actualRomaji);
        }
    }
}
