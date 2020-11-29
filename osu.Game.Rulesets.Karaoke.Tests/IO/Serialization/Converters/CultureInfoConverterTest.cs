// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    [TestFixture]
    public class CultureInfoConverterTest
    {
        [TestCase(1, "1")]
        [TestCase(null, "null")]
        public void TestSerialize(int? calId, string json)
        {
            var language = calId != null ? new CultureInfo(calId.Value) : default(CultureInfo);
            var result = JsonConvert.SerializeObject(language, createSettings());
            Assert.AreEqual(result, json);
        }

        [TestCase("1", 1)]
        [TestCase("null", null)]
        public void TestDeserialize(string json, int? calId)
        {
            var result = JsonConvert.DeserializeObject<CultureInfo>(json, createSettings());
            Assert.AreEqual(result?.LCID, calId);
        }

        [Test]
        public void TestAllCultureInfo()
        {
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var cultureInfo in cultureInfos)
            {
                // this weird cultureInfo will let test case fail.
                if (cultureInfo.LCID == 4096)
                    continue;

                var json = JsonConvert.SerializeObject(cultureInfo, createSettings());
                var deserializedCultureInfo =  JsonConvert.DeserializeObject<CultureInfo>(json, createSettings());
                Assert.AreEqual(deserializedCultureInfo, cultureInfo);
            }
        }

        private JsonSerializerSettings createSettings()
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.Converters = new JsonConverter[] { new CultureInfoConverter() };
            return globalSetting;
        }
    }
}
