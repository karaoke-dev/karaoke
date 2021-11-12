// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class FontUsageConvertorTest : BaseSingleConverterTest<FontUsageConvertor>
    {
        [TestCase("", 20, "", false, false, "{}")]
        [TestCase("OpenSans", 20, "", false, false, "{\"family\":\"OpenSans\"}")]
        [TestCase("OpenSans", 30, "", false, false, "{\"family\":\"OpenSans\",\"size\":30.0}")]
        [TestCase("OpenSans", 30, "RegularItalic", false, false, "{\"family\":\"OpenSans\",\"weight\":\"RegularItalic\",\"size\":30.0}")]
        [TestCase("OpenSans", 30, "RegularItalic", true, false, "{\"family\":\"OpenSans\",\"weight\":\"RegularItalic\",\"size\":30.0,\"italics\":true}")]
        [TestCase("OpenSans", 30, "RegularItalic", true, true, "{\"family\":\"OpenSans\",\"weight\":\"RegularItalic\",\"size\":30.0,\"italics\":true,\"fixedWidth\":true}")]
        public void TestSerialize(string family, float size, string weight, bool italics, bool fixedWidth, string json)
        {
            var font = new FontUsage(family, size, weight, italics, fixedWidth);
            var result = JsonConvert.SerializeObject(font, CreateSettings());
            Assert.AreEqual(result, json);
        }

        [TestCase("{}", null, 20, null, false, false)]
        [TestCase("{\"family\": \"OpenSans\"}", "OpenSans", 20, null, false, false)]
        [TestCase("{\"family\": \"OpenSans\",\"size\": 30.0}", "OpenSans", 30, null, false, false)]
        [TestCase("{\"family\": \"OpenSans\",\"weight\": \"RegularItalic\",\"size\": 30.0}", "OpenSans", 30, "RegularItalic", false, false)]
        [TestCase("{\"family\": \"OpenSans\",\"weight\": \"RegularItalic\",\"size\": 30.0,\"italics\": true}", "OpenSans", 30, "RegularItalic", true, false)]
        [TestCase("{\"family\": \"OpenSans\",\"weight\": \"RegularItalic\",\"size\": 30.0,\"italics\": true,\"fixedWidth\": true}", "OpenSans", 30, "RegularItalic", true, true)]
        public void TestTestDeserialize(string json, string family, float size, string weight, bool italics, bool fixedWidth)
        {
            var result = JsonConvert.DeserializeObject<FontUsage>(json, CreateSettings());
            var actual = new FontUsage(family, size, weight, italics, fixedWidth);
            Assert.AreEqual(result, actual);
        }
    }
}
