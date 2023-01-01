// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class SingerConvertorTest : BaseSingleConverterTest<SingerConverter>
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
    public void TestSingerStateSerializer()
    {
        var singer = new SingerState(1, 2);

        const string expected = "{\"$type\":\"SingerState\",\"id\":1,\"main_singer_id\":2}";
        string actual = JsonConvert.SerializeObject(singer, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestSingerStateDeserializer()
    {
        const string json = "{\"$type\":\"SingerState\",\"id\":1,\"main_singer_id\":2}";

        var expected = new SingerState(1, 2);
        var actual = (SingerState)JsonConvert.DeserializeObject<ISinger>(json, CreateSettings())!;
        Assert.AreEqual(expected.ID, actual.ID);
        Assert.AreEqual(expected.MainSingerId, actual.MainSingerId);
    }

    [Test]
    public void TestSingerInfoSerializer()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        singerInfo.AddSingerState(singer);

        const string expected = "{\"singers\":[{\"$type\":\"Singer\",\"id\":1},{\"$type\":\"SingerState\",\"id\":2,\"main_singer_id\":1}]}";
        string actual = JsonConvert.SerializeObject(singerInfo, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestSingerInfoDeserializer()
    {
        const string json = "{\"singers\":[{\"$type\":\"Singer\",\"id\":1},{\"$type\":\"SingerState\",\"id\":2,\"main_singer_id\":1}]}";

        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        singerInfo.AddSingerState(singer);

        var actual = JsonConvert.DeserializeObject<SingerInfo>(json, CreateSettings())!;
        Assert.AreEqual(singerInfo.Singers.Count, actual.Singers.Count);
        Assert.AreEqual(singerInfo.Singers[0].ID, singerInfo.Singers[0].ID); // test singer
        Assert.AreEqual(singerInfo.Singers[1].ID, singerInfo.Singers[1].ID); // test sub-singer.
    }
}
