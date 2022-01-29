// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Layout.Components
{
    public class DrawableLayoutList : OsuRearrangeableListContainer<LyricLayout>
    {
        private Scroll scroll;

        protected override ScrollContainer<Drawable> CreateScrollContainer() => scroll = new Scroll();

        protected override FillFlowContainer<RearrangeableListItem<LyricLayout>> CreateListFillFlowContainer() => new Flow
        {
            DragActive = { BindTarget = DragActive }
        };

        protected override OsuRearrangeableListItem<LyricLayout> CreateOsuDrawable(LyricLayout item)
        {
            if (item == scroll.PlaceholderItem.Model)
                return scroll.ReplacePlaceholder();

            return new DrawableLayoutListItem(item, true);
        }

        /// <summary>
        /// The scroll container for this <see cref="DrawableLayoutList"/>.
        /// Contains the main flow of <see cref="DrawableLayoutListItem"/> and attaches a placeholder item to the end of the list.
        /// </summary>
        /// <remarks>
        /// Use <see cref="ReplacePlaceholder"/> to transfer the placeholder into the main list.
        /// </remarks>
        private class Scroll : OsuScrollContainer
        {
            /// <summary>
            /// The currently-displayed placeholder item.
            /// </summary>
            public DrawableLayoutListItem PlaceholderItem { get; private set; }

            protected override Container<Drawable> Content => content;
            private readonly Container content;

            private readonly Container<DrawableLayoutListItem> placeholderContainer;

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
                        placeholderContainer = new Container<DrawableLayoutListItem>
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
            public DrawableLayoutListItem ReplacePlaceholder()
            {
                var previous = PlaceholderItem;

                placeholderContainer.Clear(false);
                placeholderContainer.Add(PlaceholderItem = new DrawableLayoutListItem(new LyricLayout(), false));

                return previous;
            }
        }

        /// <summary>
        /// The flow of <see cref="DrawableLayoutListItem"/>. Disables layout easing unless a drag is in progress.
        /// </summary>
        private class Flow : FillFlowContainer<RearrangeableListItem<LyricLayout>>
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
