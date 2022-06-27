// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class ShaderConvertorTest : BaseSingleConverterTest<ShaderConvertor>
    {
        protected override JsonConverter[] CreateExtraConverts()
            => new JsonConverter[]
            {
                new ColourConvertor(),
            };

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
            Assert.AreEqual(expected, result);
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
            var actual = JsonConvert.DeserializeObject<ICustomizedShader>(json, CreateSettings()) as ShadowShader;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ShadowOffset, actual.ShadowOffset);
            Assert.AreEqual(expected.ShadowColour.ToHex(), actual.ShadowColour.ToHex());
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
                    }
                }
            };

            const string expected =
                "{\"$type\":\"StepShader\",\"name\":\"HelloShader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#7F7F7F7F\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}";
            string actual = JsonConvert.SerializeObject(shader, CreateSettings());
            Assert.AreEqual(expected, actual);
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
                    }
                }
            };
            var actual = JsonConvert.DeserializeObject<ICustomizedShader>(json, CreateSettings()) as StepShader;

            // test step shader.
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.StepShaders.Count, actual.StepShaders.Count);

            // test shadow shader inside.
            var expectedShadowShader = expected.StepShaders.FirstOrDefault() as ShadowShader;
            var actualShadowShader = actual.StepShaders.FirstOrDefault() as ShadowShader;
            Assert.IsNotNull(expectedShadowShader);
            Assert.IsNotNull(actualShadowShader);
            Assert.AreEqual(expectedShadowShader.ShadowOffset, actualShadowShader.ShadowOffset);
            Assert.AreEqual(expectedShadowShader.ShadowColour.ToHex(), actualShadowShader.ShadowColour.ToHex());
        }
    }
}
