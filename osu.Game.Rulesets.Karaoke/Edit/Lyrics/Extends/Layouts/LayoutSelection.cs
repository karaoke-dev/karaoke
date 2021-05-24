// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Layouts
{
    public class LayoutSelection : Section
    {
        private const float layout_settion_horizontal_padding = 20;

        private const float layout_setting_vertical_spacing = 15;

        protected override string Title => "Layout";

        public LayoutSelection()
        {
            Padding = new MarginPadding
            {
                Horizontal = layout_settion_horizontal_padding,
                Vertical = SECTION_PADDING,
            };

            if (!(Content is FillFlowContainer fillFlowContainer))
                return;

            // use lazy way to initialize fill flow container in section.
            fillFlowContainer.Direction = FillDirection.Full;
            fillFlowContainer.Spacing = new Vector2(SECTION_SPACING, layout_setting_vertical_spacing);
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skinSource, ILyricEditorState state)
        {
            var layoutDictionary = skinSource.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            if (layoutDictionary == null)
                return;

            foreach (var layoutIndex in layoutDictionary)
            {
                var layout = skinSource?.GetConfig<KaraokeSkinLookup, LyricLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, layoutIndex.Key)).Value;
                Content.Add(new LayoutSelectionItem(layout));
            }

            state.BindableCaretPosition.BindValueChanged(e =>
            {
                var lyric = e.NewValue?.Lyric;
                var layoutSelectionItems = Content.Children.OfType<LayoutSelectionItem>();

                foreach (var item in layoutSelectionItems)
                {
                    item.Lyric = lyric;
                }
            }, true);
        }

        public class LayoutSelectionItem : FillFlowContainer
        {
            private const float selection_size = (240 - layout_settion_horizontal_padding * 2 - SECTION_SPACING) / 2;

            private readonly Bindable<int> selectedLayoutIndex = new Bindable<int>();

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
                    var selected = layout.ID == e.NewValue;
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

                    // unbind previous event
                    if (Lyric != null)
                    {
                        selectedLayoutIndex.UnbindFrom(Lyric.LayoutIndexBindable);
                    }

                    // update lyric
                    drawableLayoutPreview.Lyric = value;

                    // bind layout index.
                    if (Lyric != null)
                    {
                        selectedLayoutIndex.BindTo(Lyric.LayoutIndexBindable);
                    }
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
