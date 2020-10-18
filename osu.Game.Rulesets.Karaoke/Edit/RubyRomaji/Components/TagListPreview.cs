// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.RubyRomaji.Components
{
    public class TagListPreview<T> : Container where T : ITag
    {
        private readonly Box background;
        private readonly PreviewTagTable previewTagTable;

        public Bindable<T[]> BindableTag => previewTagTable.BindableTag;

        public TagListPreview()
        {
            Child = new Container
            {
                Masking = true,
                CornerRadius = 5,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        Name = "Background",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                    },
                    new OsuScrollContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = previewTagTable = new PreviewTagTable(),
                    }
                }
            };
        }

        public int MaxTagPosition
        {
            get => previewTagTable.MaxPosition;
            set => previewTagTable.MaxPosition = value;
        }

        public T[] Tags
        {
            get => previewTagTable.Tags;
            set => previewTagTable.Tags = value;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray9;
        }

        public class PreviewTagTable : TableContainer
        {
            private const float horizontal_inset = 20;
            private const float row_height = 100;

            public Bindable<T[]> BindableTag { get; } = new Bindable<T[]>();

            private readonly Cached tagsCache = new Cached();

            private readonly FillFlowContainer backgroundFlow;

            public PreviewTagTable()
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                Padding = new MarginPadding { Horizontal = horizontal_inset };
                RowSize = new Dimension(GridSizeMode.AutoSize);

                AddInternal(backgroundFlow = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Depth = 1f,
                    Padding = new MarginPadding { Horizontal = -horizontal_inset },
                    Margin = new MarginPadding { Top = row_height }
                });
            }

            private int maxPosition;

            public int MaxPosition
            {
                get => maxPosition;
                set
                {
                    maxPosition = value;
                    tagsCache.Invalidate();
                }
            }

            private T[] tags;

            public T[] Tags
            {
                get => tags;
                set
                {
                    tags = value;
                    BindableTag.Value = value;
                    tagsCache.Invalidate();
                }
            }

            protected override void Update()
            {
                base.Update();

                //Re-create content;
                if (tagsCache.IsValid) return;

                Content = null;
                backgroundFlow.Clear();

                if (Tags?.Any() != true)
                    return;

                Columns = createHeaders();
                Content = Tags.Select((g, i) => createContent(i, g)).ToArray().ToRectangular();

                tagsCache.Validate();
            }

            private TableColumn[] createHeaders()
            {
                var columns = new List<TableColumn>
                {
                    new TableColumn("Start", Anchor.TopCentre, new Dimension(GridSizeMode.Absolute, 50)),
                    new TableColumn("End", Anchor.TopCentre, new Dimension(GridSizeMode.Absolute, 50)),
                    new TableColumn("Text", Anchor.TopCentre),
                };

                return columns.ToArray();
            }

            private Drawable[] createContent(int index, T tag)
            {
                // IDK why but it only works with Bindable<ITag>, Bindable<T> doesn't work
                var bindableTag = new Bindable<ITag>(tag);

                OsuDropdown<int> startPositionDropdown;
                OsuDropdown<int> endPositionDropdown;
                OsuTextBox textBox;

                var content = new Drawable[]
                {
                    startPositionDropdown = new OsuDropdown<int>
                    {
                        RelativeSizeAxes = Axes.X,
                    },
                    endPositionDropdown = new OsuDropdown<int>
                    {
                        RelativeSizeAxes = Axes.X,
                    },
                    textBox = new OsuTextBox
                    {
                        RelativeSizeAxes = Axes.X,
                        Text = tag.Text
                    }
                };

                startPositionDropdown.Items = Enumerable.Range(0, MaxPosition + 1).ToList();
                startPositionDropdown.Current.Value = tag.StartIndex;
                startPositionDropdown.Current.BindValueChanged(value =>
                {
                    var newValue = value.NewValue;

                    if (value.OldValue != newValue)
                    {
                        bindableTag.Value.StartIndex = newValue;
                        bindableTag.TriggerChange();
                    }

                    // Update end dropdown's value
                    endPositionDropdown.Items = Enumerable.Range(newValue + 1, MaxPosition - newValue + 1).ToList();
                }, true);

                endPositionDropdown.Current.Value = tag.EndIndex;
                endPositionDropdown.Current.BindValueChanged(value =>
                {
                    var newValue = value.NewValue;
                    if (value.OldValue == newValue)
                        return;

                    bindableTag.Value.EndIndex = newValue;
                    bindableTag.TriggerChange();
                });

                textBox.Current.BindValueChanged(value =>
                {
                    bindableTag.Value.Text = value.NewValue;
                    bindableTag.TriggerChange();
                });

                bindableTag.BindValueChanged(value =>
                {
                    // Copy and trigger bindable
                    var newValue = value.NewValue;
                    Tags[index].StartIndex = newValue.StartIndex;
                    Tags[index].EndIndex = newValue.EndIndex;
                    Tags[index].Text = newValue.Text;
                    BindableTag.TriggerChange();
                });

                return content;
            }
        }
    }
}
