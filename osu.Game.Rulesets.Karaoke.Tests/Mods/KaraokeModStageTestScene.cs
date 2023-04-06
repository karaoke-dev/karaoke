// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods;

public abstract partial class KaraokeModStageTestScene<TModStage, TStageInfo> : ModTestScene
    where TModStage : ModStage<TStageInfo>, new()
    where TStageInfo : StageInfo
{
    protected override Ruleset CreatePlayerRuleset() => new KaraokeRuleset();

    [Test]
    public void TestCreateModWithStage()
    {
        CreateModTest(new ModTestData
        {
            Mod = new TModStage(),
            Beatmap = new TestKaraokeBeatmap(Ruleset.Value),
            PassCondition = () => true
        });
    }

    [Test]
    public void TestCreateModWithoutStage()
    {
        CreateModTest(new ModTestData
        {
            Mod = new TModStage(),
            // todo: add the stage info to beatmap.
            Beatmap = new TestKaraokeBeatmap(Ruleset.Value),
            PassCondition = () => true
        });
    }
}
