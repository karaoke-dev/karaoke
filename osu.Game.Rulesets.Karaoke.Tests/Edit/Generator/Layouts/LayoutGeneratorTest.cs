// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Layouts;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.Layouts
{
    [TestFixture]
    public class LayoutGeneratorTest
    {
        [TestCase(new[] { "枯れた世界に...", "強く生きてい...", "あぁ、...", "光なん..." }, new[] { 0, 1, 0, 1 })]
        [TestCase(new[] { "", "", "", "" }, new[] { 0, 1, 0, 1 })] // should apply layout event lyric is empty.
        [TestCase(new string[] { }, new int[] { })]
        [TestCase(null, null)] // should not crash in null.
        public void TestApplyLayoutIndex(string[] texts, int[] layoutIds)
        {
            var lyrics = texts.Select(x => new Lyric { Text = x }).ToList();

            var generator = new LayoutGenerator(generatorConfig());
            generator.ApplyLayout(lyrics, LocalLayout.CycleTwo);

            Assert.AreEqual(lyrics.Select(x => x.LayoutIndex).ToArray(), layoutIds);
        }

        public void TestApplyLayoutTime(string lyrics, string times)
        {
            // todo : should make helper to fast generate list of lyric.
        }

        private LayoutGeneratorConfig generatorConfig()
        {
            return new LayoutGeneratorConfig();
        }
    }
}
