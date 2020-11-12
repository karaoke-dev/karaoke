// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    [TestFixture]
    public class TimeTagsConverterTest
    {
        [Test]
        public void TestSerialize()
        {
            var rowTimeTag = new List<Tuple<TimeTagIndex, double?>>
            {
                Tuple.Create<TimeTagIndex, double?>(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1000d),
                Tuple.Create<TimeTagIndex, double?>(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1100d),
                Tuple.Create<TimeTagIndex, double?>(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1200d),
            };

            var result = JsonConvert.SerializeObject(rowTimeTag, createSettings());

            Assert.AreEqual(result, "[\r\n  \"0,0,1000\",\r\n  \"0,1,1100\",\r\n  \"0,1,1200\"\r\n]");
        }

        [Test]
        public void TestDeserialize()
        {
            var jsonString = "[\r\n  \"0,0,1000\",\r\n  \"0,1,1100\",\r\n  \"0,1,1200\"\r\n]";
            var result = JsonConvert.DeserializeObject<List<Tuple<TimeTagIndex, double?>>>(jsonString, createSettings());

            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0].Item1.Index, 0);
            Assert.AreEqual(result[0].Item1.State, TimeTagIndex.IndexState.Start);
            Assert.AreEqual(result[0].Item2, 1000);
        }

        private JsonSerializerSettings createSettings()
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.Converters = new JsonConverter[] { new TimeTagsConverter() };
            return globalSetting;
        }
    }
}
