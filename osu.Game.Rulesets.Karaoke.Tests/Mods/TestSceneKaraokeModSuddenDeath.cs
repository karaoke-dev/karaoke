// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Replays;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods;

[Ignore("Scoring is not implemented.")]
public partial class TestSceneKaraokeModSuddenDeath : ModFailConditionTestScene
{
    protected override Ruleset CreatePlayerRuleset() => new KaraokeRuleset();

    private readonly Lyric referencedLyric = TestCaseNoteHelper.CreateLyricForNote(1, "カラオケ", 1000, 1000);

    public TestSceneKaraokeModSuddenDeath()
        : base(new KaraokeModSuddenDeath())
    {
    }

    [Test]
    public void TestGreatHit() => CreateModTest(new ModTestData
    {
        Mod = new KaraokeModSuddenDeath(),
        PassCondition = () => ((ModFailConditionTestPlayer)Player).CheckFailed(false),
        Autoplay = false,
        CreateBeatmap = () => new Beatmap
        {
            HitObjects = new List<HitObject>
            {
                referencedLyric,
                new Note
                {
                    ReferenceLyricId = referencedLyric.ID,
                    Tone = new Tone(0),
                },
            },
        },
        ReplayFrames = new List<ReplayFrame>
        {
            new KaraokeReplayFrame(1000, 0),
            new KaraokeReplayFrame(2000, 0),
        },
    });

    [Test]
    public void TestBreakOnHoldNote() => CreateModTest(new ModTestData
    {
        Mod = new KaraokeModSuddenDeath(),
        PassCondition = () => ((ModFailConditionTestPlayer)Player).CheckFailed(true) && Player.Results.Count == 2,
        Autoplay = false,
        CreateBeatmap = () => new Beatmap
        {
            HitObjects = new List<HitObject>
            {
                referencedLyric,
                new Note
                {
                    ReferenceLyricId = referencedLyric.ID,
                    Tone = new Tone(0),
                },
            },
        },
        ReplayFrames = new List<ReplayFrame>
        {
            new KaraokeReplayFrame(0, -1),
        },
    });
}
