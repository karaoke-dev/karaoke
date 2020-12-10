// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class LanguageSelectionDialog : TitleFocusedOverlayContainer
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

        public class DrawableLanguageList : OsuRearrangeableListContainer<CultureInfo>
        {
            public readonly Bindable<CultureInfo> SelectedSet = new Bindable<CultureInfo>();

            public Action<CultureInfo> RequestSelection;

            private SearchContainer<RearrangeableListItem<CultureInfo>> searchContainer;

            protected override FillFlowContainer<RearrangeableListItem<CultureInfo>> CreateListFillFlowContainer() => searchContainer = new SearchContainer<RearrangeableListItem<CultureInfo>>
            {
                Spacing = new Vector2(0, 3),
                LayoutDuration = 200,
                LayoutEasing = Easing.OutQuint,
            };

            public void Filter(string text)
            {
                searchContainer.SearchTerm = text;
            }

            protected override OsuRearrangeableListItem<CultureInfo> CreateOsuDrawable(CultureInfo item)
                => new DrawableLanguageListItem(item)
                {
                    SelectedSet = { BindTarget = SelectedSet },
                    RequestSelection = set => RequestSelection?.Invoke(set)
                };

            public class DrawableLanguageListItem : OsuRearrangeableListItem<CultureInfo>, IFilterable
            {
                public readonly Bindable<CultureInfo> SelectedSet = new Bindable<CultureInfo>();

                public Action<CultureInfo> RequestSelection;

                private TextFlowContainer text;

                private Color4 selectedColour;

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

                [BackgroundDependencyLoader]
                private void load(OsuColour colours)
                {
                    selectedColour = colours.Yellow;
                    HandleColour = colours.Gray5;
                }

                protected override void LoadComplete()
                {
                    base.LoadComplete();

                    SelectedSet.BindValueChanged(set =>
                    {
                        if (set.OldValue?.Equals(Model) != true && set.NewValue?.Equals(Model) != true)
                            return;

                        text.FadeColour(set.NewValue.Equals(Model) ? selectedColour : Color4.White, FADE_DURATION);
                    }, true);
                }

                protected override Drawable CreateContent() => text = new OsuTextFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Text = Model.EnglishName,
                };

                protected override bool OnClick(ClickEvent e)
                {
                    RequestSelection?.Invoke(Model);
                    return true;
                }

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
