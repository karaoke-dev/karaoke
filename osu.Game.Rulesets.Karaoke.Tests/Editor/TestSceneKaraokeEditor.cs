// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Screens;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    public class TestSceneKaraokeEditor : ScreenTestScene<KaraokeEditor>
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        [Cached]
        private readonly KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override KaraokeEditor CreateScreen() => new();

        private DialogOverlay dialogOverlay;
        private LyricCheckerManager lyricCheckerManager;

        public TestSceneKaraokeEditor()
        {
            var beatmap = new TestKaraokeBeatmap(new KaraokeRuleset().RulesetInfo);
            var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
            lyricEditorConfigManager = new KaraokeRulesetLyricEditorConfigManager();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay(),
                lyricCheckerManager = new LyricCheckerManager()
            });

            Dependencies.Cache(dialogOverlay);
            Dependencies.Cache(lyricCheckerManager);
        }
    }
}
