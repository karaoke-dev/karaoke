// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Screens.Edit.Components.Menus;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Menus
{
    /// <summary>
    /// Copied from <see cref="ScreenSelectionTabControl"/>
    /// </summary>
    /// <typeparam name="TScreenMode"></typeparam>
    public class GenericScreenSelectionTabControl<TScreenMode> : OsuTabControl<TScreenMode>
    {
        public GenericScreenSelectionTabControl()
        {
            AutoSizeAxes = Axes.X;
            RelativeSizeAxes = Axes.Y;

            TabContainer.RelativeSizeAxes &= ~Axes.X;
            TabContainer.AutoSizeAxes = Axes.X;
            TabContainer.Padding = new MarginPadding();

            AddInternal(new Box
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                RelativeSizeAxes = Axes.X,
                Height = 1,
                Colour = Color4.White.Opacity(0.2f),
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            AccentColour = colours.Yellow;
        }

        protected override Dropdown<TScreenMode> CreateDropdown() => null;

        protected override TabItem<TScreenMode> CreateTabItem(TScreenMode value) => new TabItem(value);

        private class TabItem : OsuTabItem
        {
            private const float transition_length = 250;

            public TabItem(TScreenMode value)
                : base(value)
            {
                Text.Margin = new MarginPadding();
                Text.Anchor = Anchor.CentreLeft;
                Text.Origin = Anchor.CentreLeft;
            }

            protected override void OnActivated()
            {
                base.OnActivated();
                Bar.ScaleTo(new Vector2(1, 5), transition_length, Easing.OutQuint);
            }

            protected override void OnDeactivated()
            {
                base.OnDeactivated();
                Bar.ScaleTo(Vector2.One, transition_length, Easing.OutQuint);
            }
        }
    }
}
