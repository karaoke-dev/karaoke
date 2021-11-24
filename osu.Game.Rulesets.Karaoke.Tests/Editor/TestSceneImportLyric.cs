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
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneImportLyric : EditorSubScreenTestScene<TestSceneImportLyric.TestImportLyricScreen>
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        [Cached]
        private readonly KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override TestImportLyricScreen CreateEditor()
        {
            var temp = TestResources.GetTestLrcForImport("light");
            return new TestImportLyricScreen(new FileInfo(temp));
        }

        private DialogOverlay dialogOverlay;
        private LanguageSelectionDialog languageSelectionDialog;
        private ImportLyricManager importManager;
        private LyricCheckerManager lyricCheckerManager;

        public TestSceneImportLyric()
        {
            var beatmap = new TestKaraokeBeatmap(null);
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
                languageSelectionDialog = new LanguageSelectionDialog(),
                importManager = new ImportLyricManager(),
                lyricCheckerManager = new LyricCheckerManager()
            });

            Dependencies.Cache(dialogOverlay);
            Dependencies.Cache(languageSelectionDialog);
            Dependencies.Cache(importManager);
            Dependencies.Cache(lyricCheckerManager);

            Dependencies.Cache(new EditorClock());
        }

        [Test]
        public void TestGoToStep()
        {
            var steps = EnumUtils.GetValues<ImportLyricStep>();

            foreach (var step in steps)
            {
                AddStep($"go to step {Enum.GetName(typeof(ImportLyricStep), step)}", () => { Editor.GoToStep(step); });
            }
        }

        public class TestImportLyricScreen : ImportLyricScreen
        {
            public TestImportLyricScreen(FileInfo fileInfo)
            {
                if (ScreenStack.CurrentScreen is not DragFileSubScreen dragFileSubScreen)
                    throw new ScreenStack.ScreenNotInStackException($"{nameof(DragFileSubScreen)} does not in the screen.");

                dragFileSubScreen.ImportLyricFile(fileInfo);
            }

            public void GoToStep(ImportLyricStep step)
            {
                if (ScreenStack.CurrentScreen is not IImportLyricSubScreen lyricSubScreen)
                    return;

                if (step == lyricSubScreen.Step)
                    return;

                if (step <= lyricSubScreen.Step)
                    return;

                var totalSteps = EnumUtils.GetValues<ImportLyricStep>().Where(x => x > lyricSubScreen.Step && x <= step);

                foreach (var gotoStep in totalSteps)
                {
                    ScreenStack.Push(gotoStep);
                }
            }
        }
    }
}
