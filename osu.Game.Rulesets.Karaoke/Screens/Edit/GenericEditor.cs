// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.Cursor;
using osu.Game.Graphics.UserInterface;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Menus;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Menus;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class GenericEditor<TScreenMode> : ScreenWithBeatmapBackground, IKeyBindingHandler<GlobalAction> where TScreenMode : Enum
{
    public override float BackgroundParallaxAmount => 0.1f;

    public override bool AllowUserExit => false;

    public override bool HideOverlaysOnEnter => true;

    public override bool DisallowExternalBeatmapRulesetChanges => true;

    public override bool? ApplyModTrackAdjustments => false;

    public readonly Bindable<TScreenMode> Mode = new();

    private Container<GenericEditorScreen<TScreenMode>> screenContainer = null!;

    private GenericEditorScreen<TScreenMode>? currentScreen;

    private EditorMenuBar menuBar = null!;

    private DependencyContainer dependencies = null!;

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    [BackgroundDependencyLoader(true)]
    private void load(OsuColour colours, EditorBeatmap editorBeatmap, BindableBeatDivisor beatDivisor)
    {
        // todo: should re-inject editor clock because it will let track cannot change time because it's in another screen.
        var clock = new EditorClock(editorBeatmap, beatDivisor);

        var loadableBeatmap = Beatmap.Value;
        clock.ChangeSource(loadableBeatmap.Track);

        dependencies.CacheAs(clock);
        AddInternal(clock);

        AddInternal(new OsuContextMenuContainer
        {
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                new Container
                {
                    Name = "Screen container",
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Top = 40, Bottom = 60 },
                    Child = screenContainer = new Container<GenericEditorScreen<TScreenMode>>
                    {
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                    },
                },
                new Container
                {
                    Name = "Top bar",
                    RelativeSizeAxes = Axes.X,
                    Height = 40,
                    Children = new Drawable[]
                    {
                        menuBar = new EditorMenuBar
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                        },
                        new GenericScreenSelectionTabControl<TScreenMode>
                        {
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.BottomRight,
                            X = -15,
                            Current = Mode,
                        },
                    },
                },
                new BottomBar(),
            },
        });

        Mode.BindValueChanged(onModeChanged, true);
    }

    private void onModeChanged(ValueChangedEvent<TScreenMode> e)
    {
        var lastScreen = currentScreen;

        lastScreen?.Hide();

        try
        {
            if ((currentScreen = screenContainer.SingleOrDefault(s => EqualityComparer<TScreenMode>.Default.Equals(s.Type, e.NewValue))) != null)
            {
                screenContainer.ChangeChildDepth(currentScreen, lastScreen?.Depth + 1 ?? 0);
                currentScreen.Show();
                return;
            }

            currentScreen = GenerateScreen(e.NewValue);

            LoadComponentAsync(currentScreen, newScreen =>
            {
                if (newScreen != currentScreen)
                    return;

                screenContainer.Add(newScreen);
                newScreen.Show();
            });
        }
        finally
        {
            updateMenuItems(e.NewValue);
        }
    }

    private void updateMenuItems(TScreenMode screenMode)
    {
        var menuItems = new List<MenuItem>
        {
            new("Menu")
            {
                Items = new[]
                {
                    new EditorMenuItem("Save"),
                    new EditorMenuItem("Back", MenuItemType.Standard, this.Exit),
                },
            },
        };

        menuItems.AddRange(GenerateMenuItems(screenMode));

        menuBar.Items = menuItems;
    }

    protected abstract GenericEditorScreen<TScreenMode> GenerateScreen(TScreenMode screenMode);

    protected abstract MenuItem[] GenerateMenuItems(TScreenMode screenMode);

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.Back:
                // as we don't want to display the back button, manual handling of exit action is required.
                // follow how editor.cs does.
                this.Exit();
                return true;

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }
}
