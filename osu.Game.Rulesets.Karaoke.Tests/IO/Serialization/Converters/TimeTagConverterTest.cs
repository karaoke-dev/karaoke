// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    [TestFixture]
    public class TimeTagConverterTest : BaseSingleConverterTest<TimeTagConverter>
    {
        [TestCase(1, TextIndex.IndexState.Start, 1000, "[1,start]:1000")]
        [TestCase(1, TextIndex.IndexState.End, 1000, "[1,end]:1000")]
        [TestCase(-1, TextIndex.IndexState.Start, 1000, "[-1,start]:1000")] // Should not check index is out of range in here.
        [TestCase(1, TextIndex.IndexState.Start, -1000, "[1,start]:-1000")] // Should not check if time is negative.
        [TestCase(1, TextIndex.IndexState.Start, null, "[1,start]:")]
        public void TestSerialize(int index, TextIndex.IndexState state, int? time, string json)
        {
            var timeTag = new TimeTag(new TextIndex(index, state), time);

            string expected = $"\"{json}\"";
            string actual = JsonConvert.SerializeObject(timeTag, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[1,start]:1000", 1, TextIndex.IndexState.Start, 1000)]
        [TestCase("[1,end]:1000", 1, TextIndex.IndexState.End, 1000)]
        [TestCase("[-1,start]:1000", -1, TextIndex.IndexState.Start, 1000)] // Should not check index is out of range in here.
        [TestCase("[1,start]:-1000", 1, TextIndex.IndexState.Start, -1000)] // Should not check if time is negative.
        [TestCase("[1,start]:", 1, TextIndex.IndexState.Start, null)]
        [TestCase("", 0, TextIndex.IndexState.Start, null)] // Test deal with format is not right below.
        [TestCase("[1,???]:", 0, TextIndex.IndexState.Start, null)]
        [TestCase("[1,]", 0, TextIndex.IndexState.Start, null)]
        [TestCase("[,start]", 0, TextIndex.IndexState.Start, null)]
        [TestCase("[]", 0, TextIndex.IndexState.Start, null)]
        public void TestDeserialize(string json, int index, TextIndex.IndexState state, int? time)
        {
            var expected = new TimeTag(new TextIndex(index, state), time);
            var actual = JsonConvert.DeserializeObject<TimeTag>($"\"{json}\"", CreateSettings());
            Assert.AreEqual(expected.Index, actual?.Index);
            Assert.AreEqual(expected.Time, actual?.Time);
        }
    }
}
