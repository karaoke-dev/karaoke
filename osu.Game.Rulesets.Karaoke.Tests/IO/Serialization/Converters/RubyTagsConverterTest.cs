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
    public class RubyTagsConverterTest : BaseSingleConverterTest<RubyTagsConverter>
    {
        protected override JsonConverter[] CreateExtraConverts()
            => new JsonConverter[]
            {
                new RubyTagConverter(),
            };

        [Test]
        public void TestSerialize()
        {
            var timeTags = new[]
            {
                new RubyTag
                {
                    StartIndex = 2,
                    EndIndex = 3,
                    Text = "ビ"
                },
                new RubyTag
                {
                    StartIndex = 1,
                    EndIndex = 2,
                    Text = "ル"
                },
            };

            const string expected = "[\"[1,2]:ル\",\"[2,3]:ビ\"]";
            string actual = JsonConvert.SerializeObject(timeTags, CreateSettings());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDeserialize()
        {
            const string json = "[\"[2,3]:ビ\",\"[1,2]:ル\"]";

            var expected = new[]
            {
                new RubyTag
                {
                    StartIndex = 1,
                    EndIndex = 2,
                    Text = "ル"
                },
                new RubyTag
                {
                    StartIndex = 2,
                    EndIndex = 3,
                    Text = "ビ"
                },
            };
            var actual = JsonConvert.DeserializeObject<RubyTag[]>(json, CreateSettings()) ?? throw new InvalidCastException();
            TextTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
