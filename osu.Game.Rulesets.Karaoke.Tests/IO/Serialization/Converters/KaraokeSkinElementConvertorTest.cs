// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class KaraokeSkinElementConvertorTest : BaseSingleConverterTest<KaraokeSkinElementConvertor>
    {
        protected override JsonConverter[] CreateExtraConverts()
            => new JsonConverter[]
            {
                new ColourConvertor(),
                new Vector2Converter(),
                new ShaderConvertor(),
                new FontUsageConvertor()
            };

        [Test]
        public void TestLyricConfigSerializer()
        {
            var lyricConfig = LyricConfig.CreateDefault();

            const string expected =
                "{\"$type\":0,\"name\":\"Default\",\"smart_horizon\":2,\"lyrics_interval\":4,\"ruby_interval\":2,\"romaji_interval\":2,\"ruby_alignment\":2,\"romaji_alignment\":2,\"ruby_margin\":4,\"romaji_margin\":4,\"main_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\",\"size\":48.0},\"ruby_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"},\"romaji_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"}}";
            string actual = JsonConvert.SerializeObject(lyricConfig, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLyricConfigDeserialize()
        {
            const string json =
                "{\"$type\":0,\"name\":\"Default\",\"smart_horizon\":2,\"lyrics_interval\":4,\"ruby_interval\":2,\"romaji_interval\":2,\"ruby_alignment\":2,\"romaji_alignment\":2,\"ruby_margin\":4,\"romaji_margin\":4,\"main_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\",\"size\":48.0},\"ruby_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"},\"romaji_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"}}";

            var expected = LyricConfig.CreateDefault();
            var actual = (LyricConfig)JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings())!;
            ObjectAssert.ArePropertyEqual(expected, actual);
        }

        [Test]
        public void TestLyricLayoutSerializer()
        {
            var lyricLayout = new LyricLayout
            {
                ID = 1,
                Name = "Testing layout",
                Alignment = Anchor.TopLeft,
                HorizontalMargin = 10,
                VerticalMargin = 20,
            };

            const string expected = "{\"$type\":1,\"id\":1,\"name\":\"Testing layout\",\"alignment\":9,\"horizontal_margin\":10,\"vertical_margin\":20}";
            string actual = JsonConvert.SerializeObject(lyricLayout, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLyricLayoutDeserialize()
        {
            const string json = "{\"$type\":1,\"id\":1,\"name\":\"Testing layout\",\"alignment\":9,\"horizontal_margin\":10,\"vertical_margin\":20}";

            var expected = new LyricLayout
            {
                ID = 1,
                Name = "Testing layout",
                Alignment = Anchor.TopLeft,
                HorizontalMargin = 10,
                VerticalMargin = 20,
            };
            var actual = (LyricLayout)JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings())!;
            ObjectAssert.ArePropertyEqual(expected, actual);
        }

        [Test]
        public void TestLyricStyleSerializer()
        {
            var lyricStyle = LyricStyle.CreateDefault();

            const string expected =
                "{\"$type\":2,\"left_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":3.0,\"outline_colour\":\"#CCA532\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#6B5B2D\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"right_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":3.0,\"outline_colour\":\"#5932CC\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#3D2D6B\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"name\":\"Default\"}";
            string actual = JsonConvert.SerializeObject(lyricStyle, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLyricStyleDeserializer()
        {
            const string json =
                "{\"$type\":2,\"left_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":3.0,\"outline_colour\":\"#CCA532\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#6B5B2D\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"right_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":3.0,\"outline_colour\":\"#5932CC\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#3D2D6B\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"name\":\"Default\"}";

            var expected = LyricStyle.CreateDefault();
            var actual = (LyricStyle)JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings())!;
            ObjectAssert.ArePropertyEqual(expected, actual);
        }

        [Test]
        public void TestNoteStyleSerializer()
        {
            var lyricConfig = NoteStyle.CreateDefault();

            const string expected = "{\"$type\":3,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}";
            string actual = JsonConvert.SerializeObject(lyricConfig, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNoteStyleDeserializer()
        {
            const string json = "{\"$type\":3,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}";

            var expected = NoteStyle.CreateDefault();
            var actual = (NoteStyle)JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings())!;
            ObjectAssert.ArePropertyEqual(expected, actual);
        }
    }
}
