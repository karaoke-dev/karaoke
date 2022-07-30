// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class TranslatesConvertorTest : BaseSingleConverterTest<TranslatesConvertor>
    {
        protected override JsonConverter[] CreateExtraConverts()
            => new JsonConverter[]
            {
                new CultureInfoConverter(),
            };

        [Test]
        public void TestSerialize()
        {
            var translates = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo("en-US"), "karaoke" },
                { new CultureInfo("Ja-jp"), "カラオケ" }
            };

            const string expected = "[{\"key\":1033,\"value\":\"karaoke\"},{\"key\":1041,\"value\":\"カラオケ\"}]";
            string actual = JsonConvert.SerializeObject(translates, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDeserialize()
        {
            const string json = "[{\"key\":1033,\"value\":\"karaoke\"},{\"key\":1041,\"value\":\"カラオケ\"}]";

            var expected = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo("en-US"), "karaoke" },
                { new CultureInfo("Ja-jp"), "カラオケ" }
            };

            var actual = JsonConvert.DeserializeObject<Dictionary<CultureInfo, string>>(json, CreateSettings()) ?? throw new InvalidCastException();
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
