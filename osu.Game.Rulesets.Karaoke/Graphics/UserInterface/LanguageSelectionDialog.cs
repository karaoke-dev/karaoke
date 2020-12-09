// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class LanguageSelectionDialog : TitleFocusedOverlayContainer
    {
        protected override string Title => "Select language";

        private TranslateSearchTextBox filter;
        private DrawableLanguageList languageList;

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
                    new Dimension(GridSizeMode.Absolute, 50),
                    new Dimension(GridSizeMode.Distributed)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        filter = new TranslateSearchTextBox
                        {
                            RelativeSizeAxes = Axes.X,
                        }
                    },
                    new Drawable[]
                    {
                        languageList = new DrawableLanguageList
                        {
                            RelativeSizeAxes = Axes.Both,
                            Items = { BindTarget = languages }
                        }
                    }
                }
            };

            filter.Current.BindValueChanged(e => languageList.Filter(e.NewValue));
        }

        public class TranslateSearchTextBox : SearchTextBox
        {
            protected override Color4 SelectionColour => Color4.Gray;

            public TranslateSearchTextBox()
            {
                PlaceholderText = @"type in keywords...";
            }
        }

        public class DrawableLanguageList : OsuRearrangeableListContainer<CultureInfo>
        {
            public void Filter(string text)
            {
                var items = (SearchContainer<RearrangeableListItem<CultureInfo>>)ListContainer;
                items.SearchTerm = text;
            }

            protected override FillFlowContainer<RearrangeableListItem<CultureInfo>> CreateListFillFlowContainer() => new SearchContainer<RearrangeableListItem<CultureInfo>>
            {
                Spacing = new Vector2(0, 3),
                LayoutDuration = 200,
                LayoutEasing = Easing.OutQuint,
            };

            protected override OsuRearrangeableListItem<CultureInfo> CreateOsuDrawable(CultureInfo item)
                => new DrawableLanguageListItem(item);

            public class DrawableLanguageListItem : OsuRearrangeableListItem<CultureInfo>, IFilterable
            {
                public DrawableLanguageListItem(CultureInfo item)
                    : base(item)
                {
                    Padding = new MarginPadding { Left = 5 };
                    FilterTerms = new List<string>
                    {
                        item.Name,
                        item.DisplayName,
                        item.EnglishName,
                        item.NativeName
                    };
                }

                protected override Drawable CreateContent() => new OsuTextFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Text = Model.EnglishName,
                };
                public IEnumerable<string> FilterTerms { get; }

                private bool matchingFilter = true;

                public bool MatchingFilter
                {
                    get => matchingFilter;
                    set
                    {
                        if (matchingFilter == value)
                            return;

                        matchingFilter = value;
                        updateFilter();
                    }
                }

                private void updateFilter() => this.FadeTo(MatchingFilter ? 1 : 0, 200);

                public bool FilteringActive { get; set; }
            }
        }
    }
}
