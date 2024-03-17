// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class RubyTagsConverterTest : BaseSingleConverterTest<RubyTagsConverter>
{
    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new RubyTagConverter();
    }

    [Test]
    public void TestSerialize()
    {
        var timeTags = new[]
        {
            new RubyTag
            {
                StartIndex = 1,
                EndIndex = 1,
                Text = "ビ",
            },
            new RubyTag
            {
                StartIndex = 0,
                EndIndex = 0,
                Text = "ル",
            },
        };

        const string expected = "[\"[0]:ル\",\"[1]:ビ\"]";
        string actual = JsonConvert.SerializeObject(timeTags, CreateSettings());
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestDeserialize()
    {
        const string json = "[\"[1]:ビ\",\"[0]:ル\"]";

        var expected = new[]
        {
            new RubyTag
            {
                StartIndex = 0,
                EndIndex = 0,
                Text = "ル",
            },
            new RubyTag
            {
                StartIndex = 1,
                EndIndex = 1,
                Text = "ビ",
            },
        };
        var actual = JsonConvert.DeserializeObject<RubyTag[]>(json, CreateSettings()) ?? throw new InvalidCastException();
        RubyTagAssert.ArePropertyEqual(expected, actual);
    }
}
