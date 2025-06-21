// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

public class TranslationConverterTest : BaseSingleConverterTest<TranslationConverter>
{
    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new CultureInfoConverter();
    }

    [Test]
    public void TestSerialize()
    {
        var translations = new Dictionary<CultureInfo, string>
        {
            { new CultureInfo("en-US"), "karaoke" },
            { new CultureInfo("Ja-jp"), "カラオケ" },
        };

        const string expected = "[{\"key\":1033,\"value\":\"karaoke\"},{\"key\":1041,\"value\":\"カラオケ\"}]";
        string actual = JsonConvert.SerializeObject(translations, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestDeserialize()
    {
        const string json = "[{\"key\":1033,\"value\":\"karaoke\"},{\"key\":1041,\"value\":\"カラオケ\"}]";

        var expected = new Dictionary<CultureInfo, string>
        {
            { new CultureInfo("en-US"), "karaoke" },
            { new CultureInfo("Ja-jp"), "カラオケ" },
        };

        var actual = JsonConvert.DeserializeObject<Dictionary<CultureInfo, string>>(json, CreateSettings()) ?? throw new InvalidCastException();
        Assert.That(actual, Is.EquivalentTo(expected));

        var actualWithInterface = JsonConvert.DeserializeObject<IDictionary<CultureInfo, string>>(json, CreateSettings()) ?? throw new InvalidCastException();
        Assert.That(actualWithInterface, Is.EquivalentTo(expected));
    }
}
