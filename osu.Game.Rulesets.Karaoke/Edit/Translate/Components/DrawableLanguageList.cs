// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    public class DrawableLanguageList : OsuRearrangeableListContainer<BeatmapSetOnlineLanguage>
    {
        private Scroll scroll;

        protected override ScrollContainer<Drawable> CreateScrollContainer() => scroll = new Scroll();

        protected override FillFlowContainer<RearrangeableListItem<BeatmapSetOnlineLanguage>> CreateListFillFlowContainer() => new Flow
        {
            DragActive = { BindTarget = DragActive }
        };

        protected override OsuRearrangeableListItem<BeatmapSetOnlineLanguage> CreateOsuDrawable(BeatmapSetOnlineLanguage item)
        {
            if (item == scroll.PlaceholderItem.Model)
                return scroll.ReplacePlaceholder();

            return new DrawableLanguageListItem(item, true);
        }

        /// <summary>
        /// The scroll container for this <see cref="DrawableLanguageList"/>.
        /// Contains the main flow of <see cref="DrawableLanguageListItem"/> and attaches a placeholder item to the end of the list.
        /// </summary>
        /// <remarks>
        /// Use <see cref="ReplacePlaceholder"/> to transfer the placeholder into the main list.
        /// </remarks>
        private class Scroll : OsuScrollContainer
        {
            /// <summary>
            /// The currently-displayed placeholder item.
            /// </summary>
            public DrawableLanguageListItem PlaceholderItem { get; private set; }

            protected override Container<Drawable> Content => content;
            private readonly Container content;

            private readonly Container<DrawableLanguageListItem> placeholderContainer;

            public Scroll()
            {
                ScrollbarVisible = false;
                Padding = new MarginPadding(10);

                base.Content.Add(new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    LayoutDuration = 200,
                    LayoutEasing = Easing.OutQuint,
                    Children = new Drawable[]
                    {
                        content = new Container { RelativeSizeAxes = Axes.X },
                        placeholderContainer = new Container<DrawableLanguageListItem>
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        }
                    }
                });

                ReplacePlaceholder();
            }

            protected override void Update()
            {
                base.Update();

                // AutoSizeAxes cannot be used as the height should represent the post-layout-transform height at all times, so that the placeholder doesn't bounce around.
                content.Height = ((Flow)Child).Children.Sum(c => c.DrawHeight + 5);
            }

            /// <summary>
            /// Replaces the current <see cref="PlaceholderItem"/> with a new one, and returns the previous.
            /// </summary>
            /// <returns>The current <see cref="PlaceholderItem"/>.</returns>
            public DrawableLanguageListItem ReplacePlaceholder()
            {
                var previous = PlaceholderItem;

                placeholderContainer.Clear(false);
                placeholderContainer.Add(PlaceholderItem = new DrawableLanguageListItem(new BeatmapSetOnlineLanguage(), false));

                return previous;
            }
        }

        /// <summary>
        /// The flow of <see cref="DrawableLanguageListItem"/>. Disables layout easing unless a drag is in progress.
        /// </summary>
        private class Flow : FillFlowContainer<RearrangeableListItem<BeatmapSetOnlineLanguage>>
        {
            public readonly IBindable<bool> DragActive = new Bindable<bool>();

            public Flow()
            {
                Spacing = new Vector2(0, 5);
                LayoutEasing = Easing.OutQuint;
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();
                DragActive.BindValueChanged(active => LayoutDuration = active.NewValue ? 200 : 0);
            }
        }
    }
}
