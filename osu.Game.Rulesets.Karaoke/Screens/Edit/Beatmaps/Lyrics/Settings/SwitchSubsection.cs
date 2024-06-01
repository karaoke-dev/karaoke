// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

/// <summary>
/// A subsection that can switch between multiple modes.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public abstract partial class SwitchSubsection<TEnum> : CompositeDrawable, IHasCurrentValue<TEnum>
    where TEnum : struct, Enum
{
    private const int corner_radius = 15;

    private const int tab_padding = 20;
    private const int tab_height = 40;

    private const int spacing = 10;

    private const int description_horizontal_padding = 20;
    private const int description_vertical_padding = 10;

    public Bindable<TEnum> Current
    {
        get => tabControl.Current;
        set => tabControl.Current = value;
    }

    private readonly Box background;
    private readonly SwitchTabControl tabControl;
    private readonly LyricEditorDescriptionTextFlowContainer lyricEditorDescription;

    protected SwitchSubsection()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChild = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(spacing),
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
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Padding = new MarginPadding(tab_padding),
                            Child = tabControl = CreateTabControl().With(x =>
                            {
                                x.RelativeSizeAxes = Axes.X;
                                x.Height = tab_height;
                            }),
                        },
                    },
                },
                lyricEditorDescription = new LyricEditorDescriptionTextFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding
                    {
                        Vertical = description_vertical_padding,
                        Horizontal = description_horizontal_padding,
                    },
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
    {
        background.Colour = colourProvider.Background4(state.Mode);

        tabControl.Items = Enum.GetValues<TEnum>();
        tabControl.Current.BindValueChanged(x =>
        {
            // update description text.
            lyricEditorDescription.Description = GetDescription(x.NewValue);
        }, true);
    }

    protected abstract SwitchTabControl CreateTabControl();

    protected abstract DescriptionFormat GetDescription(TEnum @enum);

    protected abstract partial class SwitchTabControl : TabControl<TEnum>
    {
        public const int SPACING = 20;

        protected override TabFillFlowContainer CreateTabFlow() => new()
        {
            RelativeSizeAxes = Axes.Y,
            AutoSizeAxes = Axes.X,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(SPACING, 0),
        };

        protected override Dropdown<TEnum>? CreateDropdown() => null;

        protected sealed override TabItem<TEnum> CreateTabItem(TEnum value) => CreateStepButton(new OsuColour(), value);

        protected abstract SwitchTabItem CreateStepButton(OsuColour colour, TEnum step);
    }

    protected abstract partial class SwitchTabItem : TabItem<TEnum>
    {
        protected SwitchTabItem(TEnum value)
            : base(value)
        {
            RelativeSizeAxes = Axes.Y;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            UpdateState();
            updateTabSize();
        }

        private void updateTabSize()
        {
            if (Parent?.Parent is not SwitchTabControl control)
                throw new InvalidOperationException();

            int tabAmount = Enum.GetValues<TEnum>().Length;
            Width = (control.DrawWidth - (tabAmount - 1) * SwitchTabControl.SPACING) / tabAmount;
        }

        protected sealed override void OnActivated() => UpdateState();

        protected sealed override void OnDeactivated() => UpdateState();

        protected abstract void UpdateState();
    }
}
