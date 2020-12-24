// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class TimeTagUtilsTest
    {
        [TestCase("[1,start]:1000", 2, "[3,start]:1000")]
        [TestCase("[1,end]:1000", 2, "[3,end]:1000")]
        [TestCase("[1,start]:", 2, "[3,start]:")]
        [TestCase("[1,end]:", 2, "[3,end]:")]
        [TestCase("[1,start]:1000", -2, "[-1,start]:1000")]
        [TestCase("[1,end]:1000", -2, "[-1,end]:1000")]
        public void TestShiftingTimeTag(string shiftingTag, int shifting, string actualTag)
        {
            var timeTag = TestCaseTagHelper.ParseTimeTag(shiftingTag);

            var shiftingTimeTag = TimeTagUtils.ShiftingTimeTag(timeTag, shifting);
            var actualTimeTag = TestCaseTagHelper.ParseTimeTag(actualTag);

            Assert.AreEqual(shiftingTimeTag.Index, actualTimeTag.Index);
            Assert.AreEqual(shiftingTimeTag.Time, actualTimeTag.Time);
        }
    }
}
