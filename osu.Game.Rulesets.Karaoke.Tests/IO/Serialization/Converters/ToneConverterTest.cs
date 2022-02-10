// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class ToneConverterTest : BaseSingleConverterTest<ToneConverter>
    {
        [TestCase(1, true, "1.5")]
        [TestCase(1, false, "1.0")]
        [TestCase(0, true, "0.5")]
        [TestCase(0, false, "0.0")]
        [TestCase(-1, true, "-0.5")]
        [TestCase(-1, false, "-1.0")]
        public void TestSerialize(int scale, bool half, string json)
        {
            var tone = new Tone
            {
                Scale = scale,
                Half = half,
            };

            string expected = $"{json}";
            string actual = JsonConvert.SerializeObject(tone, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [TestCase("1.5", 1, true)]
        [TestCase("1.0", 1, false)]
        [TestCase("0.5", 0, true)]
        [TestCase("0.0", 0, false)]
        [TestCase("-0.5", -1, true)]
        [TestCase("-1.0", -1, false)]
        public void TestDeserialize(string json, int scale, bool half)
        {
            var expected = new Tone
            {
                Scale = scale,
                Half = half,
            };
            var actual = JsonConvert.DeserializeObject<Tone>($"{json}", CreateSettings());
            Assert.AreEqual(expected, actual);
        }
    }
}
