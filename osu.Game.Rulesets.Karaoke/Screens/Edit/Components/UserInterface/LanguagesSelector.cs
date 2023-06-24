// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.UserInterface;

public partial class LanguagesSelector : FillFlowContainer, IHasCurrentValue<CultureInfo[]>
{
    private readonly Bindable<CultureInfo[]> current = new();

    public Bindable<CultureInfo[]> Current
    {
        get => current;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            current.UnbindBindings();
            current.BindTo(value);
        }
    }

    public LanguagesSelector()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Spacing = new Vector2(10);

        current.BindValueChanged(e =>
        {
            RemoveAll(x => x is SelectedLanguage, true);

            for (int i = 0; i < e.NewValue.Length; i++)
            {
                var cultureInfo = e.NewValue[i];

                var bindable = new Bindable<CultureInfo>(cultureInfo);
                bindable.BindValueChanged(c =>
                {
                    removeCultureInfo(c.OldValue);
                    addCultureInfo(c.NewValue);
                });

                Add(new SelectedLanguage
                {
                    RelativeSizeAxes = Axes.X,
                    Text = $"#{i} - {CultureInfoUtils.GetLanguageDisplayText(cultureInfo)}",
                    OnDeleteButtonClick = () => removeCultureInfo(cultureInfo)
                });
            }
        });

        var fillFlowContainer = Content as FillFlowContainer;
        fillFlowContainer?.Insert(int.MaxValue, new CreateLanguageSubsection
        {
            RelativeSizeAxes = Axes.X,
            Height = 300,
            LanguageSelected = addCultureInfo
        });
    }

    private void removeCultureInfo(CultureInfo cultureInfo)
    {
        var languageList = current.Value?.ToList() ?? new List<CultureInfo>();
        if (!languageList.Contains(cultureInfo))
            return;

        languageList.Remove(cultureInfo);
        current.Value = languageList.ToArray();
    }

    private void addCultureInfo(CultureInfo cultureInfo)
    {
        var languageList = current.Value?.ToList() ?? new List<CultureInfo>();
        if (languageList.Contains(cultureInfo))
            return;

        languageList.Add(cultureInfo);
        current.Value = languageList.ToArray();
    }

    // todo: will use rearrangeable list view for able to change the order.
    private partial class SelectedLanguage : CompositeDrawable
    {
        private const float delete_button_size = 20f;
        private const int padding = 15;

        public Action OnDeleteButtonClick;

        private readonly OsuSpriteText languageSpriteText;
        private readonly Box background;

        public SelectedLanguage()
        {
            Height = 32;
            Masking = true;
            CornerRadius = 15;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                languageSpriteText = new TruncatingSpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.X,
                    Padding = new MarginPadding
                    {
                        Left = padding,
                        Right = padding * 2 + delete_button_size,
                    },
                },
                new DeleteIconButton
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Margin = new MarginPadding
                    {
                        Right = padding,
                    },
                    Size = new Vector2(delete_button_size),
                    Action = () => OnDeleteButtonClick?.Invoke(),
                }
            };
        }

        [BackgroundDependencyLoader(true)]
        private void load([CanBeNull] OverlayColourProvider colourProvider)
        {
            background.Colour = colourProvider?.Background5 ?? Color4Extensions.FromHex(@"1c2125");
        }

        public string Text
        {
            set => languageSpriteText.Text = value;
        }
    }

    private partial class CreateLanguageSubsection : CompositeDrawable
    {
        private const int padding = 15;

        private readonly Bindable<CultureInfo> current = new();

        public Action<CultureInfo> LanguageSelected;

        public CreateLanguageSubsection()
        {
            current.BindValueChanged(e =>
            {
                var newLanguage = e.NewValue;
                if (newLanguage == null)
                    return;

                LanguageSelected?.Invoke(newLanguage);
                current.Value = null;
            });
        }

        [BackgroundDependencyLoader(true)]
        private void load([CanBeNull] OverlayColourProvider colourProvider)
        {
            Masking = true;
            CornerRadius = 15;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider?.Background5 ?? Color4Extensions.FromHex(@"1c2125")
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(padding),
                    Child = new LanguageSelector
                    {
                        RelativeSizeAxes = Axes.Both,
                        Current = current
                    }
                }
            };
        }
    }
}
