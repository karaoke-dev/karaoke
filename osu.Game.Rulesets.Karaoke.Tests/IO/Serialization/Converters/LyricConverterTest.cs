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
            $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"id\":\"{lyric.ID}\",\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"singer_ids\":[],\"translates\":{{}},\"samples\":[],\"auxiliary_samples\":[]}}";
        string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestDeserializeWithNoConfig()
    {
        var expected = new Lyric();

        string json =
            $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"id\":\"{expected.ID}\",\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"singer_ids\":[],\"translates\":{{}},\"samples\":[],\"auxiliary_samples\":[]}}";
        var actual = JsonConvert.DeserializeObject<Lyric>(json, CreateSettings())!;

        Assert.AreEqual(expected.ID, actual.ID);
        Assert.AreEqual(expected.Text, actual.Text);
        TimeTagAssert.ArePropertyEqual(expected.TimeTags, actual.TimeTags);
        Assert.AreEqual(expected.LyricTimingInfo, actual.LyricTimingInfo);
        RubyTagAssert.ArePropertyEqual(expected.RubyTags, actual.RubyTags);
        Assert.AreEqual(expected.StartTime, actual.StartTime);
        Assert.AreEqual(expected.Duration, actual.Duration);
        Assert.AreEqual(expected.SingerIds, actual.SingerIds);
        Assert.AreEqual(expected.Translates, actual.Translates);
        Assert.AreEqual(expected.Language, actual.Language);
        Assert.AreEqual(expected.Lock, actual.Lock);
        Assert.AreEqual(expected.ReferenceLyric, actual.ReferenceLyric);
        Assert.AreEqual(expected.ReferenceLyricId, actual.ReferenceLyricId);
        Assert.AreEqual(expected.ReferenceLyricConfig, actual.ReferenceLyricConfig);
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
        Assert.AreEqual(expected, actual);
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
            $"{{\"time_preempt\":600.0,\"time_fade_in\":400.0,\"start_time_bindable\":0.0,\"samples_bindable\":[],\"id\":\"{lyric.ID}\",\"text\":\"\",\"time_tags\":[],\"ruby_tags\":[],\"singer_ids\":[],\"translates\":{{}},\"reference_lyric_id\":\"{lyric.ReferenceLyricId}\",\"reference_lyric_config\":{{\"$type\":\"ReferenceLyricConfig\"}},\"samples\":[],\"auxiliary_samples\":[]}}";
        string actual = JsonConvert.SerializeObject(lyric, CreateSettings());
        Assert.AreEqual(expected, actual);
    }
}
