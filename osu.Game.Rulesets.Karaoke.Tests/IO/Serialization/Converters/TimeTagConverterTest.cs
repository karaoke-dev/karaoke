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
        [TestCase(1, TimeTagIndex.IndexState.Start, 1000, "[1,start]:1000")]
        [TestCase(1, TimeTagIndex.IndexState.End, 1000, "[1,end]:1000")]
        [TestCase(-1, TimeTagIndex.IndexState.Start, 1000, "[-1,start]:1000")] // Should not check index is out of range in here.
        [TestCase(1, TimeTagIndex.IndexState.Start, -1000, "[1,start]:-1000")] // Should not check if time is negative.
        [TestCase(1, TimeTagIndex.IndexState.Start, null, "[1,start]:")]
        public void TestSerialize(int index, TimeTagIndex.IndexState state, int? time, string json)
        {
            var timeTag = new TimeTag(new TimeTagIndex(index, state), time);

            var result = JsonConvert.SerializeObject(timeTag, CreateSettings());
            Assert.AreEqual(result, $"\"{json}\"");
        }

        [TestCase("[1,start]:1000", 1, TimeTagIndex.IndexState.Start, 1000)]
        [TestCase("[1,end]:1000", 1, TimeTagIndex.IndexState.End, 1000)]
        [TestCase("[-1,start]:1000", -1, TimeTagIndex.IndexState.Start, 1000)] // Should not check index is out of range in here.
        [TestCase("[1,start]:-1000", 1, TimeTagIndex.IndexState.Start, -1000)] // Should not check if time is negative.
        [TestCase("[1,start]:", 1, TimeTagIndex.IndexState.Start, null)]
        [TestCase("", TimeTagIndex.IndexState.Start, 0, null)] // Test deal with format is not right below.
        [TestCase("[1,???]:", 0, TimeTagIndex.IndexState.Start, null)]
        [TestCase("[1,]", 0, TimeTagIndex.IndexState.Start, null)]
        [TestCase("[,start]", 0, TimeTagIndex.IndexState.Start, null)]
        [TestCase("[]", 0, TimeTagIndex.IndexState.Start, null)]
        public void TestDeserialize(string json, int index, TimeTagIndex.IndexState state, int? time)
        {
            var result = JsonConvert.DeserializeObject<TimeTag>($"\"{json}\"", CreateSettings());
            var actual = new TimeTag(new TimeTagIndex(index, state), time);
            Assert.AreEqual(result.Index, actual.Index);
            Assert.AreEqual(result.Time, actual.Time);
        }
    }
}
