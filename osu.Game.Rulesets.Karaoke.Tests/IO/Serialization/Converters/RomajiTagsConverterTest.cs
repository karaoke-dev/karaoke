// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class RomajiTagsConverterTest : BaseSingleConverterTest<RomajiTagsConverter>
{
    protected override JsonConverter[] CreateExtraConverts()
        => new JsonConverter[]
        {
            new RomajiTagConverter(),
        };

    [Test]
    public void TestSerialize()
    {
        var timeTags = new[]
        {
            new RomajiTag
            {
                StartIndex = 2,
                EndIndex = 2,
                Text = "ji"
            },
            new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "roma"
            },
        };

        const string expected = "[\"[0,1]:roma\",\"[2]:ji\"]";
        string actual = JsonConvert.SerializeObject(timeTags, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestDeserialize()
    {
        const string json = "[\"[2]:ji\",\"[0,1]:roma\"]";

        var expected = new[]
        {
            new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "roma"
            },
            new RomajiTag
            {
                StartIndex = 2,
                EndIndex = 2,
                Text = "ji"
            },
        };
        var actual = JsonConvert.DeserializeObject<RomajiTag[]>(json, CreateSettings()) ?? throw new InvalidCastException();
        TextTagAssert.ArePropertyEqual(expected, actual);
    }
}
