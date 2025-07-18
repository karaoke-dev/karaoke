// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class KaraokeSkinElementConverterTest : BaseSingleConverterTest<KaraokeSkinElementConverter>
{
    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new ColourConverter();
        yield return new Vector2Converter();
        yield return new ShaderConverter();
        yield return new FontUsageConverter();
    }

    [Test]
    public void TestLyricConfigSerializer()
    {
        var lyricConfig = LyricFontInfo.CreateDefault();

        const string expected =
            "{\"$type\":0,\"name\":\"Default\",\"smart_horizon\":2,\"lyrics_interval\":4,\"ruby_interval\":2,\"romanisation_interval\":2,\"ruby_alignment\":2,\"romanisation_alignment\":2,\"ruby_margin\":4,\"romanisation_margin\":4,\"main_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\",\"size\":48.0},\"ruby_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"},\"romanisation_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"}}";
        string actual = JsonConvert.SerializeObject(lyricConfig, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestLyricConfigDeserialize()
    {
        const string json =
            "{\"$type\":0,\"name\":\"Default\",\"smart_horizon\":2,\"lyrics_interval\":4,\"ruby_interval\":2,\"romanisation_interval\":2,\"ruby_alignment\":2,\"romanisation_alignment\":2,\"ruby_margin\":4,\"romanisation_margin\":4,\"main_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\",\"size\":48.0},\"ruby_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"},\"romanisation_text_font\":{\"family\":\"Torus\",\"weight\":\"Bold\"}}";

        var expected = LyricFontInfo.CreateDefault();
        var actual = (LyricFontInfo)JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }

    [Test]
    public void TestNoteStyleSerializer()
    {
        var lyricConfig = NoteStyle.CreateDefault();

        const string expected = "{\"$type\":1,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}";
        string actual = JsonConvert.SerializeObject(lyricConfig, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestNoteStyleDeserializer()
    {
        const string json = "{\"$type\":1,\"name\":\"Default\",\"note_color\":\"#44AADD\",\"blink_color\":\"#FF66AA\",\"text_color\":\"#FFFFFF\",\"bold_text\":true}";

        var expected = NoteStyle.CreateDefault();
        var actual = (NoteStyle)JsonConvert.DeserializeObject<IKaraokeSkinElement>(json, CreateSettings())!;
        ObjectAssert.ArePropertyEqual(expected, actual);
    }
}
