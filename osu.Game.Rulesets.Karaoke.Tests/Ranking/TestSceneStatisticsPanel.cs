// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Scoring;
using osu.Game.Screens.Ranking.Statistics;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Ranking
{
    public class TestSceneStatisticsPanel : OsuTestScene
    {
        [Test]
        public void TestScoreWithStatistics()
        {
            var score = new TestKaraokeScoreInfo
            {
                HitEvents = TestSceneHitEventTimingDistributionGraph.CreateDistributedHitEvents()
            };

            loadPanel(score);
        }

        [Test]
        public void TestScoreWithoutStatistics()
        {
            loadPanel(new TestKaraokeScoreInfo());
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
