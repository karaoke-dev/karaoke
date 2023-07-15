// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class RubyTagConverterTest : BaseSingleConverterTest<RubyTagConverter>
{
    [TestCase(0, 1, "ルビ", "[0,1]:ルビ")]
    [TestCase(0, 0, "ルビ", "[0]:ルビ")]
    [TestCase(-1, -2, "ルビ", "[-1,-2]:ルビ")] // Should not check ruby is out of range in here.
    [TestCase(0, 1, "::[][]", "[0,1]:::[][]")]
    [TestCase(0, 1, "", "[0,1]:")]
    public void TestSerialize(int startIndex, int endIndex, string text, string json)
    {
        var rubyTag = new RubyTag
        {
            StartIndex = startIndex,
            EndIndex = endIndex,
            Text = text,
        };

        string expected = $"\"{json}\"";
        string actual = JsonConvert.SerializeObject(rubyTag, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [TestCase("[0,1]:ルビ", 0, 1, "ルビ")]
    [TestCase("[0]:ルビ", 0, 0, "ルビ")]
    [TestCase("[-1,-2]:ルビ", -1, -2, "ルビ")] // Should not check ruby is out of range in here.
    [TestCase("[0,1]:::[][]", 0, 1, "::[][]")]
    [TestCase("[0,1]:", 0, 1, null)] // todo: expected value should be string.empty.
    [TestCase("[0,1]:null", 0, 1, "null")]
    [TestCase("", 0, 0, "")] // Test deal with format is not right below.
    [TestCase("[0,1]", 0, 0, "")]
    [TestCase("[1,]", 0, 0, "")]
    [TestCase("[,1]", 0, 0, "")]
    [TestCase("[]", 0, 0, "")]
    public void TestDeserialize(string json, int startIndex, int endIndex, string text)
    {
        var expected = new RubyTag
        {
            StartIndex = startIndex,
            EndIndex = endIndex,
            Text = text,
        };
        var actual = JsonConvert.DeserializeObject<RubyTag>($"\"{json}\"", CreateSettings()) ?? throw new InvalidCastException();
        TextTagAssert.ArePropertyEqual(expected, actual);
    }
}
