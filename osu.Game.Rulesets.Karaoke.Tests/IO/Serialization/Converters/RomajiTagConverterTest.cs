// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    [TestFixture]
    public class RomajiTagConverterTest : BaseSingleConverterTest<RomajiTagConverter>
    {
        [TestCase(1, 2, "romaji", "[1,2]:romaji")]
        [TestCase(1, 1, "romaji", "[1,1]:romaji")]
        [TestCase(-1, -2, "romaji", "[-1,-2]:romaji")] // Should not check romaji is out of range in here.
        [TestCase(1, 2, "::[][]", "[1,2]:::[][]")]
        [TestCase(1, 2, "", "[1,2]:")]
        public void TestSerialize(int startIndex, int endIndex, string text, string json)
        {
            var romajiTag = new RomajiTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };

            string expected = $"\"{json}\"";
            string actual = JsonConvert.SerializeObject(romajiTag, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[1,2]:romaji", 1, 2, "romaji")]
        [TestCase("[1,1]:romaji", 1, 1, "romaji")]
        [TestCase("[-1,-2]:romaji", -1, -2, "romaji")] // Should not check romaji is out of range in here.
        [TestCase("[1,2]:::[][]", 1, 2, "::[][]")]
        [TestCase("[1,2]:", 1, 2, null)] // todo: expected value should be string.empty.
        [TestCase("[1,2]:null", 1, 2, "null")]
        [TestCase("", 0, 0, "")] // Test deal with format is not right below.
        [TestCase("[1,2]", 0, 0, "")]
        [TestCase("[1,]", 0, 0, "")]
        [TestCase("[,1]", 0, 0, "")]
        [TestCase("[]", 0, 0, "")]
        public void TestDeserialize(string json, int startIndex, int endIndex, string text)
        {
            var expected = new RomajiTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
            var actual = JsonConvert.DeserializeObject<RomajiTag>($"\"{json}\"", CreateSettings()) ?? throw new InvalidCastException();
            TextTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
