// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Difficulty;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Difficulty
{
    [TestFixture]
    public class KaraokeDifficultyCalculatorTest : DifficultyCalculatorTest
    {
        public KaraokeDifficultyCalculatorTest()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();
        }

        protected override string ResourceAssembly => "osu.Game.Rulesets.Karaoke.Tests";

        [TestCase(1.7028276483104616d, 936, "karaoke-file-samples")]
        [TestCase(1.6935488434919981d, 924, "karaoke-file-samples-without-note")]
        public void Test(double expectedStarRating, int expectedMaxCombo, string name)
            => base.Test(expectedStarRating, expectedMaxCombo, name);

        protected override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new KaraokeDifficultyCalculator(new KaraokeRuleset().RulesetInfo, beatmap);

        protected override Ruleset CreateRuleset() => new KaraokeRuleset();
    }
}
