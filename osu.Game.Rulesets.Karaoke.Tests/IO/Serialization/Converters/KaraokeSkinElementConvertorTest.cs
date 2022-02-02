// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class KaraokeSkinElementConvertorTest : BaseSingleConverterTest<KaraokeSkinElementConvertor>
    {
        [Test]
        public void TestLyricConfigSerializer()
        {
            var lyricConfig = LyricConfig.CreateDefault();
            string result = JsonConvert.SerializeObject(lyricConfig, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":0,\"name\":\"Default\",\"smart_horizon\":2,\"lyrics_interval\":4,\"ruby_interval\":2,\"romaji_interval\":2,\"ruby_alignment\":2,\"romaji_alignment\":2,\"ruby_margin\":4,\"romaji_margin\":4,\"main_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\",\"size\":48.0},\"ruby_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"},\"romaji_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"}}");
        }

        [Test]
        public void TestLyricConfigDeserialize()
        {
            const string json = "{\"$type\":0,\"name\":\"Default\",\"smart_horizon\":2,\"lyrics_interval\":4,\"ruby_interval\":2,\"romaji_interval\":2,\"ruby_alignment\":2,\"romaji_alignment\":2,\"ruby_margin\":4,\"romaji_margin\":4,\"main_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\",\"size\":48.0},\"ruby_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"},\"romaji_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"}}";
            var result = JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings()) as LyricConfig;
            var actual = LyricConfig.CreateDefault();

            Assert.NotNull(result);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.SmartHorizon, actual.SmartHorizon);
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
                Continuous = true,
            };
            string result = JsonConvert.SerializeObject(lyricLayout, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":1,\"id\":1,\"name\":\"Testing layout\",\"alignment\":9,\"horizontal_margin\":10,\"vertical_margin\":20,\"continuous\":true}");
        }

        [Test]
        public void TestLyricLayoutDeserialize()
        {
            const string json = "{\"$type\":1,\"id\":1,\"name\":\"Testing layout\",\"alignment\":9,\"horizontal_margin\":10,\"vertical_margin\":20,\"continuous\":true}";
            var result = JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings()) as LyricLayout;
            var actual = new LyricLayout
            {
                ID = 1,
                Name = "Testing layout",
                Alignment = Anchor.TopLeft,
                HorizontalMargin = 10,
                VerticalMargin = 20,
                Continuous = true,
            };

            Assert.NotNull(result);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.Alignment, actual.Alignment);
        }

        [Test]
        public void TestLyricStyleSerializer()
        {
            var lyricStyle = LyricStyle.CreateDefault();
            string result = JsonConvert.SerializeObject(lyricStyle, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":2,\"left_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":10,\"outline_colour\":\"#CCA532\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#6B5B2D\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"right_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":10,\"outline_colour\":\"#5932CC\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#3D2D6B\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"name\":\"Default\"}");
        }

        [Test]
        public void TestLyricStyleDeserializer()
        {
            const string json = "{\"$type\":2,\"left_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":10,\"outline_colour\":\"#CCA532\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#6B5B2D\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"right_lyric_text_shaders\":[{\"$type\":\"StepShader\",\"name\":\"Step shader\",\"draw\":true,\"step_shaders\":[{\"$type\":\"OutlineShader\",\"radius\":10,\"outline_colour\":\"#5932CC\"},{\"$type\":\"ShadowShader\",\"shadow_colour\":\"#3D2D6B\",\"shadow_offset\":{\"x\":3.0,\"y\":3.0}}]}],\"name\":\"Default\"}";
            var result = JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings()) as LyricStyle;
            var actual = LyricStyle.CreateDefault();

            Assert.NotNull(result);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.ID, actual.ID);
            Assert.AreEqual(result.LeftLyricTextShaders.Count, actual.LeftLyricTextShaders.Count);
            Assert.AreEqual(result.RightLyricTextShaders.Count, actual.RightLyricTextShaders.Count);
        }

        [Test]
        public void TestNoteStyleSerializer()
        {
            var lyricConfig = NoteStyle.CreateDefault();
            string result = JsonConvert.SerializeObject(lyricConfig, CreateSettings());
            Assert.AreEqual(result, "{\"$type\":3,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}");
        }

        [Test]
        public void TestNoteStyleDeserializer()
        {
            const string json = "{\"$type\":3,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}";
            var result = JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings()) as NoteStyle;
            var actual = NoteStyle.CreateDefault();

            Assert.NotNull(result);
            Assert.AreEqual(result.Name, actual.Name);
            Assert.AreEqual(result.ID, actual.ID);
            Assert.AreEqual(result.BlinkColor, actual.BlinkColor);
        }
    }
}
