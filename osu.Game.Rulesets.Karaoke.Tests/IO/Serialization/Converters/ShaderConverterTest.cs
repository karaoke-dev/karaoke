// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class ShaderConverterTest : BaseSingleConverterTest<ShaderConverter>
{
    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new ColourConverter();
    }

    [Test]
    public void TestSerializer()
    {
        var shader = new ShadowShader
        {
            ShadowOffset = new Vector2(3),
            ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
        };

        const string expected = "{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#7F7F7F7F\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}";
        string result = JsonConvert.SerializeObject(shader, CreateSettings());
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void TestDeserialize()
    {
        const string json = "{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#7F7F7F7F\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}";

        var expected = new ShadowShader
        {
            ShadowOffset = new Vector2(3),
            ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
        };
        var actual = (ShadowShader)JsonConvert.DeserializeObject<ICustomizedShader>(json, CreateSettings())!;
        Assert.That(actual.ShadowOffset, Is.EqualTo(expected.ShadowOffset));
        Assert.That(actual.ShadowColour.ToHex(), Is.EqualTo(expected.ShadowColour.ToHex()));
    }

    [Test]
    public void TestSerializerListItems()
    {
        var shader = new StepShader
        {
            Name = "HelloShader",
            StepShaders = new[]
            {
                new ShadowShader
                {
                    ShadowOffset = new Vector2(3),
                    ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
                },
            },
        };

        const string expected =
            "{\"$type\":\"StepShader\",\"name\":\"HelloShader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#7F7F7F7F\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}";
        string actual = JsonConvert.SerializeObject(shader, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestDeserializeListItems()
    {
        const string json =
            "{\"$type\":\"StepShader\",\"name\":\"HelloShader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#7F7F7F7F\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}";

        var expected = new StepShader
        {
            Name = "HelloShader",
            StepShaders = new[]
            {
                new ShadowShader
                {
                    ShadowOffset = new Vector2(3),
                    ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
                },
            },
        };
        var actual = (StepShader)JsonConvert.DeserializeObject<ICustomizedShader>(json, CreateSettings())!;

        // test step shader.
        Assert.That(actual.StepShaders.Count, Is.EqualTo(expected.StepShaders.Count));

        // test shadow shader inside.
        var expectedShadowShader = (ShadowShader)expected.StepShaders.First();
        var actualShadowShader = (ShadowShader)actual.StepShaders.First();
        Assert.That(actualShadowShader.ShadowOffset, Is.EqualTo(expectedShadowShader.ShadowOffset));
        Assert.That(actualShadowShader.ShadowColour.ToHex(), Is.EqualTo(expectedShadowShader.ShadowColour.ToHex()));
    }
}
