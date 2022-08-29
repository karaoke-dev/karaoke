// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    public class RomajiTagTest
    {
        [TestCase]
        public void TestClone()
        {
            var romajiTag = new RomajiTag
            {
                Text = "romaji",
                StartIndex = 1,
                EndIndex = 2
            };

            var clonedRomajiTag = romajiTag.DeepClone();

            Assert.AreNotSame(clonedRomajiTag.TextBindable, romajiTag.TextBindable);
            Assert.AreEqual(clonedRomajiTag.Text, romajiTag.Text);

            Assert.AreNotSame(clonedRomajiTag.StartIndexBindable, romajiTag.StartIndexBindable);
            Assert.AreEqual(clonedRomajiTag.StartIndex, romajiTag.StartIndex);

            Assert.AreNotSame(clonedRomajiTag.EndIndexBindable, romajiTag.EndIndexBindable);
            Assert.AreEqual(clonedRomajiTag.EndIndex, romajiTag.EndIndex);
        }
    }
}
