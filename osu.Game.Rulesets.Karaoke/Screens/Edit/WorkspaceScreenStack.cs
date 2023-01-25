// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

/// <summary>
/// A component which provides functionality for displaying and handling transitions between multiple <see cref="WorkspaceScreen{TItem}"/>s.
/// </summary>
public abstract partial class WorkspaceScreenStack<TItem> : CompositeDrawable
{
    private readonly Bindable<TItem> item = new();
    private readonly Container<WorkspaceScreen<TItem>> screenContainer;

    private WorkspaceScreen<TItem>? currentScreen;

    protected WorkspaceScreenStack()
    {
        InternalChildren = new Drawable[]
        {
            new Container
            {
                Name = "Screen container",
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Top = 40, Bottom = 60 },
                Child = screenContainer = new Container<WorkspaceScreen<TItem>>
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true
                }
            },
            new Container
            {
                Name = "Top bar",
                RelativeSizeAxes = Axes.X,
                Height = 36,
                Children = new Drawable[]
                {
                    CreateTabControl().With(x =>
                    {
                        x.RelativeSizeAxes = Axes.X;
                        x.Height = 36;
                    })
                },
            },
        };

        item.BindValueChanged(onItemChanged, true);
    }

    private void onItemChanged(ValueChangedEvent<TItem> e)
    {
        var lastScreen = currentScreen;

        lastScreen?.Hide();

        try
        {
            if ((currentScreen = screenContainer.SingleOrDefault(s => EqualityComparer<TItem>.Default.Equals(s.Item, e.NewValue))) != null)
            {
                screenContainer.ChangeChildDepth(currentScreen, lastScreen?.Depth + 1 ?? 0);

                currentScreen.Show();
                return;
            }

            currentScreen = CreateWorkspaceScreen(e.NewValue);

            LoadComponentAsync(currentScreen, newScreen =>
            {
                if (newScreen != currentScreen || newScreen == null)
                    return;

                screenContainer.Add(newScreen);
                newScreen.Show();
            });
        }
        catch
        {
        }
    }

    protected abstract WorkspaceScreen<TItem>? CreateWorkspaceScreen(TItem item);

    protected virtual WorkspaceScreenStackTabControl CreateTabControl() => new();

    public partial class WorkspaceScreenStackTabControl : OverlayTabControl<TItem>
    {
        public WorkspaceScreenStackTabControl()
        {
            TabContainer.Margin = new MarginPadding { Horizontal = 10 };
        }

        protected override TabItem<TItem> CreateTabItem(TItem value) => new WorkspaceScreenStackTabItem(value)
        {
            AccentColour = AccentColour
        };

        private partial class WorkspaceScreenStackTabItem : OverlayTabItem
        {
            public WorkspaceScreenStackTabItem(TItem value)
                : base(value)
            {
                // todo: copied from OsuTabItem.
                switch (value)
                {
                    case IHasDescription hasDescription:
                        Text.Text = hasDescription.GetDescription();
                        break;

                    case Enum e:
                        Text.Text = e.GetLocalisableDescription();
                        break;

                    case LocalisableString l:
                        Text.Text = l;
                        break;

                    default:
                        Text.Text = value?.ToString() ?? string.Empty;
                        break;
                }
            }
        }
    }
}
