// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    public class TimeTagTest
    {
        [TestCase]
        public void TestClone()
        {
            var timeTag = new TimeTag(new TextIndex(1, TextIndex.IndexState.End), 1000);

            var clonedTimeTag = timeTag.DeepClone();

            Assert.AreEqual(clonedTimeTag.Index, timeTag.Index);

            Assert.AreNotSame(clonedTimeTag.TimeBindable, timeTag.TimeBindable);
            Assert.AreEqual(clonedTimeTag.Time, timeTag.Time);
        }
    }
}
