// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;
using System.IO;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneImportLyric : EditorClockTestScene
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private DialogOverlay dialogOverlay;
        private ImportLyricScreen screen;
        private ImportLyricManager importManager;

        public TestSceneImportLyric()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay(),
                importManager = new ImportLyricManager()
            });

            Dependencies.Cache(dialogOverlay);
            Dependencies.Cache(importManager);
        }

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            var temp = TestResources.GetTestLrcForImport("default");
            Child = screen = new ImportLyricScreen(new FileInfo(temp));
        });
    }
}
