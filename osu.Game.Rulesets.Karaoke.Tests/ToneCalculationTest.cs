// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class ToneCalculationTest
    {
        [Test]
        public void TestToneCalculation()
        {
            var tone1 = new Tone
            {
                Scale = 3,
                Half = true,
            };

            var tone2 = new Tone
            {
                Scale = 1,
                Half = true
            };

            Assert.AreEqual(tone1 + tone2, new Tone { Scale = 5 });
            Assert.AreEqual(tone1 + tone2, 5);

            Assert.AreEqual(tone1 - tone2, new Tone { Scale = 2 });
            Assert.AreEqual(tone1 - tone2, 2);

            Assert.AreEqual(tone1 == tone2, false);
            Assert.AreEqual(tone1 != tone2, true);

            Assert.AreEqual(tone1 == 3, false);
            Assert.AreEqual(tone1 != 3, true);

            Assert.AreEqual(tone1 > tone2, true);
            Assert.AreEqual(tone1 >= tone2, true);

            Assert.AreEqual(tone1 < tone2, false);
            Assert.AreEqual(tone1 <= tone2, false);

            Assert.AreEqual(tone1 > 1, true);
            Assert.AreEqual(tone1 >= 1, true);

            Assert.AreEqual(tone1 < 1, false);
            Assert.AreEqual(tone1 <= 1, false);

            Assert.AreEqual(-tone1, new Tone { Scale = -4, Half = true });
        }

        [Test]
        public void TestToneComparison()
        {
            var tone1 = new Tone
            {
                Scale = 0,
                Half = true,
            };

            var tone2 = new Tone
            {
                Scale = -1,
                Half = true
            };

            Assert.AreEqual(tone1 > 0, true);
            Assert.AreEqual(tone1 > 1, false);
            Assert.AreEqual(tone1 > tone2, true);
            Assert.AreEqual(tone2 > 0, false);
            Assert.AreEqual(tone2 > -1, true);

            Assert.AreEqual(tone1 < 0, false);
            Assert.AreEqual(tone1 < 1, true);
            Assert.AreEqual(tone1 < tone2, false);
            Assert.AreEqual(tone2 < 0, true);
            Assert.AreEqual(tone2 < -1, false);
        }
    }
}
