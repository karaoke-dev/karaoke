// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Tests.Scoring;

[TestFixture]
public class KaraokeNoteHitWindowsTest
{
    [Test]
    public void TestGreatWindowIsAvailableForDifficultyCalculation()
    {
        var hitWindows = new KaraokeNoteHitWindows();
        hitWindows.SetDifficulty(5);

        Assert.That(hitWindows.WindowFor(HitResult.Great), Is.EqualTo(hitWindows.WindowFor(HitResult.Perfect)));
    }
}
