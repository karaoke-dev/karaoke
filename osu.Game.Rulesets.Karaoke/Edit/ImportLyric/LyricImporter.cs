// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Containers;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    [Cached(typeof(IImportStateResolver))]
    public class LyricImporter : ScreenWithBeatmapBackground, IImportStateResolver, IKeyBindingHandler<GlobalAction>
    {
        private readonly LyricImporterWaveContainer waves;
        private readonly Box background;

        [Cached]
        protected LyricImporterSubScreenStack ScreenStack { get; private set; }

        private readonly BindableBeatDivisor beatDivisor = new();

        private EditorBeatmap editorBeatmap;

        private ImportLyricManager importManager;

        private LyricsProvider lyricsProvider;

        private LyricCheckerManager lyricCheckerManager;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        // Hide the back button because we cannot show it only in the first step.
        public override bool AllowBackButton => false;

        public event Action<IBeatmap> OnImportFinished;

        public LyricImporter()
        {
            InternalChild = waves = new LyricImporterWaveContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = new PopoverContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        new KaraokeEditInputManager(new KaraokeRuleset().RulesetInfo)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding { Top = Header.HEIGHT },
                            Child = ScreenStack = new LyricImporterSubScreenStack { RelativeSizeAxes = Axes.Both }
                        },
                        new Header(ScreenStack),
                    }
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
        private void load(OverlayColourProvider colourProvider)
        {
            background.Colour = colourProvider.Background3;

            // todo: remove caching of this and consume via editorBeatmap?
            // follow how editor.cs do.
            dependencies.Cache(beatDivisor);

            // inject local editor beatmap handler because should not affect global beatmap data.
            var playableBeatmap = new KaraokeBeatmap
            {
                BeatmapInfo =
                {
                    Ruleset = new KaraokeRuleset().RulesetInfo,
                },
            };
            AddInternal(editorBeatmap = new EditorBeatmap(playableBeatmap));
            dependencies.CacheAs(editorBeatmap);

            AddInternal(importManager = new ImportLyricManager());
            dependencies.Cache(importManager);

            AddInternal(lyricsProvider = new LyricsProvider());
            dependencies.CacheAs<ILyricsProvider>(lyricsProvider);

            AddInternal(lyricCheckerManager = new LyricCheckerManager());
            dependencies.Cache(lyricCheckerManager);

            dependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
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

        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            if (e.Repeat)
                return false;

            switch (e.Action)
            {
                case GlobalAction.Back:
                    // as we don't want to display the back button, manual handling of exit action is required.
                    // follow how editor.cs does.
                    if (ScreenStack.CurrentScreen is not ILyricImporterStepScreen screen)
                        throw new InvalidOperationException("Screen stack should only contains step screen");

                    if (screen.Step != LyricImporterStep.ImportLyric)
                    {
                        // the better UX behavior should be move to the previous step.
                        // But it will not asking.
                        return false;

                        // todo: implement.
                        // ScreenStack.Exit();
                    }

                    this.Exit();
                    return true;

                default:
                    return false;
            }
        }

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
        {
        }

        private class LyricImporterWaveContainer : WaveContainer
        {
            protected override bool StartHidden => true;

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                FirstWaveColour = colourProvider.Light4;
                SecondWaveColour = colourProvider.Light3;
                ThirdWaveColour = colourProvider.Dark4;
                FourthWaveColour = colourProvider.Dark3;
            }
        }
    }
}
