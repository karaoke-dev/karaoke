// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Scoring;
using osu.Game.Screens;
using osu.Game.Screens.Play;
using osu.Game.Screens.Ranking;
using osu.Game.Screens.Ranking.Statistics;
using osu.Game.Tests.Visual;
using osuTK.Input;

namespace osu.Game.Rulesets.Karaoke.Tests.Ranking
{
    [TestFixture]
    public class TestSceneResultsScreen : OsuManualInputManagerTestScene
    {
        protected override IBeatmap CreateBeatmap(RulesetInfo ruleset) => new TestKaraokeBeatmap(ruleset);

        private TestResultsScreen createResultsScreen() => new TestResultsScreen(new TestKaraokeScoreInfo
        {
            HitEvents = TestSceneHitEventTimingDistributionGraph.CreateDistributedHitEvents()
        });

        [Test]
        public void TestShowStatisticsAndClickOtherPanel()
        {
            TestResultsScreen screen = null;

            AddStep("load results", () => Child = new TestResultsContainer(screen = createResultsScreen()));
            AddUntilStep("wait for loaded", () => screen.IsLoaded);

            ScorePanel expandedPanel = null;
            ScorePanel contractedPanel = null;

            AddStep("click expanded panel then contracted panel", () =>
            {
                expandedPanel = this.ChildrenOfType<ScorePanel>().Single(p => p.State == PanelState.Expanded);
                InputManager.MoveMouseTo(expandedPanel);
                InputManager.Click(MouseButton.Left);

                contractedPanel = this.ChildrenOfType<ScorePanel>().First(p => p.State == PanelState.Contracted && p.ScreenSpaceDrawQuad.TopLeft.X > screen.ScreenSpaceDrawQuad.TopLeft.X);
                InputManager.MoveMouseTo(contractedPanel);
                InputManager.Click(MouseButton.Left);
            });

            AddAssert("statistics shown", () => this.ChildrenOfType<StatisticsPanel>().Single().State.Value == Visibility.Visible);

            AddAssert("contracted panel still contracted", () => contractedPanel.State == PanelState.Contracted);
            AddAssert("expanded panel still expanded", () => expandedPanel.State == PanelState.Expanded);
        }

        private class TestResultsContainer : Container
        {
            [Cached(typeof(Player))]
            private readonly Player player = new TestPlayer();

            public TestResultsContainer(IScreen screen)
            {
                RelativeSizeAxes = Axes.Both;
                OsuScreenStack stack;

                InternalChild = stack = new OsuScreenStack
                {
                    RelativeSizeAxes = Axes.Both,
                };

                stack.Push(screen);
            }
        }

        private class TestResultsScreen : ResultsScreen
        {
            public TestResultsScreen(ScoreInfo score)
                : base(score)
            {
            }

            protected override APIRequest FetchScores(Action<IEnumerable<ScoreInfo>> scoresCallback)
            {
                var scores = new List<ScoreInfo>();

                for (int i = 0; i < 20; i++)
                {
                    var score = new TestKaraokeScoreInfo();
                    score.TotalScore += 10 - i;
                    scores.Add(score);
                }

                scoresCallback?.Invoke(scores);

                return null;
            }
        }
    }
}
