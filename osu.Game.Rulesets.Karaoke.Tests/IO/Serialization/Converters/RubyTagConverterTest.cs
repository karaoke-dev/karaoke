// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    public class RubyTagConverterTest : BaseSingleConverterTest<RubyTagConverter>
    {
        [TestCase(1, 2, "ルビ", "[1,2]:ルビ")]
        [TestCase(1, 1, "ルビ", "[1,1]:ルビ")]
        [TestCase(-1, -2, "ルビ", "[-1,-2]:ルビ")] // Should not check ruby is out of range in here.
        [TestCase(1, 2, "::[][]", "[1,2]:::[][]")]
        [TestCase(1, 2, null, "[1,2]:")]
        [TestCase(1, 2, "", "[1,2]:")]
        public void TestSerialize(int startIndex, int endIndex, string text, string json)
        {
            var rubyTag = new RubyTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };

            string expected = $"\"{json}\"";
            string actual = JsonConvert.SerializeObject(rubyTag, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [TestCase("[1,2]:ルビ", 1, 2, "ルビ")]
        [TestCase("[1,1]:ルビ", 1, 1, "ルビ")]
        [TestCase("[-1,-2]:ルビ", -1, -2, "ルビ")] // Should not check ruby is out of range in here.
        [TestCase("[1,2]:::[][]", 1, 2, "::[][]")]
        [TestCase("[1,2]:", 1, 2, null)]
        [TestCase("[1,2]:null", 1, 2, "null")]
        [TestCase("", 0, 0, null)] // Test deal with format is not right below.
        [TestCase("[1,2]", 0, 0, null)]
        [TestCase("[1,]", 0, 0, null)]
        [TestCase("[,1]", 0, 0, null)]
        [TestCase("[]", 0, 0, null)]
        public void TestDeserialize(string json, int startIndex, int endIndex, string text)
        {
            var expected = new RubyTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
            var actual = JsonConvert.DeserializeObject<RubyTag>($"\"{json}\"", CreateSettings());
            TextTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
