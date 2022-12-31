// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Components
{
    public partial class TestSceneLyricEditorColourProvider : OsuTestScene
    {
        [Test]
        public void ShowWithNoFetch()
        {
            var provider = new LyricEditorColourProvider();
            var types = Enum.GetValues<LyricEditorMode>();

            string[] colourName =
            {
                nameof(LyricEditorColourProvider.Colour1),
                nameof(LyricEditorColourProvider.Colour2),
                nameof(LyricEditorColourProvider.Colour3),
                nameof(LyricEditorColourProvider.Colour4),
                nameof(LyricEditorColourProvider.Highlight1),
                nameof(LyricEditorColourProvider.Content1),
                nameof(LyricEditorColourProvider.Content2),
                nameof(LyricEditorColourProvider.Light1),
                nameof(LyricEditorColourProvider.Light2),
                nameof(LyricEditorColourProvider.Light3),
                nameof(LyricEditorColourProvider.Light4),
                nameof(LyricEditorColourProvider.Dark1),
                nameof(LyricEditorColourProvider.Dark2),
                nameof(LyricEditorColourProvider.Dark3),
                nameof(LyricEditorColourProvider.Dark4),
                nameof(LyricEditorColourProvider.Dark5),
                nameof(LyricEditorColourProvider.Dark6),
                nameof(LyricEditorColourProvider.Foreground1),
                nameof(LyricEditorColourProvider.Background1),
                nameof(LyricEditorColourProvider.Background2),
                nameof(LyricEditorColourProvider.Background3),
                nameof(LyricEditorColourProvider.Background4),
                nameof(LyricEditorColourProvider.Background5),
                nameof(LyricEditorColourProvider.Background6),
            };

            Schedule(() =>
            {
                var editMOdeColumns = new TableColumn[]
                {
                    new TitleTableColumn("Edit mode")
                };
                var editModeContent = types.Select(type =>
                {
                    return new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Text = type.ToString()
                        }
                    };
                }).To2DArray();

                var columns = colourName.Select(c => new TitleTableColumn(c)).OfType<TableColumn>().ToArray();
                var content = types.Select(type =>
                {
                    return colourName.Select(c =>
                    {
                        object? value = provider.GetType().GetMethod(c)?.Invoke(provider, new object[] { type });
                        if (value == null)
                            throw new ArgumentNullException(nameof(value));

                        var colour = (Color4)value;
                        return new PreviewColourDrawable(colour);
                    }).OfType<Drawable>();
                }).To2DArray();

                const int edit_mode_name_width = 120;
                Child = new OsuScrollContainer(Direction.Horizontal)
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new TableContainer
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = edit_mode_name_width,
                            Columns = editMOdeColumns,
                            Content = editModeContent,
                        },
                        new TableContainer
                        {
                            RelativeSizeAxes = Axes.Y,
                            AutoSizeAxes = Axes.X,
                            Columns = columns,
                            Content = content,
                            Margin = new MarginPadding
                            {
                                Left = edit_mode_name_width
                            }
                        }
                    }
                };
            });
        }

        private class TitleTableColumn : TableColumn
        {
            public TitleTableColumn(string title)
                : base(title, Anchor.Centre, new Dimension(GridSizeMode.Absolute, 120))
            {
            }
        }

        private partial class PreviewColourDrawable : CompositeDrawable
        {
            [Resolved, AllowNull]
            private GameHost host { get; set; }

            private readonly Color4 color;

            public PreviewColourDrawable(Color4 color)
            {
                this.color = color;

                RelativeSizeAxes = Axes.Both;
                InternalChildren = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = color,
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = color.ToHex(),
                    }
                };
            }

            protected override bool OnClick(ClickEvent e)
            {
                host.GetClipboard()?.SetText(color.ToHex());
                return base.OnClick(e);
            }
        }
    }
}
