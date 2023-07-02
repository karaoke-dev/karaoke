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
    [TestCase(null, "null")]
    public void TestSerialize(string? id, string json)
    {
        var elementId = createElementId(id);
        string actual = JsonConvert.SerializeObject(elementId, CreateSettings());
        Assert.AreEqual(json, actual);
    }

    [TestCase("\"1234567\"", "1234567")]
    [TestCase("\"\"", "")]
    [TestCase("null", "")] // should make sure that create the ElementId.Empty if not receive the id.
    public void TestDeserialize(string json, string? id)
    {
        var expected = createElementId(id);
        var result = JsonConvert.DeserializeObject<ElementId>(json, CreateSettings());
        Assert.AreEqual(expected, result);
    }

    private static ElementId? createElementId(string? str)
    {
        if (str == null)
            return null;

        if (str == string.Empty)
            return ElementId.Empty;

        return new ElementId(str);
    }
}
