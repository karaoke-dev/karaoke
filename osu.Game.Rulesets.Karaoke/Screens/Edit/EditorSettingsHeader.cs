// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Overlays.Toolbar;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class EditorSettingsHeader<TEditStep> : EditorSettingsHeader, IHasCurrentValue<TEditStep> where TEditStep : struct, Enum
{
    private const int border_margin = 10;

    private const int tab_height = 40;
    private const int description_padding = 10;

    public Bindable<TEditStep> Current
    {
        get => tabControl.Current;
        set => tabControl.Current = value;
    }

    [Resolved]
    private EditorSettings editorSettings { get; set; } = null!;

    // for the DescriptionMarkdownTextFlowContainer.
    [Cached]
    private readonly OverlayColourProvider overlayColourProvider;

    private readonly EditStepTabControl tabControl;
    private readonly DescriptionTextFlowContainer lyricEditorDescription;

    protected EditorSettingsHeader()
    {
        overlayColourProvider = new OverlayColourProvider(CreateColourScheme());

        AddInternal(new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Padding = new MarginPadding(border_margin),
            Children = new Drawable[]
            {
                tabControl = CreateTabControl().With(x =>
                {
                    x.RelativeSizeAxes = Axes.X;
                    x.Height = tab_height;
                }),
                lyricEditorDescription = CreateDescriptionTextFlowContainer().With(x =>
                {
                    x.RelativeSizeAxes = Axes.X;
                    x.AutoSizeAxes = Axes.Y;
                    x.Padding = new MarginPadding(description_padding);
                }),
            },
        });
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        tabControl.Items = Enum.GetValues<TEditStep>();
        tabControl.Current.BindValueChanged(x =>
        {
            var step = x.NewValue;

            // update description text.
            lyricEditorDescription.Description = GetSelectionDescription(step);

            // wait until description text ready.
            Schedule(() =>
            {
                editorSettings.ReloadSections();
                UpdateEditStep(step);
            });
        }, true);
    }

    protected virtual DescriptionTextFlowContainer CreateDescriptionTextFlowContainer() => new();

    protected abstract OverlayColourScheme CreateColourScheme();

    protected abstract EditStepTabControl CreateTabControl();

    protected abstract DescriptionFormat GetSelectionDescription(TEditStep step);

    protected virtual void UpdateEditStep(TEditStep step)
    {
    }

    protected abstract partial class EditStepTabControl : TabControl<TEditStep>
    {
        public const int SPACING = 10;

        protected override TabFillFlowContainer CreateTabFlow() => new()
        {
            RelativeSizeAxes = Axes.Y,
            AutoSizeAxes = Axes.X,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(SPACING, 0),
        };

        protected override Dropdown<TEditStep>? CreateDropdown() => null;

        protected sealed override TabItem<TEditStep> CreateTabItem(TEditStep value) => CreateStepButton(new OsuColour(), value);

        protected abstract StepTabButton CreateStepButton(OsuColour colour, TEditStep step);
    }

    protected abstract partial class IssueStepTabButton : StepTabButton
    {
        protected readonly IBindableList<Issue> Issues = new BindableList<Issue>();

        protected IssueStepTabButton(TEditStep value)
            : base(value)
        {
            CountCircle countCircle;

            AddInternal(countCircle = new CountCircle
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.Centre,
                X = -5,
            });

            Issues.BindCollectionChanged((_, _) =>
            {
                int count = Issues.Count;
                countCircle.Alpha = count == 0 ? 0 : 1;
                countCircle.Count = count;
            });
        }
    }

    protected partial class StepTabButton : TabItem<TEditStep>
    {
        private readonly Box background;
        private readonly OsuSpriteText text;

        public StepTabButton(TEditStep value)
            : base(value)
        {
            RelativeSizeAxes = Axes.Y;

            Child = new Container
            {
                Masking = true,
                CornerRadius = 15,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    text = new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = OsuFont.GetFont(size: 18, weight: FontWeight.Bold),
                    },
                },
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            updateState();
            updateTabSize();
        }

        private void updateTabSize()
        {
            if (Parent?.Parent is not EditStepTabControl control)
                throw new InvalidOperationException();

            int tabAmount = Enum.GetValues<TEditStep>().Length;
            Width = (control.DrawWidth - (tabAmount - 1) * EditStepTabControl.SPACING) / tabAmount;
        }

        public LocalisableString Text
        {
            get => text.Text;
            set => text.Text = value;
        }

        public Color4 SelectedColour { get; init; }

        public Color4 UnSelectedColour { get; init; }

        protected sealed override void OnActivated() => updateState();

        protected sealed override void OnDeactivated() => updateState();

        private void updateState()
        {
            background.Colour = Active.Value ? SelectedColour : UnSelectedColour;
            Children.ForEach(x => x.Alpha = Active.Value ? 1.0f : 0.6f);
        }
    }

    /// <summary>
    /// Copied from <see cref="ToolbarNotificationButton"/>
    /// </summary>
    private partial class CountCircle : CompositeDrawable
    {
        private readonly OsuSpriteText countText;
        private readonly Circle circle;

        private int count;

        public int Count
        {
            get => count;
            set
            {
                if (count == value)
                    return;

                if (value != count)
                {
                    circle.FlashColour(Color4.White, 600, Easing.OutQuint);
                    this.ScaleTo(1.1f).Then().ScaleTo(1, 600, Easing.OutElastic);
                }

                count = value;
                countText.Text = value.ToString("#,0");
            }
        }

        public CountCircle()
        {
            AutoSizeAxes = Axes.X;
            Height = 20;

            InternalChildren = new Drawable[]
            {
                circle = new Circle
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Red,
                },
                countText = new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = -1,
                    Font = OsuFont.GetFont(size: 18, weight: FontWeight.Bold),
                    Padding = new MarginPadding(5),
                    Colour = Color4.White,
                    UseFullGlyphHeight = true,
                },
            };
        }
    }
}

public abstract partial class EditorSettingsHeader : CompositeDrawable
{
    private readonly Box box;

    protected EditorSettingsHeader()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChildren = new Drawable[]
        {
            box = new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
        };
    }

    public Colour4 BackgroundColour
    {
        get => box.Colour;
        set => box.Colour = value;
    }
}
