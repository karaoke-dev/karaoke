// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Game.Screens.Ranking.Statistics;
using osu.Game.Tests;
using osu.Game.Tests.Visual;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests.Ranking
{
    public class TestSceneStatisticsPanel : OsuTestScene
    {
        [Test]
        public void TestScoreWithStatistics()
        {
            var score = new TestScoreInfo(new KaraokeRuleset().RulesetInfo)
            {
                //TODO : add hit event.
                HitEvents = new List<HitEvent>()
            };

            loadPanel(score);
        }

        [Test]
        public void TestScoreWithoutStatistics()
        {
            loadPanel(new TestScoreInfo(new KaraokeRuleset().RulesetInfo));
        }

        [Test]
        public void TestNullScore()
        {
            loadPanel(null);
        }

        private void loadPanel(ScoreInfo score) => AddStep("load panel", () =>
        {
            Child = new StatisticsPanel
            {
                RelativeSizeAxes = Axes.Both,
                State = { Value = Visibility.Visible },
                Score = { Value = score }
            };
        });
    }
}
