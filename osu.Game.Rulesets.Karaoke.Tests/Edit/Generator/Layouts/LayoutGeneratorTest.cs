// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Layouts;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.Layouts
{
    [TestFixture]
    public class LayoutGeneratorTest
    {
        [TestCase(new[] { "枯れた世界に...", "強く生きてい...", "あぁ、...", "光なん..." }, new[] { 1, 2, 1, 2 })]
        [TestCase(new[] { "", "", "", "" }, new[] { 1, 2, 1, 2 })] // should apply layout event lyric is empty.
        [TestCase(new string[] { }, new int[] { })]
        [TestCase(null, null)] // should not crash in null.
        public void TestApplyLayoutIndex(string[] texts, int[] actualLayoutIds)
        {
            var lyrics = texts?.Select(x => new Lyric { Text = x }).ToArray();

            var generator = new LayoutGenerator(generatorConfig());
            generator.ApplyLayout(lyrics);

            Assert.AreEqual(lyrics?.Select(x => x.LayoutIndex).ToArray(), actualLayoutIds);
        }

        [TestCase(new[] { "[1000,3000]:枯れた世界に...", "[6000,8000]:枯れた世界に...", "[8000,12000]:あぁ、..." },
            new[] { "[1000,3000]", "[6000,8000]", "[4000,12000]" })] // todo : second lyric start time should be 6000 but can be ignored now.
        public void TestApplyLayoutTime(string[] lyricTexts, string[] actualTimes)
        {
            var lyrics = TestCaseTagHelper.ParseLyrics(lyricTexts);

            var generator = new LayoutGenerator(generatorConfig());
            generator.ApplyLayout(lyrics);

            Assert.AreEqual(lyrics.Select(x => $"[{x.StartTime},{x.EndTime}]").ToArray(), actualTimes);
        }

        private static LayoutGeneratorConfig generatorConfig()
        {
            return new()
            {
                NewLyricLineTime = 15000,
            };
        }
    }
}
