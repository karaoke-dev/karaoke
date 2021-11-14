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
        protected override JsonConverter[] CreateDefaultConverts()
            => base.CreateDefaultConverts().Concat(new JsonConverter[]
            {
                new ColourConvertor(),
            }).ToArray();

        [Test]
        public void TestSerializer()
        {
            var shader = new ShadowShader
            {
                ShadowOffset = new Vector2(10),
                ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
            };
            var result = JsonConvert.SerializeObject(shader, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":\"ShadowShader\",\"ShadowColour\":\"#7F7F7F7F\",\"ShadowOffset\":{\"x\":10.0,\"y\":10.0}}");
        }

        [Test]
        public void TestDeserialize()
        {
            const string json = "{\"$type\":\"ShadowShader\",\"ShadowColour\":\"#7F7F7F7F\",\"ShadowOffset\":{\"x\":10.0,\"y\":10.0}}";
            var result = JsonConvert.DeserializeObject<IShader>(json, CreateSettings()) as ShadowShader;
            var actual = new ShadowShader
            {
                ShadowOffset = new Vector2(10),
                ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
            };
            Assert.NotNull(result);
            Assert.AreEqual(result.ShadowOffset, actual.ShadowOffset);
            Assert.AreEqual(result.ShadowColour.ToHex(), actual.ShadowColour.ToHex());
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
                        ShadowOffset = new Vector2(10),
                        ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
                    }
                }
            };
            var result = JsonConvert.SerializeObject(shader, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":\"StepShader\",\"Name\":\"HelloShader\",\"Draw\":true,\"StepShaders\":[{\"$type\":\"ShadowShader\",\"ShadowColour\":\"#7F7F7F7F\",\"ShadowOffset\":{\"x\":10.0,\"y\":10.0}}]}");
        }

        [Test]
        public void TestDeserializListItems()
        {
            const string json = "{\"$type\":\"StepShader\",\"Name\":\"HelloShader\",\"Draw\":true,\"StepShaders\":[{\"$type\":\"ShadowShader\",\"ShadowColour\":\"#7F7F7F7F\",\"ShadowOffset\":{\"x\":10.0,\"y\":10.0}}]}";
            var result = JsonConvert.DeserializeObject<IShader>(json, CreateSettings()) as StepShader;
            var actual = new StepShader
            {
                Name = "HelloShader",
                StepShaders = new[]
                {
                    new ShadowShader
                    {
                        ShadowOffset = new Vector2(10),
                        ShadowColour = new Color4(0.5f, 0.5f, 0.5f, 0.5f),
                    }
                }
            };

            // test step shader.
            Assert.NotNull(result);
            Assert.AreEqual(result.StepShaders.Count, actual.StepShaders.Count);

            // test shadow shader inside.
            var resultShadowShader = result.StepShaders.FirstOrDefault() as ShadowShader;
            var actualShadowShader = result.StepShaders.FirstOrDefault() as ShadowShader;
            Assert.NotNull(resultShadowShader);
            Assert.NotNull(actualShadowShader);
            Assert.AreEqual(resultShadowShader.ShadowOffset, actualShadowShader.ShadowOffset);
            Assert.AreEqual(resultShadowShader.ShadowColour.ToHex(), actualShadowShader.ShadowColour.ToHex());
        }
    }
}
