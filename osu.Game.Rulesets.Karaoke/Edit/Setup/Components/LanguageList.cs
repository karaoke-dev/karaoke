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
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components;

/// <summary>
/// A component which displays a collection of <see cref="CultureInfo"/>
/// </summary>
public partial class LanguageList : CompositeDrawable
{
    public BindableList<CultureInfo> Languages { get; } = new();

    private FillFlowContainer languages = null!;

    private const int fade_duration = 200;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        AutoSizeDuration = fade_duration;
        AutoSizeEasing = Easing.OutQuint;

        InternalChild = languages = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Spacing = new Vector2(8),
            Direction = FillDirection.Full,
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Languages.BindCollectionChanged((_, args) =>
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
                updateSingers();
        }, true);
        FinishTransforms(true);
    }

    private void updateSingers()
    {
        languages.Clear();

        foreach (CultureInfo language in Languages)
        {
            languages.Add(new LanguageDisplay
            {
                Current = { Value = language },
                DeleteRequested = languageDeletionRequested,
            });
        }

        languages.Add(new AddLanguageButton
        {
            Action = languageInsertionRequested,
        });
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
