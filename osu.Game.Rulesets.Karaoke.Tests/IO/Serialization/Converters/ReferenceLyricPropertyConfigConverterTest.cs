// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class ReferenceLyricPropertyConfigConverterTest : BaseSingleConverterTest<ReferenceLyricPropertyConfigConverter>
{
    [Test]
    public void TestReferenceLyricConfigSerializer()
    {
        var config = new ReferenceLyricConfig
        {
            OffsetTime = 100,
        };

        const string expected = "{\"$type\":\"ReferenceLyricConfig\",\"offset_time\":100.0}";
        string actual = JsonConvert.SerializeObject(config, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestReferenceLyricConfigDeserializer()
    {
        const string json = "{\"$type\":\"ReferenceLyricConfig\",\"offset_time\":100.0}";

        var expected = new ReferenceLyricConfig
        {
            OffsetTime = 100,
        };
        var actual = (ReferenceLyricConfig)JsonConvert.DeserializeObject<IReferenceLyricPropertyConfig>(json, CreateSettings())!;
        Assert.That(actual.OffsetTime, Is.EqualTo(expected.OffsetTime));
    }

    [Test]
    public void TestSyncLyricConfigSerializer()
    {
        var config = new SyncLyricConfig
        {
            OffsetTime = 100,
            SyncSingerProperty = true,
            SyncTimeTagProperty = false,
        };

        const string expected = "{\"$type\":\"SyncLyricConfig\",\"offset_time\":100.0,\"sync_time_tag_property\":false}";
        string actual = JsonConvert.SerializeObject(config, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestSyncLyricConfigDeserializer()
    {
        const string json = "{\"$type\":\"SyncLyricConfig\",\"offset_time\":100.0,\"sync_time_tag_property\":false}";

        var expected = new SyncLyricConfig
        {
            OffsetTime = 100,
            SyncSingerProperty = true,
            SyncTimeTagProperty = false,
        };
        var actual = (SyncLyricConfig)JsonConvert.DeserializeObject<IReferenceLyricPropertyConfig>(json, CreateSettings())!;
        Assert.That(actual.OffsetTime, Is.EqualTo(expected.OffsetTime));
        Assert.That(actual.SyncSingerProperty, Is.EqualTo(expected.SyncSingerProperty));
        Assert.That(actual.SyncTimeTagProperty, Is.EqualTo(expected.SyncTimeTagProperty));
    }
}
