// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Setup;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneSetupScreen : EditorClockTestScene
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Blue);

        public TestSceneSetupScreen()
        {
            editorBeatmap = new EditorBeatmap(new KaraokeBeatmap
            {
                BeatmapInfo =
                {
                    Ruleset = new KaraokeRuleset().RulesetInfo,
                },
            });
        }

        [Test]
        public void TestKaraoke() => runForRuleset(new KaraokeRuleset().RulesetInfo);

        private void runForRuleset(RulesetInfo rulesetInfo)
        {
            AddStep("create screen", () =>
            {
                editorBeatmap.BeatmapInfo.Ruleset = rulesetInfo;

                Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

                Child = new SetupScreen
                {
                    State = { Value = Visibility.Visible },
                };
            });
        }
    }
}
