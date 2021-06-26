// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class LanguageSelectionDialog : TitleFocusedOverlayContainer, IHasCurrentValue<CultureInfo>
    {
        protected override string Title => "Select language";

        private readonly LanguageSelectionSearchTextBox filter;
        private readonly DrawableLanguageList languageList;

        private readonly BindableWithCurrent<CultureInfo> current = new BindableWithCurrent<CultureInfo>();

        public Bindable<CultureInfo> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        public LanguageSelectionDialog()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.5f, 0.8f);

            var languages = new BindableList<CultureInfo>(CultureInfo.GetCultures(CultureTypes.NeutralCultures));
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 40),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        filter = new LanguageSelectionSearchTextBox
                        {
                            RelativeSizeAxes = Axes.X,
                        }
                    },
                    new Drawable[]
                    {
                        languageList = new DrawableLanguageList
                        {
                            RelativeSizeAxes = Axes.Both,
                            RequestSelection = item =>
                            {
                                Current.Value = item;
                                Hide();
                            },
                            Items = { BindTarget = languages }
                        }
                    }
                }
            };

            filter.Current.BindValueChanged(e => languageList.Filter(e.NewValue));
            Current.BindValueChanged(e => languageList.SelectedSet.Value = e.NewValue);
        }

        public class LanguageSelectionSearchTextBox : SearchTextBox
        {
            protected override Color4 SelectionColour => Color4.Gray;

            public LanguageSelectionSearchTextBox()
            {
                PlaceholderText = @"type in keywords...";
            }
        }

        public class DrawableLanguageList : RearrangeableTextListContainer<CultureInfo>
        {
            protected override OsuRearrangeableListItem<CultureInfo> CreateOsuDrawable(CultureInfo item)
                => new DrawableLanguageListItem(item)
                {
                    SelectedSet = { BindTarget = SelectedSet },
                    RequestSelection = set => RequestSelection?.Invoke(set)
                };

            public class DrawableLanguageListItem : DrawableTextListItem
            {
                public DrawableLanguageListItem(CultureInfo item)
                    : base(item)
                {
                    Padding = new MarginPadding { Left = 5 };
                }

                public override IEnumerable<string> FilterTerms => new[]
                {
                    Model.Name,
                    Model.DisplayName,
                    Model.EnglishName,
                    Model.NativeName
                };

                protected override string GetDisplayText(CultureInfo model) => model.DisplayName;
            }
        }
    }
}
