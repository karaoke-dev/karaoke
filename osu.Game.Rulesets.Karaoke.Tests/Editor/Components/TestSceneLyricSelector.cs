// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Components
{
    public class TestSceneLyricSelector : OsuManualInputManagerTestScene
    {
        [Cached(typeof(EditorBeatmap))]
        private readonly EditorBeatmap editorBeatmap;

        public TestSceneLyricSelector()
        {
            var beatmap = new TestKaraokeBeatmap(new KaraokeRuleset().RulesetInfo);
            var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
        }

        [Test]
        public void TestAllFiles()
        {
            AddStep("show the selector", () =>
            {
                var language = new Bindable<Lyric?>();
                Child = new LyricSelector
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.8f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Current = language
                };
            });
        }
    }
}
