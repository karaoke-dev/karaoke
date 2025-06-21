// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class LyricConverterTest : BaseSingleConverterTest<LyricConverter>
{
    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new ReferenceLyricPropertyConfigConverter();
        yield return new ElementIdConverter();
    }

    [Test]
    public void TestLyricConverterWithNoConfig()
    {
        var lyric = new Lyric();

        string expected =
            $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"id\":\"{lyric.ID}\",\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"singer_ids\":[],\"translations\":{{}},\"samples\":[],\"auxiliary_samples\":[]}}";
        string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestDeserializeWithNoConfig()
    {
        var expected = new Lyric();

        string json =
            $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"id\":\"{expected.ID}\",\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"singer_ids\":[],\"translations\":{{}},\"samples\":[],\"auxiliary_samples\":[]}}";
        var actual = JsonConvert.DeserializeObject<Lyric>(json, CreateSettings())!;

        Assert.That(actual.ID, Is.EqualTo(expected.ID));
        Assert.That(actual.Text, Is.EqualTo(expected.Text));
        TimeTagAssert.ArePropertyEqual(expected.TimeTags, actual.TimeTags);
        RubyTagAssert.ArePropertyEqual(expected.RubyTags, actual.RubyTags);
        Assert.That(actual.StartTime, Is.EqualTo(expected.StartTime));
        Assert.That(actual.Duration, Is.EqualTo(expected.Duration));
        Assert.That(actual.EndTime, Is.EqualTo(expected.EndTime));
        Assert.That(actual.SingerIds, Is.EqualTo(expected.SingerIds));
        Assert.That(actual.Translations, Is.EqualTo(expected.Translations));
        Assert.That(actual.Language, Is.EqualTo(expected.Language));
        Assert.That(actual.Lock, Is.EqualTo(expected.Lock));
        Assert.That(actual.ReferenceLyric, Is.EqualTo(expected.ReferenceLyric));
        Assert.That(actual.ReferenceLyricId, Is.EqualTo(expected.ReferenceLyricId));
        Assert.That(actual.ReferenceLyricConfig, Is.EqualTo(expected.ReferenceLyricConfig));
    }

    [Test]
    public void TestLyricConverterWithSyncConfig()
    {
        var referencedLyric = new Lyric();
        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new SyncLyricConfig(),
        };

        string expected =
            $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"id\":\"{lyric.ID}\",\"reference_lyric_id\":\"{lyric.ReferenceLyricId}\",\"reference_lyric_config\":{{\"$type\":\"SyncLyricConfig\"}},\"samples\":[],\"auxiliary_samples\":[]}}";
        string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestLyricConverterWithReferenceConfig()
    {
        var referencedLyric = new Lyric();
        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new ReferenceLyricConfig(),
        };

        string expected =
            $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"id\":\"{lyric.ID}\",\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"singer_ids\":[],\"translations\":{{}},\"reference_lyric_id\":\"{lyric.ReferenceLyricId}\",\"reference_lyric_config\":{{\"$type\":\"ReferenceLyricConfig\"}},\"samples\":[],\"auxiliary_samples\":[]}}";
        string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }
}
