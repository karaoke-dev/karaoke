// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Tests.Scoring;

[TestFixture]
public class KaraokeScoreMultiplierCalculatorTest
{
    [TestCase(1.0)]
    [TestCase(0.5, typeof(KaraokeModNoFail))]
    [TestCase(1.06, typeof(KaraokeModHiddenNote))]
    [TestCase(0, typeof(KaraokeModPractice))]
    [TestCase(0, typeof(KaraokeModDisableNote))]
    [TestCase(0.53, typeof(KaraokeModNoFail), typeof(KaraokeModHiddenNote))]
    public void TestMultiplier(double expected, params System.Type[] modTypes)
    {
        var calculator = new KaraokeScoreMultiplierCalculator(new ScoreMultiplierContext(new BeatmapDifficulty()));
        var mods = new Mod[modTypes.Length];

        for (int i = 0; i < modTypes.Length; i++)
            mods[i] = (Mod)System.Activator.CreateInstance(modTypes[i])!;

        Assert.That(calculator.CalculateFor(mods), Is.EqualTo(expected));
    }
}
