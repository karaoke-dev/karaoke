// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization
{
    public class KaraokeJsonSerializableExtensionsTest
    {
        [Test]
        public void TestSerializeLyric()
        {
            var lyric = new Lyric();

            const string expeccted = @"{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""sample_control_point"":{""sample_bank_bindable"":""normal"",""sample_volume_bindable"":100,""sample_bank"":""normal"",""sample_volume"":100},""difficulty_control_point"":{""slider_velocity_bindable"":1.0,""slider_velocity"":1.0},""text"":"""",""time_tags"":[],""ruby_tags"":[],""romaji_tags"":[],""singers"":[],""translates"":[],""samples"":[],""auxiliary_samples"":[]}";

            string actual = JsonConvert.SerializeObject(lyric, createSettings());
            Assert.AreEqual(expeccted, actual);
        }

        [Test]
        public void TestDeserializeLyric()
        {
            const string json = @"{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""sample_control_point"":{""sample_bank_bindable"":""normal"",""sample_volume_bindable"":100,""sample_bank"":""normal"",""sample_volume"":100},""difficulty_control_point"":{""slider_velocity_bindable"":1.0,""slider_velocity"":1.0},""text"":"""",""time_tags"":[],""ruby_tags"":[],""romaji_tags"":[],""singers"":[],""translates"":[],""samples"":[],""auxiliary_samples"":[]}";

            Lyric expected = new Lyric();
            Lyric actual = JsonConvert.DeserializeObject<Lyric>(json, createSettings())!;
            ObjectAssert.ArePropertyEqual(expected, actual);
        }

        [Test]
        public void TestSerializeNote()
        {
            var note = new Note();

            const string expeccted = @"{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""sample_control_point"":{""sample_bank_bindable"":""normal"",""sample_volume_bindable"":100,""sample_bank"":""normal"",""sample_volume"":100},""difficulty_control_point"":{""slider_velocity_bindable"":1.0,""slider_velocity"":1.0},""start_time_offset"":0.0,""end_time_offset"":0.0,""samples"":[],""auxiliary_samples"":[]}";

            string actual = JsonConvert.SerializeObject(note, createSettings());
            Assert.AreEqual(expeccted, actual);
        }

        [Test]
        public void TestDeserializeNote()
        {
            const string json = @"{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""sample_control_point"":{""sample_bank_bindable"":""normal"",""sample_volume_bindable"":100,""sample_bank"":""normal"",""sample_volume"":100},""difficulty_control_point"":{""slider_velocity_bindable"":1.0,""slider_velocity"":1.0},""start_time_offset"":0.0,""end_time_offset"":0.0,""samples"":[],""auxiliary_samples"":[]}";

            Note expected = new Note();
            Note actual = JsonConvert.DeserializeObject<Note>(json, createSettings())!;
            ObjectAssert.ArePropertyEqual(expected, actual);
        }

        private JsonSerializerSettings createSettings()
        {
            var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
            settings.Formatting = Formatting.None;
            return settings;
        }
    }
}
