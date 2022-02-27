// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags.Components
{
    public class LabelledOpacityAdjustment : LabelledSwitchButton
    {
        protected const float CONFIG_BUTTON_SIZE = 20f;

        private readonly OpacityButton opacityButton;

        public LabelledOpacityAdjustment()
        {
            if (InternalChildren[1] is not FillFlowContainer fillFlowContainer)
                return;

            // change padding to place config button.
            fillFlowContainer.Padding = new MarginPadding
            {
                Horizontal = CONTENT_PADDING_HORIZONTAL,
                Vertical = CONTENT_PADDING_VERTICAL,
                Right = CONTENT_PADDING_HORIZONTAL + CONFIG_BUTTON_SIZE + CONTENT_PADDING_HORIZONTAL,
            };

            // add config button.
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding
                {
                    Top = CONTENT_PADDING_VERTICAL,
                    Right = CONTENT_PADDING_HORIZONTAL,
                },
                Child = opacityButton = new OpacityButton
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Size = new Vector2(CONFIG_BUTTON_SIZE),
                }
            });

            Component.Current.BindValueChanged(_ => updateConfigButtonOpacity(), true);
        }

        private void updateConfigButtonOpacity()
        {
            bool showOpacityButton = Component.Current.Value;
            opacityButton.FadeTo(showOpacityButton ? 1 : 0.3f, 200, Easing.OutQuint);
            opacityButton.Enabled.Value = showOpacityButton;
        }

        public Bindable<float> Opacity
        {
            set => opacityButton.Current = value;
        }

        private class OpacityButton : IconButton, IHasPopover, IHasCurrentValue<float>
        {
            public Bindable<float> Current { get; set; }

            public OpacityButton()
            {
                Icon = FontAwesome.Solid.Cog;
                Action = this.ShowPopover;
            }

            public Popover GetPopover()
                => new OsuPopover
                {
                    Child = new OpacitySliderBar
                    {
                        Width = 150,
                        Current = Current,
                    }
                };

            private class OpacitySliderBar : OsuSliderBar<float>
            {
                public override LocalisableString TooltipText => (Current.Value * 100).ToString("N0") + "%";
            }
        }
    }
}
