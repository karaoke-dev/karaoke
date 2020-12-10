// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    [TestFixture]
    public class TimeTagsConverterTest : BaseSingleConverterTest<TimeTagsConverter>
    {
        [Test]
        public void TestSerialize()
        {
            var rowTimeTag = new[]
            {
                new TimeTag(new TimeTagIndex(0), 1000d),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1100d),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1200d),
            };

            var result = JsonConvert.SerializeObject(rowTimeTag, CreateSettings());

            Assert.AreEqual(result, "[\r\n  \"0,0,1000\",\r\n  \"0,1,1100\",\r\n  \"0,1,1200\"\r\n]");
        }

        [Test]
        public void TestDeserialize()
        {
            const string json_string = "[\r\n  \"0,0,1000\",\r\n  \"0,1,1100\",\r\n  \"0,1,1200\"\r\n]";
            var result = JsonConvert.DeserializeObject<TimeTag[]>(json_string, CreateSettings());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0].Index.Index, 0);
            Assert.AreEqual(result[0].Index.State, TimeTagIndex.IndexState.Start);
            Assert.AreEqual(result[0].Time, 1000);
        }
    }
}
