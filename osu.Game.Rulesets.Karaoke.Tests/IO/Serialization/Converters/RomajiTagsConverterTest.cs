// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
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
                    EndIndex = 3,
                    Text = "ji"
                },
                new RomajiTag
                {
                    StartIndex = 1,
                    EndIndex = 2,
                    Text = "roma"
                },
            };

            const string expected = "[\"[1,2]:roma\",\"[2,3]:ji\"]";
            string actual = JsonConvert.SerializeObject(timeTags, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDeserialize()
        {
            const string json = "[\"[2,3]:ji\",\"[1,2]:roma\"]";

            var expected = new[]
            {
                new RomajiTag
                {
                    StartIndex = 1,
                    EndIndex = 2,
                    Text = "roma"
                },
                new RomajiTag
                {
                    StartIndex = 2,
                    EndIndex = 3,
                    Text = "ji"
                },
            };
            var actual = JsonConvert.DeserializeObject<RomajiTag[]>(json, CreateSettings());
            TextTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
