// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class ElementIdConverterTest : BaseSingleConverterTest<ElementIdConverter>
{
    [TestCase("1234567", "\"1234567\"")]
    [TestCase("", "\"\"")]
    [TestCase(null, "null")] // support the case with nullable property.
    public void TestSerialize(string? id, string json)
    {
        var elementId = createElementId(id);
        string actual = JsonConvert.SerializeObject(elementId, CreateSettings());
        Assert.AreEqual(json, actual);
    }

    [TestCase("\"1234567\"", "1234567")]
    [TestCase("\"\"", "")]
    [TestCase("null", null)] // support the case with nullable property.
    public void TestDeserialize(string json, string? id)
    {
        var expected = createElementId(id);
        var result = JsonConvert.DeserializeObject<ElementId?>(json, CreateSettings());
        Assert.AreEqual(expected, result);
    }

    private static ElementId? createElementId(string? str) =>
        str switch
        {
            null => null,
            "" => ElementId.Empty,
            _ => new ElementId(str)
        };
}
