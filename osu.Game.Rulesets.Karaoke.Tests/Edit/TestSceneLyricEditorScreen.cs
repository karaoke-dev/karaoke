// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;
using System.IO;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    [Ignore("This test case run failed in appveyor : (")]
    public class TestSceneLyricEditorScreen : EditorClockTestScene
    {
        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private DialogOverlay dialogOverlay;

        private LyricEditorScreen screen;

        private ImportLyricManager importManager;

        public TestSceneLyricEditorScreen()
        {
            // a trick to get osu! to register karaoke beatmaps
            KaraokeLegacyBeatmapDecoder.Register();
        }

        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap =
            new EditorBeatmap(new TestKaraokeBeatmap(null));

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            // Copied from TestSceneHitObjectComposer
            var clock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };
            Dependencies.CacheAs<IAdjustableClock>(clock);
            Dependencies.CacheAs<IFrameBasedClock>(clock);
            Dependencies.CacheAs(editorBeatmap);
            Dependencies.CacheAs<IBeatSnapProvider>(editorBeatmap);

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
            Child = screen = new LyricEditorScreen();
        });

        [Test]
        public void TestImportLyricFile()
        {
            AddAssert("Import lrc file.", () =>
            {
                var temp = TestResources.GetTestLrcForImport("default");
                return screen.ImportLyricFile(new FileInfo(temp));
            });
        }
    }
}
