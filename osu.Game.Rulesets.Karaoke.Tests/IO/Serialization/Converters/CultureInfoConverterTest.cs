// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

[TestFixture]
public class CultureInfoConverterTest : BaseSingleConverterTest<CultureInfoConverter>
{
    [TestCase(1, "1")]
    [TestCase(null, "null")]
    public void TestSerialize(int? lcid, string json)
    {
        var language = lcid != null ? new CultureInfo(lcid.Value) : default;
        string actual = JsonConvert.SerializeObject(language, CreateSettings());
        Assert.AreEqual(json, actual);
    }

    [TestCase("1", 1)]
    [TestCase("null", null)]
    public void TestDeserialize(string json, int? lcid)
    {
        var result = JsonConvert.DeserializeObject<CultureInfo>(json, CreateSettings());
        Assert.AreEqual(lcid, result?.LCID);
    }
}
