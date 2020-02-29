// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Tests.Visual;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using System.Linq;
using osu.Framework.Extensions;
using osu.Framework.Allocation;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneSinger : OsuTestScene
    {
        private readonly KaraokeLegacySkinTransformer skinTransformer;

        private readonly Box background;
        private readonly SingerTableContainer singerTableContainer;

        public TestSceneSinger()
        {
            skinTransformer = new KaraokeLegacySkinTransformer(null);

            Child = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = 700,
                Height = 500,
                Masking = true,
                CornerRadius = 5,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        Name = "Background",
                        RelativeSizeAxes = Axes.Both
                    },
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        ColumnDimensions = new Dimension[]
                        {
                            new Dimension(GridSizeMode.Absolute, 300)
                        },
                        Content = new Drawable[][]
                        {
                            new []
                            {
                                new OsuScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Child = singerTableContainer = new SingerTableContainer
                                    {

                                    }
                                },
                                new OsuScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                            
                        }
                    }
                }
            };

            var bindableSinger = skinTransformer.GetConfig<KaraokeIndexLookup, Dictionary<int, string>>(KaraokeIndexLookup.Singer);
            bindableSinger.BindValueChanged(x =>
            {
                var singers = x.NewValue.ToDictionary(i => i.Key, v =>
                {
                    // TODO : get lookup by number
                    var lookup = new KaraokeSkinLookup(KaraokeSkinConfiguration.Singer, 0);
                    return skinTransformer.GetConfig<KaraokeSkinLookup, Singer>(lookup).Value;
                });

                singerTableContainer.Singers = singers;
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray9;
        }

        public class SingerTableContainer : TableContainer
        {
            private const float horizontal_inset = 20;
            private const float row_height = 10;

            public BindableInt BindableLayoutIndex { get; } = new BindableInt();

            private readonly FillFlowContainer backgroundFlow;

            public SingerTableContainer()
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

            private IDictionary<int,Singer> singers;

            public IDictionary<int, Singer> Singers
            {
                get => singers;
                set
                {
                    singers = value;

                    Content = null;
                    backgroundFlow.Clear();

                    if (Singers?.Any() != true)
                        return;

                    Columns = createHeaders();
                    Content = Singers.Select(s => createContent(s.Key, s.Value)).ToArray().ToRectangular();

                    // All the singer is not selected
                    BindableLayoutIndex.Value = 0;
                }
            }

            private TableColumn[] createHeaders()
            {
                var columns = new List<TableColumn>
                    {
                        new TableColumn("Selected", Anchor.Centre, new Dimension(GridSizeMode.Absolute, 50)),
                        new TableColumn("Singer name", Anchor.Centre),
                    };

                return columns.ToArray();
            }

            private Drawable[] createContent(int index, Singer singer)
            {
                return new Drawable[]
                    {
                        new OsuCheckbox
                        {
                            
                        },
                        new OsuSpriteText
                        {
                            Text = singer.Name
                        },
                    };
            }
        }
    }
}
