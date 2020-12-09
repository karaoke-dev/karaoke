// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;
using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class LanguageSelectionDialog : TitleFocusedOverlayContainer
    {
        protected override string Title => "Select language";

        public LanguageSelectionDialog()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.5f, 0.8f);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            var languages = new BindableList<CultureInfo>(CultureInfo.GetCultures(CultureTypes.AllCultures));
            Children = new Drawable[]
            {
                new DrawableLanguageList
                {
                    RelativeSizeAxes = Axes.Both,
                    Items = { BindTarget = languages }
                }
            };
        }

        public class DrawableLanguageList : OsuRearrangeableListContainer<CultureInfo>
        {
            protected override FillFlowContainer<RearrangeableListItem<CultureInfo>> CreateListFillFlowContainer() => new Flow
            {
                DragActive = { BindTarget = DragActive }
            };

            protected override OsuRearrangeableListItem<CultureInfo> CreateOsuDrawable(CultureInfo item)
                => new DrawableLanguageListItem(item);

            /// <summary>
            /// The flow of <see cref="DrawableLanguageListItem"/>. Disables layout easing unless a drag is in progress.
            /// </summary>
            private class Flow : FillFlowContainer<RearrangeableListItem<CultureInfo>>
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

            public class DrawableLanguageListItem : OsuRearrangeableListItem<CultureInfo>
            {
                public DrawableLanguageListItem(CultureInfo item)
                    : base(item)
                {
                    Padding = new MarginPadding { Left = 5 };
                }

                protected override Drawable CreateContent() => new OsuTextFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Text = Model.DisplayName,
                };
            }
        }
    }
}
