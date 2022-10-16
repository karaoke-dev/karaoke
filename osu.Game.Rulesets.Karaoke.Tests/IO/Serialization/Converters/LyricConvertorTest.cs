// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class LyricConvertorTest : BaseSingleConverterTest<LyricConvertor>
    {
        protected override JsonConverter[] CreateExtraConverts() =>
            new JsonConverter[]
            {
                new ReferenceLyricPropertyConfigConvertor()
            };

        [Test]
        public void TestLyricConvertorWithNoConfig()
        {
            var lyric = new Lyric();

            const string expected =
                "{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"sample_control_point\":{\"sample_bank_bindable\":\"normal\",\"sample_volume_bindable\":100,\"sample_bank\":\"normal\",\"sample_volume\":100},\"difficulty_control_point\":{\"slider_velocity_bindable\":1.0,\"slider_velocity\":1.0},\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"romaji_tags\":[],\"singers\":[],\"translates\":{},\"samples\":[],\"auxiliary_samples\":[]}";
            string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDeserializeWithNoConfig()
        {
            const string json =
                "{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"sample_control_point\":{\"sample_bank_bindable\":\"normal\",\"sample_volume_bindable\":100,\"sample_bank\":\"normal\",\"sample_volume\":100},\"difficulty_control_point\":{\"slider_velocity_bindable\":1.0,\"slider_velocity\":1.0},\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"romaji_tags\":[],\"singers\":[],\"translates\":{},\"samples\":[],\"auxiliary_samples\":[]}";

            var expected = new Lyric();
            var actual = JsonConvert.DeserializeObject<Lyric>(json, CreateSettings())!;
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Text, actual.Text);
            TimeTagAssert.ArePropertyEqual(expected.TimeTags, actual.TimeTags);
            Assert.AreEqual(expected.LyricStartTime, actual.LyricStartTime);
            Assert.AreEqual(expected.LyricEndTime, actual.LyricEndTime);
            TextTagAssert.ArePropertyEqual(expected.RubyTags, actual.RubyTags);
            TextTagAssert.ArePropertyEqual(expected.RomajiTags, actual.RomajiTags);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.Duration, actual.Duration);
            Assert.AreEqual(expected.Singers, actual.Singers);
            Assert.AreEqual(expected.Translates, actual.Translates);
            Assert.AreEqual(expected.Language, actual.Language);
            Assert.AreEqual(expected.Lock, actual.Lock);
            Assert.AreEqual(expected.ReferenceLyric, actual.ReferenceLyric);
            Assert.AreEqual(expected.ReferenceLyricConfig, actual.ReferenceLyricConfig);
        }

        [Test]
        public void TestLyricConvertorWithSyncConfig()
        {
            var lyric = new Lyric
            {
                ReferenceLyric = new Lyric(),
                ReferenceLyricConfig = new SyncLyricConfig()
            };

            const string reference_lyric_json =
                "{\"$id\":\"1\",\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"sample_control_point\":{\"sample_bank_bindable\":\"normal\",\"sample_volume_bindable\":100,\"sample_bank\":\"normal\",\"sample_volume\":100},\"difficulty_control_point\":{\"slider_velocity_bindable\":1.0,\"slider_velocity\":1.0},\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"romaji_tags\":[],\"singers\":[],\"translates\":{},\"samples\":[],\"auxiliary_samples\":[]}";
            const string expected =
                $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"sample_control_point\":{{\"sample_bank_bindable\":\"normal\",\"sample_volume_bindable\":100,\"sample_bank\":\"normal\",\"sample_volume\":100}},\"difficulty_control_point\":{{\"slider_velocity_bindable\":1.0,\"slider_velocity\":1.0}},\"reference_lyric\":{reference_lyric_json},\"reference_lyric_config\":{{\"$type\":\"SyncLyricConfig\"}},\"samples\":[],\"auxiliary_samples\":[]}}";
            string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLyricConvertorWithReferenceConfig()
        {
            var lyric = new Lyric
            {
                ReferenceLyric = new Lyric(),
                ReferenceLyricConfig = new ReferenceLyricConfig()
            };

            const string reference_lyric_json =
                "{\"$id\":\"1\",\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"sample_control_point\":{\"sample_bank_bindable\":\"normal\",\"sample_volume_bindable\":100,\"sample_bank\":\"normal\",\"sample_volume\":100},\"difficulty_control_point\":{\"slider_velocity_bindable\":1.0,\"slider_velocity\":1.0},\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"romaji_tags\":[],\"singers\":[],\"translates\":{},\"samples\":[],\"auxiliary_samples\":[]}";
            const string expected =
                $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"sample_control_point\":{{\"sample_bank_bindable\":\"normal\",\"sample_volume_bindable\":100,\"sample_bank\":\"normal\",\"sample_volume\":100}},\"difficulty_control_point\":{{\"slider_velocity_bindable\":1.0,\"slider_velocity\":1.0}},\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"romaji_tags\":[],\"singers\":[],\"translates\":{{}},\"reference_lyric\":{reference_lyric_json},\"reference_lyric_config\":{{\"$type\":\"ReferenceLyricConfig\"}},\"samples\":[],\"auxiliary_samples\":[]}}";
            string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
            Assert.AreEqual(expected, actual);
        }
    }
}
