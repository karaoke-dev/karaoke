// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public partial class ImportLyricHeader : TabControlOverlayHeader<ILyricImporterStepScreen>
{
    protected override OverlayTitle CreateTitle() => new ImportLyricHeaderTitle();

    protected override OsuTabControl<ILyricImporterStepScreen> CreateTabControl() => new ImportStepTabControl();

    protected override Drawable CreateContent() => new ImportLyricHeaderContent();

    private LyricImporterSubScreenStack screenStack = null!;

    [BackgroundDependencyLoader]
    private void load(LyricImporterSubScreenStack screenStack)
    {
        this.screenStack = screenStack;

        screenStack.ScreenPushed += onPushed;
        screenStack.ScreenExited += onExited;

        Current.ValueChanged += e =>
        {
            var newScreen = e.NewValue;
            if (newScreen == null)
                throw new InvalidOperationException();

            onClickTabItem(newScreen);
        };
    }

    private void onPushed(IScreen _, IScreen newScreen)
    {
        if (newScreen is not ILyricImporterStepScreen lyricSubScreen)
            throw new NotImportStepScreenException();

        TabControl.AddItem(lyricSubScreen);
        Current.Value = lyricSubScreen;
    }

    private void onExited(IScreen _, IScreen newScreen)
    {
        if (newScreen is not ILyricImporterStepScreen lyricSubScreen)
            throw new NotImportStepScreenException();

        TabControl.Items.ToList().SkipWhile(s => s != lyricSubScreen).Skip(1).ForEach(TabControl.RemoveItem);
        Current.Value = lyricSubScreen;
    }

    private void onClickTabItem(ILyricImporterStepScreen screen)
    {
        screenStack.Pop(screen);
    }

    private partial class ImportLyricHeaderTitle : OverlayTitle
    {
        public ImportLyricHeaderTitle()
        {
            Title = "Import lyric";
            Description = "Import the lyric from the file.";
            Icon = OsuIcon.News;
        }
    }

    public partial class ImportStepTabControl : BreadcrumbControl<ILyricImporterStepScreen>
    {
        public ImportStepTabControl()
        {
            RelativeSizeAxes = Axes.X;
            Height = 47;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            AccentColour = colourProvider.Light2;
        }

        protected override TabItem<ILyricImporterStepScreen> CreateTabItem(ILyricImporterStepScreen value) => new ControlTabItem(value)
        {
            AccentColour = AccentColour,
        };

        private partial class ControlTabItem : BreadcrumbTabItem
        {
            protected override float ChevronSize => 8;

            public ControlTabItem(ILyricImporterStepScreen value)
                : base(value)
            {
                RelativeSizeAxes = Axes.Y;
                Text.Font = Text.Font.With(size: 14);
                Text.Text = value.Title;
                Text.Anchor = Anchor.CentreLeft;
                Text.Origin = Anchor.CentreLeft;
                Chevron.Y = 1;
                Bar.Height = 0;
            }

            // base OsuTabItem makes font bold on activation, we don't want that here
            protected override void OnActivated() => FadeHovered();

            protected override void OnDeactivated() => FadeUnhovered();
        }
    }

    public partial class ImportLyricHeaderContent : CompositeDrawable
    {
        public ImportLyricHeaderContent()
        {
            Height = 32;
            RelativeSizeAxes = Axes.X;
        }

        [BackgroundDependencyLoader]
        private void load(LyricImporterSubScreenStack screenStack)
        {
            screenStack.ScreenPushed += onScreenChanged;
            screenStack.ScreenExited += onScreenChanged;
        }

        private void onScreenChanged(IScreen _, IScreen newScreen)
        {
            ClearInternal();

            if (newScreen is not IHasTopNavigation screenWithNavigation)
                return;

            Schedule(() =>
            {
                // Should wait until DI loaded inside.
                AddInternal(screenWithNavigation.CreateNavigation());
            });
        }
    }
}
