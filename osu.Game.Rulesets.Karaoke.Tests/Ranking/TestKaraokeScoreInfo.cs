// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Game.Users;

namespace osu.Game.Rulesets.Karaoke.Tests.Ranking
{
    public class TestKaraokeScoreInfo : ScoreInfo
    {
        public TestKaraokeScoreInfo()
        {
            var ruleset = new KaraokeRuleset().RulesetInfo;

            User = new User
            {
                Id = 1030492,
                Username = "andy840119",
                CoverUrl = "https://osu.ppy.sh/images/headers/profile-covers/c3.jpg",
            };

            Beatmap = new TestKaraokeBeatmap(ruleset).BeatmapInfo;
            Ruleset = ruleset;
            RulesetID = ruleset.ID ?? 0;
            Mods = new Mod[] { new KaraokeModFlashlight(), new KaraokeModSnow() };

            TotalScore = 2845370;
            Accuracy = 0.95;
            MaxCombo = 999;
            Rank = ScoreRank.S;
            Date = DateTimeOffset.Now;

            Statistics[HitResult.Miss] = 1;
            Statistics[HitResult.Meh] = 50;
            Statistics[HitResult.Good] = 100;
            Statistics[HitResult.Great] = 300;
        }
    }
}
