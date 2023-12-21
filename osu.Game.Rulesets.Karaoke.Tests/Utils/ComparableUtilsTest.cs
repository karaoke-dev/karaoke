// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils;

public class ComparableUtilsTest
{
    [TestCase("{\"A\":0,\"B\":0,\"C\":\" \"}", "{\"A\":0,\"B\":0,\"C\":\" \"}", 0)] // should be the same if two values are the same.
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", 0)] // should be the same if two values are the same.
    [TestCase("{\"A\":0,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", -1)]
    [TestCase("{\"A\":1,\"B\":0,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", -1)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", -49)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":0,\"B\":1,\"C\":\"1\"}", 1)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":0,\"C\":\"1\"}", 1)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"\"}", 49)]
    public void TestCompare(string leftObjectProperty, string rightObjectProperty, int expected)
    {
        var leftObject = JsonConvert.DeserializeObject<TestObject>(leftObjectProperty);
        var rightObject = JsonConvert.DeserializeObject<TestObject>(rightObjectProperty);
        int actual = ComparableUtils.Compare(leftObject, rightObject,
            (left, right) => left.A.CompareTo(right.A),
            (left, right) => left.B.CompareTo(right.B),
            (left, right) => string.Compare(left.C, right.C, StringComparison.Ordinal)); // using different comparator might get different compare number.
        Assert.AreEqual(expected, actual);
    }

    [TestCase("{\"A\":0,\"B\":0,\"C\":\" \"}", "{\"A\":0,\"B\":0,\"C\":\" \"}", 0)] // should be the same if two values are the same.
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", 0)] // should be the same if two values are the same.
    [TestCase("{\"A\":0,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", -1)]
    [TestCase("{\"A\":1,\"B\":0,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", -1)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"\"}", "{\"A\":1,\"B\":1,\"C\":\"1\"}", -1)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":0,\"B\":1,\"C\":\"1\"}", 1)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":0,\"C\":\"1\"}", 1)]
    [TestCase("{\"A\":1,\"B\":1,\"C\":\"1\"}", "{\"A\":1,\"B\":1,\"C\":\"\"}", 1)]
    public void TestCompareByProperty(string leftObjectProperty, string rightObjectProperty, int expected)
    {
        var leftObject = JsonConvert.DeserializeObject<TestObject>(leftObjectProperty);
        var rightObject = JsonConvert.DeserializeObject<TestObject>(rightObjectProperty);
        int actual = ComparableUtils.CompareByProperty(leftObject, rightObject,
            t => t.A,
            t => t.B,
            t => t.C);
        Assert.AreEqual(expected, actual);
    }

    private class TestObject
    {
        [JsonProperty]
        public int A { get; set; }

        [JsonProperty]
        public double B { get; set; }

        [JsonProperty]
        public string C { get; set; } = string.Empty;
    }
}
