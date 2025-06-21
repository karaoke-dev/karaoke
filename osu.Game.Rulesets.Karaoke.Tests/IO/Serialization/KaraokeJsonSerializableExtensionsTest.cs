// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization;

public class KaraokeJsonSerializableExtensionsTest
{
    [Test]
    public void TestSerializeLyric()
    {
        var lyric = new Lyric();

        string expected =
            @$"{{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""id"":""{lyric.ID}"",""text"":"""",""time_tags"":[],""ruby_tags"":[],""singer_ids"":[],""translations"":[],""samples"":[],""auxiliary_samples"":[]}}";

        string actual = JsonConvert.SerializeObject(lyric, createSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestDeserializeLyric()
    {
        var expected = new Lyric();
        string json =
            @$"{{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""id"":""{expected.ID}"",""text"":"""",""time_tags"":[],""ruby_tags"":[],""singer_ids"":[],""translations"":[],""samples"":[],""auxiliary_samples"":[]}}";

        var actual = JsonConvert.DeserializeObject<Lyric>(json, createSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }

    [Test]
    public void TestSerializeNote()
    {
        var note = new Note();

        const string expected =
            @"{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""samples"":[],""auxiliary_samples"":[]}";

        string actual = JsonConvert.SerializeObject(note, createSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestDeserializeNote()
    {
        const string json =
            @"{""time_preempt"":600.0,""time_fade_in"":400.0,""start_time_bindable"":0.0,""samples_bindable"":[],""samples"":[],""auxiliary_samples"":[]}";

        var expected = new Note();
        var actual = JsonConvert.DeserializeObject<Note>(json, createSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }

    private JsonSerializerSettings createSettings()
    {
        var settings = KaraokeJsonSerializableExtensions.CreateGlobalSettings();
        settings.Formatting = Formatting.None;
        return settings;
    }
}
