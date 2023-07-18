// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class SingerConverterTest : BaseSingleConverterTest<SingerConverter>
{
    private static readonly ElementId main_singer_id = TestCaseElementIdHelper.CreateElementIdByNumber(2);

    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new ElementIdConverter();
    }

    [Test]
    public void TestMainSingerSerializer()
    {
        var singer = new Singer();

        string expected = $"{{\"$type\":\"Singer\",\"id\":\"{singer.ID}\"}}";
        string actual = JsonConvert.SerializeObject(singer, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestMainSingerDeserializer()
    {
        var expected = new Singer();

        string json = $"{{\"$type\":\"Singer\",\"id\":\"{expected.ID}\"}}";
        var actual = (Singer)JsonConvert.DeserializeObject<ISinger>(json, CreateSettings())!;

        Assert.AreEqual(expected.ID, actual.ID);
    }

    [Test]
    public void TestSingerStateSerializer()
    {
        var singer = new SingerState(main_singer_id);

        string expected = $"{{\"$type\":\"SingerState\",\"id\":\"{singer.ID}\",\"main_singer_id\":\"{singer.MainSingerId}\"}}";
        string actual = JsonConvert.SerializeObject(singer, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestSingerStateDeserializer()
    {
        var expected = new SingerState(main_singer_id);

        string json = $"{{\"$type\":\"SingerState\",\"id\":\"{expected.ID}\",\"main_singer_id\":\"{expected.MainSingerId}\"}}";
        var actual = (SingerState)JsonConvert.DeserializeObject<ISinger>(json, CreateSettings())!;

        Assert.AreEqual(expected.ID, actual.ID);
        Assert.AreEqual(expected.MainSingerId, actual.MainSingerId);
    }

    [Test]
    public void TestSingerInfoSerializer()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var singerState = singerInfo.AddSingerState(singer);

        string expected =
            $"{{\"singers\":[{{\"$type\":\"Singer\",\"id\":\"{singer.ID}\"}},{{\"$type\":\"SingerState\",\"id\":\"{singerState.ID}\",\"main_singer_id\":\"{singerState.MainSingerId}\"}}]}}";
        string actual = JsonConvert.SerializeObject(singerInfo, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestSingerInfoDeserializer()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var singerState = singerInfo.AddSingerState(singer);

        string json = $"{{\"singers\":[{{\"$type\":\"Singer\",\"id\":\"{singer.ID}\"}},{{\"$type\":\"SingerState\",\"id\":\"{singerState.ID}\",\"main_singer_id\":\"{singerState.MainSingerId}\"}}]}}";
        var actual = JsonConvert.DeserializeObject<SingerInfo>(json, CreateSettings())!;

        Assert.AreEqual(singerInfo.Singers.Count, actual.Singers.Count);
        Assert.AreEqual(singerInfo.Singers[0].ID, singerInfo.Singers[0].ID); // test singer
        Assert.AreEqual(singerInfo.Singers[1].ID, singerInfo.Singers[1].ID); // test sub-singer.
    }
}
