// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class ColourConverterTest : BaseSingleConverterTest<ColourConverter>
{
    [TestCase("#aaaaaa", "#AAAAAA")]
    [TestCase("#aaaaaaaa", "#AAAAAAAA")]
    public void TestSerialize(string hex, string json)
    {
        var colour = Color4Extensions.FromHex(hex);

        string expected = $"\"{json}\"";
        string actual = JsonConvert.SerializeObject(colour, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [TestCase("#aaaaaa", "#AAAAAA")]
    [TestCase("#AAAAAA", "#AAAAAA")]
    [TestCase("#AAAAAAAA", "#AAAAAAAA")]
    [TestCase("", null)] // should throw exception
    [TestCase(null, null)] // should throw exception
    public void TestDeserialize(string? json, string? hex)
    {
        if (hex != null)
        {
            var expected = Color4Extensions.FromHex(hex);
            var actual = JsonConvert.DeserializeObject<Color4>($"\"{json}\"", CreateSettings());
            Assert.AreEqual(expected, actual);
        }
        else
        {
            Assert.Catch(() => JsonConvert.DeserializeObject<Color4>($"\"{json}\"", CreateSettings()));
        }
    }
}
