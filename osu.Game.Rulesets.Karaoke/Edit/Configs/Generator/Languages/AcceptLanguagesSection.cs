// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Languages
{
    public class AcceptLanguagesSection : GeneratorConfigSection<LanguageDetectorConfig>
    {
        protected override string Title => "Accept languages";

        private readonly Bindable<CultureInfo[]> bindableCultureInfo = new();

        public AcceptLanguagesSection(Bindable<LanguageDetectorConfig> current)
            : base(current)
        {
            bindableCultureInfo.BindValueChanged(e =>
            {
                RemoveAll(x => x is SelectedLanguage);

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

            RegisterConfig(bindableCultureInfo, nameof(LanguageDetectorConfig.AcceptLanguages));
        }

        private void removeCultureInfo(CultureInfo cultureInfo)
        {
            var languageList = bindableCultureInfo.Value?.ToList() ?? new List<CultureInfo>();
            if (!languageList.Contains(cultureInfo))
                return;

            languageList.Remove(cultureInfo);
            bindableCultureInfo.Value = languageList.ToArray();
        }

        private void addCultureInfo(CultureInfo cultureInfo)
        {
            var languageList = bindableCultureInfo.Value?.ToList() ?? new List<CultureInfo>();
            if (languageList.Contains(cultureInfo))
                return;

            languageList.Add(cultureInfo);
            bindableCultureInfo.Value = languageList.ToArray();
        }

        // todo: will use rearrangeable list view for able to change the order.
        private class SelectedLanguage : CompositeDrawable
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
                    languageSpriteText = new OsuSpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Padding = new MarginPadding
                        {
                            Left = padding,
                            Right = padding * 2 + delete_button_size,
                        },
                        Truncate = true,
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

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                background.Colour = colourProvider.Background3;
            }

            public string Text
            {
                set => languageSpriteText.Text = value;
            }
        }

        private class CreateLanguageSubsection : CompositeDrawable
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

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                Masking = true;
                CornerRadius = 15;
                InternalChildren = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = colourProvider.Background3
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
}
