// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Groups;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class KaraokeSkinGroupConvertorTest : BaseSingleConverterTest<KaraokeSkinGroupConvertor>
    {
        [Test]
        public void TestGroupBySingerIdsSerializer()
        {
            var group = new GroupBySingerIds
            {
                ID = 123,
                Name = "Singer 1 and 2",
                SingerIds = new[] { 1, 2 }
            };
            string result = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":\"GroupBySingerIds\",\"singer_ids\":[1,2],\"id\":123,\"name\":\"Singer 1 and 2\"}");
        }

        [Test]
        public void TestGroupBySingerIdsDeserializer()
        {
            const string json = "{\"$type\":\"GroupBySingerIds\",\"singer_ids\":[1,2],\"id\":123,\"name\":\"Singer 1 and 2\"}";
            var result = JsonConvert.DeserializeObject<IGroup>(json, CreateSettings()) as GroupBySingerIds;
            var actual = new GroupBySingerIds
            {
                ID = 123,
                Name = "Singer 1 and 2",
                SingerIds = new[] { 1, 2 }
            };

            Assert.NotNull(result);
            Assert.AreEqual(result.ID, actual.ID);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.SingerIds, actual.SingerIds);
        }

        [Test]
        public void TestGroupBySingerNumberSerializer()
        {
            var group = new GroupBySingerNumber
            {
                ID = 123,
                Name = "Two singers",
                SingerNumber = 2,
            };
            string result = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":\"GroupBySingerNumber\",\"singer_number\":2,\"id\":123,\"name\":\"Two singers\"}");
        }

        [Test]
        public void TestGroupBySingerNumberDeserializer()
        {
            const string json = "{\"$type\":\"GroupBySingerNumber\",\"singer_number\":2,\"id\":123,\"name\":\"Two singers\"}";
            var result = JsonConvert.DeserializeObject<IGroup>(json, CreateSettings()) as GroupBySingerNumber;
            var actual = new GroupBySingerNumber
            {
                ID = 123,
                Name = "Two singers",
                SingerNumber = 2,
            };

            Assert.NotNull(result);
            Assert.AreEqual(result.ID, actual.ID);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.SingerNumber, actual.SingerNumber);
        }

        [Test]
        public void TestGroupByLyricIdsSerializer()
        {
            var group = new GroupByLyricIds
            {
                ID = 123,
                Name = "Lyric 1 and 2",
                LyricIds = new[] { 1, 2 }
            };
            string result = JsonConvert.SerializeObject(group, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":\"GroupByLyricIds\",\"lyric_ids\":[1,2],\"id\":123,\"name\":\"Lyric 1 and 2\"}");
        }

        [Test]
        public void TestGroupByLyricIdsDeserializer()
        {
            const string json = "{\"$type\":\"GroupByLyricIds\",\"lyric_ids\":[1,2],\"id\":123,\"name\":\"Lyric 1 and 2\"}";
            var result = JsonConvert.DeserializeObject<IGroup>(json, CreateSettings()) as GroupByLyricIds;
            var actual = new GroupByLyricIds
            {
                ID = 123,
                Name = "Lyric 1 and 2",
                LyricIds = new[] { 1, 2 }
            };

            Assert.NotNull(result);
            Assert.AreEqual(result.ID, actual.ID);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.LyricIds, actual.LyricIds);
        }
    }
}
