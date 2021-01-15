// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

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
        private LanguageSelectionDialog languageSelectionDialog;
        private TestImportLyricScreen screen;
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
                languageSelectionDialog = new LanguageSelectionDialog(),
                importManager = new ImportLyricManager()
            });

            Dependencies.Cache(dialogOverlay);
            Dependencies.Cache(languageSelectionDialog);
            Dependencies.Cache(importManager);
        }

        [Test]
        public void TestGoToStep() => Schedule(() =>
        {
            var temp = TestResources.GetTestLrcForImport("light");
            Child = screen = new TestImportLyricScreen(new FileInfo(temp));

            var steps = (ImportLyricStep[])Enum.GetValues(typeof(ImportLyricStep));

            foreach (var step in steps)
            {
                AddStep($"go to step {Enum.GetName(typeof(ImportLyricStep), step)}", () => { screen.GoToStep(step); });
            }
        });

        private class TestImportLyricScreen : ImportLyricScreen
        {
            public TestImportLyricScreen(FileInfo fileInfo)
            {
                if (!(ScreenStack.CurrentScreen is DragFileSubScreen dragFileSubScreen))
                    throw new ScreenStack.ScreenNotInStackException($"{nameof(DragFileSubScreen)} does not in the screen.");

                dragFileSubScreen.ImportLyricFile(fileInfo);
            }

            public void GoToStep(ImportLyricStep step)
            {
                if (!(ScreenStack.CurrentScreen is IImportLyricSubScreen lyricSubScreen))
                    return;

                if (step == lyricSubScreen.Step)
                    return;

                if (step <= lyricSubScreen.Step)
                    return;

                var totalSteps = ((ImportLyricStep[])Enum.GetValues(typeof(ImportLyricStep))).Where(x => x > lyricSubScreen.Step && x <= step);

                foreach (var gotoStep in totalSteps)
                {
                    ScreenStack.Push(gotoStep);
                }
            }
        }
    }
}
