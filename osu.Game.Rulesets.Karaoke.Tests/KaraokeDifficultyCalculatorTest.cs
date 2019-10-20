// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Difficulty;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class KaraokeDifficultyCalculatorTest : DifficultyCalculatorTest
    {
        public KaraokeDifficultyCalculatorTest()
        {
            // It's a tricky to let lazer to read karaoke testing beatmap
            KaroakeLegacyBeatmapDecoder.Register();
        }

        protected override string ResourceAssembly => "osu.Game.Rulesets.Karaoke.Tests";

        
        [TestCase(4.7025709907802105, "karaoke-file-samples")]
        [TestCase(4.7048320627456297, "karaoke-file-samples-without-note")]
        public void Test(double expected, string name)
            => base.Test(expected, name);

        protected override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) => new KaraokeDifficultyCalculator(new KaraokeRuleset(), beatmap);

        protected override Ruleset CreateRuleset() => new KaraokeRuleset();
    }
}
