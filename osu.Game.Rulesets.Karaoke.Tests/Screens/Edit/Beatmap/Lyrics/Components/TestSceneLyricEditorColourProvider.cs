// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
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

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Components;

public partial class TestSceneLyricEditorColourProvider : OsuTestScene
{
    [Test]
    public void ShowWithNoFetch()
    {
        var provider = new LyricEditorColourProvider();
        var types = Enum.GetValues<LyricEditorMode>();

        var colourMethods = typeof(LyricEditorColourProvider)
                            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                            .Where(x =>
                            {
                                var parameters = x.GetBaseDefinition().GetParameters();
                                return parameters.Length == 1 && parameters[0].ParameterType == typeof(LyricEditorMode);
                            }).ToArray();

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

            var columns = colourMethods.Select(c => new TitleTableColumn(c.Name)).OfType<TableColumn>().ToArray();
            var content = types.Select(type =>
            {
                return colourMethods.Select(c =>
                {
                    object? value = c.Invoke(provider, new object[] { type });
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
        [Resolved]
        private GameHost host { get; set; } = null!;

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
