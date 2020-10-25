// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.RubyRomaji.Components
{
    public class LyricList : Container
    {
        public Bindable<Lyric> BindableLyricLine => table.BindableLyricLine;

        private readonly CornerBackground background;
        private readonly PreviewLyricTable table;

        public LyricList()
        {
            Children = new Drawable[]
            {
                background = new CornerBackground
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                },
                new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = table = new PreviewLyricTable(),
                }
            };
        }

        public Lyric[] LyricLines
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

            public Bindable<Lyric> BindableLyricLine { get; } = new Bindable<Lyric>();

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

            private Lyric[] lyricLines;

            public Lyric[] LyricLines
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

            private Drawable[] createContent(int index, Lyric line)
            {
                return new Drawable[]
                {
                    new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = new FontUsage(size: 32),
                        Text = $"#{index + 1}"
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
                private readonly Bindable<Lyric> bindableLyricLine;

                public ClickablePreviewLyricSpriteText(Lyric hitObject, Bindable<Lyric> bindableLyricLine)
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
}
