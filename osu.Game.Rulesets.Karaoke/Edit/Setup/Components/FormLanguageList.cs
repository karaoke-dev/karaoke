// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Specialized;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components;

public partial class FormLanguageList : CompositeDrawable
{
    public BindableList<CultureInfo> Languages { get; } = new();

    public LocalisableString Caption { get; init; }

    public LocalisableString HintText { get; init; }

    private Box background = null!;
    private FormFieldCaption caption = null!;
    private FillFlowContainer flow = null!;

    [Resolved]
    private OverlayColourProvider colourProvider { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        Masking = true;
        CornerRadius = 5;

        AddLanguageButton button;

        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = colourProvider.Background5,
            },
            new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding(9),
                Spacing = new Vector2(7),
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    caption = new FormFieldCaption
                    {
                        Caption = Caption,
                        TooltipText = HintText,
                    },
                    flow = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Full,
                        Spacing = new Vector2(5),
                        Child = button = new AddLanguageButton
                        {
                            Action = languageInsertionRequested,
                        },
                    },
                },
            },
        };

        flow.SetLayoutPosition(button, float.MaxValue);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Languages.BindCollectionChanged((_, args) =>
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
                updateLanguages();
        }, true);
        updateState();
    }

    protected override bool OnHover(HoverEvent e)
    {
        updateState();
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);
        updateState();
    }

    private void updateState()
    {
        background.Colour = colourProvider.Background5;
        caption.Colour = colourProvider.Content2;

        BorderThickness = IsHovered ? 2 : 0;

        if (IsHovered)
            BorderColour = colourProvider.Light4;
    }

    private void updateLanguages()
    {
        flow.RemoveAll(d => d is LanguageDisplay, true);

        foreach (var language in Languages)
        {
            flow.Add(new LanguageDisplay
            {
                Current = { Value = language },
                DeleteRequested = languageDeletionRequested,
            });
        }
    }

    private void languageInsertionRequested(CultureInfo language)
    {
        if (!Languages.Contains(language))
            Languages.Add(language);
    }

    private void languageDeletionRequested(CultureInfo language) => Languages.Remove(language);

    private partial class LanguageDisplay : CompositeDrawable, IHasCurrentValue<CultureInfo>
    {
        /// <summary>
        /// Invoked when the user has requested the corresponding to this <see cref="CultureInfo"/>
        /// </summary>
        public Action<CultureInfo>? DeleteRequested;

        private readonly BindableWithCurrent<CultureInfo> current = new();

        public Bindable<CultureInfo> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        private readonly Box background;
        private readonly OsuSpriteText languageName;

        public LanguageDisplay()
        {
            AutoSizeAxes = Axes.X;
            Height = 30;
            Masking = true;
            CornerRadius = 5;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                languageName = new OsuSpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Padding = new MarginPadding { Left = 10, Right = 32 },
                },
                new IconButton
                {
                    Icon = FontAwesome.Solid.Times,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Size = new Vector2(16),
                    Margin = new MarginPadding { Right = 10 },
                    Action = () =>
                    {
                        DeleteRequested?.Invoke(Current.Value);
                    },
                },
            };

            current.BindValueChanged(x =>
            {
                languageName.Text = CultureInfoUtils.GetLanguageDisplayText(x.NewValue);
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.BlueDarker;
            languageName.Colour = colours.BlueLighter;
        }
    }

    internal partial class AddLanguageButton : CompositeDrawable, IHasPopover
    {
        public Action<CultureInfo>? Action;

        private readonly Bindable<CultureInfo?> currentLanguage = new();

        public AddLanguageButton()
        {
            Size = new Vector2(35);

            InternalChild = new IconButton
            {
                Action = this.ShowPopover,
                Icon = FontAwesome.Solid.Plus,
            };

            currentLanguage.BindValueChanged(e =>
            {
                this.HidePopover();

                var language = e.NewValue;
                if (language == null)
                    return;

                Action?.Invoke(language);

                currentLanguage.Value = null;
            });
        }

        public Popover GetPopover() => new LanguageSelectorPopover(currentLanguage)
        {
            EnableEmptyOption = false,
        };
    }
}
