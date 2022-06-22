// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Layout
{
    public class LayoutSelection : Section
    {
        private const float layout_setting_horizontal_padding = 20;

        private const float layout_setting_vertical_spacing = 15;

        protected override LocalisableString Title => "Layout";

        public LayoutSelection()
        {
            Padding = new MarginPadding
            {
                Horizontal = layout_setting_horizontal_padding,
                Vertical = SECTION_PADDING,
            };

            if (Content is not FillFlowContainer fillFlowContainer)
                return;

            // use lazy way to initialize fill flow container in section.
            fillFlowContainer.Direction = FillDirection.Full;
            fillFlowContainer.Spacing = new Vector2(SECTION_SPACING, layout_setting_vertical_spacing);
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skinSource)
        {
            var layoutDictionary = skinSource.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            if (layoutDictionary == null)
                return;

            foreach (var layoutIndex in layoutDictionary)
            {
                var layout = skinSource.GetConfig<KaraokeSkinLookup, LyricLayout>(new KaraokeSkinLookup(ElementType.LyricLayout, layoutIndex.Key))?.Value;
                Content.Add(new LayoutSelectionItem(layout));
            }

            // todo: load current layout.
            /*
            lyricCaretState.BindableCaretPosition.BindValueChanged(e =>
            {
                var lyric = e.NewValue?.Lyric;
                var layoutSelectionItems = Content.Children.OfType<LayoutSelectionItem>();

                foreach (var item in layoutSelectionItems)
                {
                    item.Lyric = lyric;
                }
            }, true);
            */
        }

        public class LayoutSelectionItem : FillFlowContainer
        {
            private const float selection_size = (240 - layout_setting_horizontal_padding * 2 - SECTION_SPACING) / 2;

            // todo: should changed into selected layout index
            private readonly Bindable<int> selectedLayoutIndex = new();

            private readonly DrawableLayoutPreview drawableLayoutPreview;

            private readonly Container cornerContainer;

            private readonly LyricLayout layout;

            public LayoutSelectionItem(LyricLayout layout)
            {
                this.layout = layout;

                AutoSizeAxes = Axes.Both;
                Spacing = new Vector2(5);
                Direction = FillDirection.Vertical;

                InternalChildren = new Drawable[]
                {
                    cornerContainer = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = 5f,
                        Child = drawableLayoutPreview = new DrawableLayoutPreview
                        {
                            Size = new Vector2(selection_size),
                            Layout = layout,
                        }
                    },
                    new OsuSpriteText
                    {
                        Text = layout.Name
                    }
                };

                selectedLayoutIndex.BindValueChanged(e =>
                {
                    bool selected = layout.ID == e.NewValue;
                    cornerContainer.BorderThickness = selected ? 3 : 0;
                }, true);
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colour)
            {
                cornerContainer.BorderColour = colour.Yellow;
            }

            public Lyric Lyric
            {
                get => drawableLayoutPreview.Lyric;
                set
                {
                    if (drawableLayoutPreview.Lyric == value)
                        return;

                    // update lyric
                    drawableLayoutPreview.Lyric = value;
                }
            }

            protected override bool OnClick(ClickEvent e)
            {
                selectedLayoutIndex.Value = layout.ID;
                return base.OnClick(e);
            }
        }
    }
}
