// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    /// <summary>
    /// Implement most feature for searchable text container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RearrangeableTextFlowListContainer<T> : OsuRearrangeableListContainer<T>
    {
        public readonly Bindable<T> SelectedSet = new Bindable<T>();

        public Action<T> RequestSelection;

        private SearchContainer<RearrangeableListItem<T>> searchContainer;

        protected override FillFlowContainer<RearrangeableListItem<T>> CreateListFillFlowContainer() => searchContainer = new SearchContainer<RearrangeableListItem<T>>
        {
            Spacing = new Vector2(0, 3),
            LayoutDuration = 200,
            LayoutEasing = Easing.OutQuint,
        };

        public void Filter(string text)
        {
            searchContainer.SearchTerm = text;
        }

        protected override OsuRearrangeableListItem<T> CreateOsuDrawable(T item)
            => new DrawableTextListItem(item)
            {
                SelectedSet = { BindTarget = SelectedSet },
                RequestSelection = set => RequestSelection?.Invoke(set)
            };

        public class DrawableTextListItem : OsuRearrangeableListItem<T>, IFilterable
        {
            public readonly Bindable<T> SelectedSet = new Bindable<T>();

            public Action<T> RequestSelection;

            private TextFlowContainer text;

            private Color4 selectedColour;

            public DrawableTextListItem(T item)
                : base(item)
            {
                Padding = new MarginPadding { Left = 5 };
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

                    var equal = Equals(set.NewValue, Model);
                    text.FadeColour(equal ? selectedColour : Color4.White, FADE_DURATION);
                }, true);
            }

            protected sealed override Drawable CreateContent() => text = new OsuTextFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            }.With(x => CreateDisplayContent(x, Model));

            protected override bool OnClick(ClickEvent e)
            {
                RequestSelection?.Invoke(Model);
                return true;
            }

            public virtual IEnumerable<string> FilterTerms => new[]
            {
                Model.ToString()
            };

            protected virtual void CreateDisplayContent(OsuTextFlowContainer textFlowContainer, T model)
                => textFlowContainer.AddText(model.ToString());

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
