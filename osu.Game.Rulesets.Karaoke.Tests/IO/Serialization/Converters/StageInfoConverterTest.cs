// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class StageInfoConverterTest : BaseSingleConverterTest<StageInfoConverter>
{
    [Test]
    public void TestClassicStageInfoSerializer()
    {
        var stageInfo = new ClassicStageInfo();

        const string expected = "{\"$type\":\"classic\",\"style_category\":{},\"lyric_layout_definition\":{},\"lyric_layout_category\":{},\"lyric_timing_info\":{\"timings\":[],\"mappings\":{}}}";
        string actual = JsonConvert.SerializeObject(stageInfo, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    [Ignore("Will fix this issue until able to save/load the customized beatmap.")]
    public void TestClassicStageInfoDeserializer()
    {
        const string json = "{\"$type\":\"classic\",\"style_category\":{},\"lyric_layout_definition\":{},\"lyric_layout_category\":{},\"lyric_timing_info\":{\"timings\":[],\"mappings\":{}}}";

        var expected = new ClassicStageInfo();
        var actual = (ClassicStageInfo)JsonConvert.DeserializeObject<StageInfo>(json, CreateSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }
}
