// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    [Cached(typeof(IImportStateResolver))]
    public class LyricImporter : ScreenWithBeatmapBackground, IImportStateResolver
    {
        private readonly LyricImporterWaveContainer waves;

        [Cached]
        protected LyricImporterSubScreenStack ScreenStack { get; private set; }

        private readonly BindableBeatDivisor beatDivisor = new();

        private EditorBeatmap editorBeatmap;
        private ImportLyricEditorChangeHandler changeHandler;

        private ImportLyricManager importManager;

        private LyricCheckerManager lyricCheckerManager;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public event Action<IBeatmap> OnImportFinished;

        public LyricImporter()
        {
            var backgroundColour = Color4Extensions.FromHex(@"3e3a44");

            InternalChild = waves = new LyricImporterWaveContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = backgroundColour,
                    },
                    new KaraokeEditInputManager(new KaraokeRuleset().RulesetInfo)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Top = Header.HEIGHT },
                        Child = ScreenStack = new LyricImporterSubScreenStack { RelativeSizeAxes = Axes.Both }
                    },
                    new Header(ScreenStack),
                }
            };

            ScreenStack.Push(LyricImporterStep.ImportLyric);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            waves.Show();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            // todo: remove caching of this and consume via editorBeatmap?
            // follow how editor.cs do.
            dependencies.Cache(beatDivisor);

            // inject local editor beatmap handler because should not affect global beatmap data.
            var playableBeatmap = new KaraokeBeatmap();
            AddInternal(editorBeatmap = new EditorBeatmap(playableBeatmap));
            dependencies.CacheAs(editorBeatmap);
            changeHandler = new ImportLyricEditorChangeHandler(editorBeatmap);
            dependencies.CacheAs<IEditorChangeHandler>(changeHandler);

            AddInternal(importManager = new ImportLyricManager());
            dependencies.Cache(importManager);

            AddInternal(lyricCheckerManager = new LyricCheckerManager());
            dependencies.Cache(lyricCheckerManager);
        }

        public void Cancel()
        {
            this.Exit();
        }

        public void Finish()
        {
            this.Exit();
            OnImportFinished?.Invoke(editorBeatmap);
        }

        private class LyricImporterWaveContainer : WaveContainer
        {
            protected override bool StartHidden => true;

            public LyricImporterWaveContainer()
            {
                FirstWaveColour = Color4Extensions.FromHex(@"654d8c");
                SecondWaveColour = Color4Extensions.FromHex(@"554075");
                ThirdWaveColour = Color4Extensions.FromHex(@"44325e");
                FourthWaveColour = Color4Extensions.FromHex(@"392850");
            }
        }

        /// <summary>
        /// Use this class as temp class until <see cref="EditorChangeHandler"/> support customized beatmap.
        /// </summary>
        private class ImportLyricEditorChangeHandler : TransactionalCommitComponent, IEditorChangeHandler
        {
            public event Action OnStateChange;

            public ImportLyricEditorChangeHandler(EditorBeatmap editorBeatmap)
            {
            }

            protected override void UpdateState()
            {
                // do nothing.
                OnStateChange?.Invoke();
            }
        }
    }
}
