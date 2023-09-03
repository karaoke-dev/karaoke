// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class EditModeSwitchSubsection<TEditMode> : FillFlowContainer, IHasCurrentValue<TEditMode>
    where TEditMode : struct, Enum
{
    private const int button_vertical_margin = 20;
    private const int horizontal_padding = 20;
    private const int corner_radius = 15;

    private readonly BindableWithCurrent<TEditMode> current = new();

    public Bindable<TEditMode> Current
    {
        get => current.Current;
        set => current.Current = value;
    }

    private readonly Box background;

    protected EditModeSwitchSubsection()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Spacing = new Vector2(10);

        LyricEditorDescriptionTextFlowContainer lyricEditorDescription;

        Children = new Drawable[]
        {
            new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = corner_radius,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new EditModeSwitcher(this)
                    {
                        Current = current,
                    },
                },
            },
            lyricEditorDescription = new LyricEditorDescriptionTextFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding { Horizontal = horizontal_padding },
            },
        };

        current.BindValueChanged(e =>
        {
            // update description text.
            lyricEditorDescription.Description = GetDescription(e.NewValue);
        }, true);
    }

    [BackgroundDependencyLoader]
    private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
    {
        background.Colour = colourProvider.Background4(state.Mode);
    }

    protected abstract LocalisableString GetButtonTitle(TEditMode mode);

    protected abstract Color4 GetButtonColour(OsuColour colours, TEditMode mode, bool active);

    protected abstract DescriptionFormat GetDescription(TEditMode mode);

    private partial class EditModeSwitcher : CompositeDrawable, IHasCurrentValue<TEditMode>
    {
        private readonly BindableWithCurrent<TEditMode> current = new();

        public Bindable<TEditMode> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        private readonly EditModeSwitchSubsection<TEditMode> parent;
        private readonly EditModeButton[] buttons;

        public EditModeSwitcher(EditModeSwitchSubsection<TEditMode> parent)
        {
            this.parent = parent;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                },
                Content = new[]
                {
                    buttons = Enum.GetValues<TEditMode>().Select(x => new EditModeButton(x)
                    {
                        Text = parent.GetButtonTitle(x),
                        Margin = new MarginPadding { Vertical = button_vertical_margin },
                        Padding = new MarginPadding { Horizontal = horizontal_padding },
                        Action = () => current.Value = x,
                    }).ToArray(),
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            current.BindValueChanged(e =>
            {
                // update button style.
                foreach (var button in buttons)
                {
                    bool highLight = EqualityComparer<TEditMode>.Default.Equals(button.Mode, e.NewValue);
                    button.Alpha = highLight ? 0.8f : 0.4f;
                    button.BackgroundColour = parent.GetButtonColour(colours, button.Mode, highLight);
                }
            }, true);
        }

        private partial class EditModeButton : EditorSectionButton
        {
            public TEditMode Mode { get; }

            public EditModeButton(TEditMode mode)
            {
                Mode = mode;
            }
        }
    }
}
