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
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Layouts
{
    public class LayoutSelection : FillFlowContainer
    {
        private const float padding = 20;
        private const float spacing = 10;

        private const float selection_size = (240 - padding * 2 - spacing) / 2;

        public LayoutSelection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Spacing = new Vector2(spacing);
            Direction = FillDirection.Full;
            Padding = new MarginPadding(padding);
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
                AddInternal(new LayoutSelectionItem(layout)
                {
                    Size = new Vector2(selection_size)
                });
            }

            state.BindableCaretPosition.BindValueChanged(e =>
            {
                var lyric = e.NewValue?.Lyric;
                var layoutSelectionItems = InternalChildren.OfType<LayoutSelectionItem>();

                foreach (var item in layoutSelectionItems)
                {
                    item.Lyric = lyric;
                }
            });
        }

        public class LayoutSelectionItem : CompositeDrawable
        {
            private readonly Bindable<int> selectedLayoutIndex = new Bindable<int>();

            private readonly DrawableLayoutPreview drawableLayoutPreview;

            private readonly LyricLayout layout;

            public LayoutSelectionItem(LyricLayout layout)
            {
                this.layout = layout;

                Masking = true;
                CornerRadius = 5f;

                InternalChildren = new Drawable[]
                {
                    drawableLayoutPreview = new DrawableLayoutPreview
                    {
                        RelativeSizeAxes = Axes.Both,
                        Layout = layout,
                    }
                };

                selectedLayoutIndex.BindValueChanged(e =>
                {
                    var selected = layout.ID == e.NewValue;
                    BorderThickness = selected ? 3 : 0;
                }, true);
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colour)
            {
                BorderColour = colour.Yellow;
            }

            public Lyric Lyric
            {
                get => drawableLayoutPreview.Lyric;
                set
                {
                    if (drawableLayoutPreview.Lyric == value)
                        return;

                    // unbind previous event
                    Lyric?.LayoutIndexBindable.UnbindFrom(selectedLayoutIndex);

                    // update lyric
                    drawableLayoutPreview.Lyric = value;

                    // bind layout index.
                    Lyric?.LayoutIndexBindable.BindTo(selectedLayoutIndex);
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
