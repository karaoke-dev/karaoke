// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneLyricRubyRomaji : OsuTestScene
    {
        public const int AREA_MARGIN = 10;

        private readonly LyricPreview lyricPreview;
        private readonly LyricPreviewArea lyricPreviewArea;
        private readonly RubyListPreviewArea rubyListPreviewArea;
        private readonly RomajiListPreviewArea romajiListPreviewArea;

        private readonly IBeatmap beatmap;
        private LyricLine[] lyricLines => beatmap.HitObjects.OfType<LyricLine>().ToArray();

        protected override IBeatmap CreateBeatmap(RulesetInfo ruleset) => new TestKaraokeBeatmap(ruleset);

        public TestSceneLyricRubyRomaji()
        {
            beatmap = CreateBeatmap(new KaraokeRuleset().RulesetInfo);

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.4f)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        lyricPreview = new LyricPreview
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(AREA_MARGIN)
                        },
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    lyricPreviewArea = new LyricPreviewArea
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Margin = new MarginPadding(AREA_MARGIN)
                                    }
                                },
                                new Drawable[]
                                {
                                    new GridContainer
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Content = new[]
                                        {
                                            new Drawable[]
                                            {
                                                rubyListPreviewArea = new RubyListPreviewArea
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Padding = new MarginPadding(AREA_MARGIN)
                                                },
                                                romajiListPreviewArea = new RomajiListPreviewArea
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Padding = new MarginPadding(AREA_MARGIN)
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            lyricPreview.LyricLines = lyricLines;
            lyricPreview.BindableLyricLine.BindValueChanged(value =>
            {
                var newValue = value.NewValue;
                if (newValue == null)
                    return;

                // Apply new lyric line
                lyricPreviewArea.LyricLine = newValue;

                // Apply new tag and max position
                var maxLyricPosition = newValue.Text.Length - 1;
                rubyListPreviewArea.Tags = newValue.RubyTags;
                rubyListPreviewArea.MaxTagPosition = maxLyricPosition;
                romajiListPreviewArea.Tags = newValue.RomajiTags;
                romajiListPreviewArea.MaxTagPosition = maxLyricPosition;
            }, true);

            rubyListPreviewArea.BindableTag.BindValueChanged(value => { lyricPreviewArea.LyricLine.RubyTags = value.NewValue.ToArray(); });

            romajiListPreviewArea.BindableTag.BindValueChanged(value => { lyricPreviewArea.LyricLine.RomajiTags = value.NewValue.ToArray(); });
        }

        public class LyricPreview : Container
        {
            public Bindable<LyricLine> BindableLyricLine => table.BindableLyricLine;

            private readonly Box background;
            private readonly PreviewLyricTable table;

            public LyricPreview()
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
                            RelativeSizeAxes = Axes.Both,
                        },
                        new OsuScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = table = new PreviewLyricTable(),
                        }
                    }
                };
            }

            public LyricLine[] LyricLines
            {
                get => table.LyricLines;
                set => table.LyricLines = value;
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray9;
            }

            public class PreviewLyricTable : TableContainer
            {
                public const int TEXT_HEIGHT = 35;
                public const int SPACING = 5;

                private const float horizontal_inset = 20;
                private const float row_height = 10;

                public Bindable<LyricLine> BindableLyricLine { get; } = new Bindable<LyricLine>();

                private readonly FillFlowContainer backgroundFlow;

                public PreviewLyricTable()
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

                private LyricLine[] lyricLines;

                public LyricLine[] LyricLines
                {
                    get => lyricLines;
                    set
                    {
                        lyricLines = value;

                        Content = null;
                        backgroundFlow.Clear();

                        if (LyricLines?.Any() != true)
                            return;

                        Columns = createHeaders();
                        Content = LyricLines.Select((g, i) => createContent(i, g)).ToArray().ToRectangular();

                        BindableLyricLine.Value = LyricLines.FirstOrDefault();
                    }
                }

                private TableColumn[] createHeaders()
                {
                    var columns = new List<TableColumn>
                    {
                        new TableColumn("Number", Anchor.Centre, new Dimension(GridSizeMode.Absolute, 50)),
                        new TableColumn("Lyric", Anchor.Centre),
                    };

                    return columns.ToArray();
                }

                private Drawable[] createContent(int index, LyricLine line)
                {
                    return new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Text = (index + 1).ToString()
                        },
                        new ClickablePreviewLyricSpriteText(line, BindableLyricLine)
                        {
                            RelativeSizeAxes = Axes.X,
                            Margin = new MarginPadding(10),
                            Font = new FontUsage(size: 32),
                            RubyFont = new FontUsage(size: 12),
                            RomajiFont = new FontUsage(size: 12)
                        }
                    };
                }

                public class ClickablePreviewLyricSpriteText : PreviewLyricSpriteText
                {
                    private readonly Bindable<LyricLine> bindableLyricLine;

                    public ClickablePreviewLyricSpriteText(LyricLine hitObject, Bindable<LyricLine> bindableLyricLine)
                        : base(hitObject)
                    {
                        this.bindableLyricLine = bindableLyricLine;
                    }

                    protected override bool OnClick(ClickEvent e)
                    {
                        bindableLyricLine.Value = HitObject;
                        return base.OnClick(e);
                    }
                }
            }
        }

        public class LyricPreviewArea : Container
        {
            private readonly Container container;

            public LyricPreviewArea()
            {
                Child = new OsuScrollContainer(Direction.Horizontal)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = container = new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        AutoSizeAxes = Axes.X,
                        Padding = new MarginPadding(30),
                    }
                };
            }

            private PreviewLyricSpriteText previewLyricLine;

            public LyricLine LyricLine
            {
                get => previewLyricLine?.HitObject;
                set => container.Child = previewLyricLine = new PreviewLyricSpriteText(value)
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Font = new FontUsage(size: 100),
                    RubyFont = new FontUsage(size: 42),
                    RomajiFont = new FontUsage(size: 42)
                };
            }
        }

        public class RubyListPreviewArea : TagListPreviewArea<RubyTag>
        {
        }

        public class RomajiListPreviewArea : TagListPreviewArea<RomajiTag>
        {
        }

        public class TagListPreviewArea<T> : Container where T : ITag
        {
            private readonly Box background;
            private readonly PreviewTagTable previewTagTable;

            public Bindable<T[]> BindableTag => previewTagTable.BindableTag;

            public TagListPreviewArea()
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
}
