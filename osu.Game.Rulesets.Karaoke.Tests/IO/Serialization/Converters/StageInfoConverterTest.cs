// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class StageInfoConverterTest : BaseSingleConverterTest<StageInfoConverter>
{
    [Test]
    public void TestClassicStageInfoSerializer()
    {
        var stageInfo = new ClassicStageInfo();

        const string expected =
            "{\"$type\":\"classic\",\"style_category\":{},\"stage_definition\":{\"border_width\":25.0,\"border_height\":25.0,\"fade_in_time\":150.0,\"fade_out_time\":150.0,\"fade_in_easing\":22,\"fade_out_easing\":22,\"lyric_scale\":2.0,\"line_height\":72.0,\"first_lyric_start_time_offset\":1000.0,\"lyric_end_time_offset\":300.0,\"last_lyric_end_time_offset\":10000.0},\"lyric_layout_category\":{},\"lyric_timing_info\":{\"timings\":[],\"mappings\":{}}}";
        string actual = JsonConvert.SerializeObject(stageInfo, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    [Ignore("todo: need to implement the serializer for the ClassicLyricTimingInfo")]
    public void TestClassicStageInfoDeserializer()
    {
        const string json =
            "{\"$type\":\"classic\",\"style_category\":{},\"stage_definition\":{\"border_width\":25.0,\"border_height\":25.0,\"fade_in_time\":150.0,\"fade_out_time\":150.0,\"fade_in_easing\":22,\"fade_out_easing\":22,\"lyric_scale\":2.0,\"line_height\":72.0,\"first_lyric_start_time_offset\":1000.0,\"lyric_end_time_offset\":300.0,\"last_lyric_end_time_offset\":10000.0},\"lyric_layout_category\":{},\"lyric_timing_info\":{\"timings\":[],\"mappings\":{}}}";

        var expected = new ClassicStageInfo();
        var actual = (ClassicStageInfo)JsonConvert.DeserializeObject<StageInfo>(json, CreateSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }

    [Test]
    public void TestPreviewStageInfoSerializer()
    {
        var stageInfo = new PreviewStageInfo();

        const string expected =
            "{\"$type\":\"preview\",\"stage_definition\":{\"blue_level\":0.5,\"dim_level\":0.5,\"fading_time\":300.0,\"fading_offset_position\":64.0,\"fade_in_easing\":22,\"fade_out_easing\":22,\"moving_in_easing\":22,\"move_out_easing\":22,\"inactive_alpha\":0.3,\"active_time\":350.0,\"active_easing\":22,\"number_of_lyrics\":5,\"lyric_height\":64.0,\"line_moving_time\":500.0,\"line_moving_easing\":22,\"line_moving_offset_time\":50.0}}";
        string actual = JsonConvert.SerializeObject(stageInfo, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestPreviewStageInfoDeserializer()
    {
        const string json =
            "{\"$type\":\"preview\",\"stage_definition\":{\"blue_level\":0.5,\"dim_level\":0.5,\"fading_time\":300.0,\"fading_offset_position\":64.0,\"fade_in_easing\":22,\"fade_out_easing\":22,\"moving_in_easing\":22,\"move_out_easing\":22,\"inactive_alpha\":0.3,\"active_time\":350.0,\"active_easing\":22,\"number_of_lyrics\":5,\"lyric_height\":64.0,\"line_moving_time\":500.0,\"line_moving_easing\":22,\"line_moving_offset_time\":50.0}}";

        var expected = new PreviewStageInfo();
        var actual = (PreviewStageInfo)JsonConvert.DeserializeObject<StageInfo>(json, CreateSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }
}
