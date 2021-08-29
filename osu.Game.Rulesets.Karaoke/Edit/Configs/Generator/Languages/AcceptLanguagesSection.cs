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
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
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
                RemoveAll(x => x is RemovableLabelledLanguageSelector);

                for (int i = 0; i < e.NewValue.Length; i++)
                {
                    var cultureInfo = e.NewValue[i];

                    var bindable = new Bindable<CultureInfo>(cultureInfo);
                    bindable.BindValueChanged(c =>
                    {
                        removeCultureInfo(c.OldValue);
                        addCultureInfo(c.NewValue);
                    });

                    Add(new RemovableLabelledLanguageSelector
                    {
                        Label = $"#{i}",
                        Description = "Language detector will only use those selected language.",
                        Current = bindable,
                        OnDeleteButtonClick = () => removeCultureInfo(cultureInfo)
                    });
                }
            });

            var fillFlowContainer = Content as FillFlowContainer;
            fillFlowContainer?.Insert(int.MaxValue, new CreateLanguageButton
            {
                Text = "Add new language",
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

        private class RemovableLabelledLanguageSelector : LabelledLanguageSelector
        {
            private const float delete_button_size = 20f;

            public Action OnDeleteButtonClick;

            public RemovableLabelledLanguageSelector()
            {
                if (!(InternalChildren[1] is FillFlowContainer fillFlowContainer))
                    return;

                // change padding to place delete button.
                fillFlowContainer.Padding = new MarginPadding
                {
                    Horizontal = CONTENT_PADDING_HORIZONTAL,
                    Vertical = CONTENT_PADDING_VERTICAL,
                    Right = CONTENT_PADDING_HORIZONTAL + delete_button_size + CONTENT_PADDING_HORIZONTAL,
                };

                // add delete button.
                AddInternal(new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding
                    {
                        Top = CONTENT_PADDING_VERTICAL + 10,
                        Right = CONTENT_PADDING_HORIZONTAL,
                    },
                    Child = new DeleteIconButton
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Size = new Vector2(delete_button_size),
                        Action = () => OnDeleteButtonClick?.Invoke(),
                    }
                });
            }
        }

        public class CreateLanguageButton : OsuButton
        {
            [Resolved]
            protected LanguageSelectionDialog LanguageSelectionDialog { get; private set; }

            private readonly Bindable<CultureInfo> current = new();

            public Action<CultureInfo> LanguageSelected;

            public CreateLanguageButton()
            {
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;

                Action = () =>
                {
                    LanguageSelectionDialog.Current = current;
                    LanguageSelectionDialog.Show();
                };

                current.BindValueChanged(e => LanguageSelected?.Invoke(e.NewValue));
            }
        }
    }
}
