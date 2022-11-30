﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class SingerConvertorTest : BaseSingleConverterTest<SingerConvertor>
{
    [Test]
    public void TestMainSingerSerializer()
    {
        var singer = new Singer(1);

        const string expected = "{\"$type\":\"Singer\",\"id\":1}";
        string actual = JsonConvert.SerializeObject(singer, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestMainSingerDeserializer()
    {
        const string json = "{\"$type\":\"Singer\",\"id\":1}";

        var expected = new Singer(1);
        var actual = (Singer)JsonConvert.DeserializeObject<ISinger>(json, CreateSettings())!;
        Assert.AreEqual(expected.ID, actual.ID);
    }

    [Test]
    public void TestSubSingerSerializer()
    {
        var singer = new SubSinger(1);

        const string expected = "{\"$type\":\"SubSinger\",\"description\":\"\",\"id\":1}";
        string actual = JsonConvert.SerializeObject(singer, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestSubSingerDeserializer()
    {
        const string json = "{\"$type\":\"SubSinger\",\"description\":\"\",\"id\":1}";

        var expected = new SubSinger(1);
        var actual = (SubSinger)JsonConvert.DeserializeObject<ISinger>(json, CreateSettings())!;
        Assert.AreEqual(expected.ID, actual.ID);
    }
}