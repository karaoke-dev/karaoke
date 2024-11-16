// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods;

public partial class TestSceneKaraokeModPerfect : ModFailConditionTestScene
{
    protected override Ruleset CreatePlayerRuleset() => new KaraokeRuleset();

    public TestSceneKaraokeModPerfect()
        : base(new KaraokeModPerfect())
    {
    }

    // TODO : test case = false will be added after scoring system is implemented.
    [Ignore("Scoring should judgement by note, not lyric.")]
    public void TestLyric(bool shouldMiss) => CreateHitObjectTest(new HitObjectTestData(new Lyric
    {
        Text = "カラオケ!",
        TimeTags = new []
        {
            new TimeTag(new TextIndex(), 1000),
            new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 2000),
        },
    }), shouldMiss);
}
