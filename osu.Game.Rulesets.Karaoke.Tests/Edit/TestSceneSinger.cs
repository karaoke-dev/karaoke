// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneSinger : OsuTestScene
    {
        private readonly KaraokeSingerEditorSkin skinTransformer;

        private readonly Box background;
        private readonly SingerTableContainer singerTableContainer;
        private readonly SingerInfoContainer singerInfoContainer;

        public TestSceneSinger()
        {
            skinTransformer = new KaraokeSingerEditorSkin();

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
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Absolute, 300)
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new OsuScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Child = singerTableContainer = new SingerTableContainer()
                                },
                                new OsuScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Child = singerInfoContainer = new SingerInfoContainer
                                    {
                                        RelativeSizeAxes = Axes.X
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var bindableSinger = skinTransformer.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Singer);
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

            singerTableContainer.BindableSinger.BindValueChanged(x =>
            {
                // Update singer info
                singerInfoContainer.Singer = x.NewValue;
            });
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

            public Bindable<Singer> BindableSinger { get; } = new Bindable<Singer>();

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

            private IDictionary<int, Singer> singers;

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
                    BindableSinger.Value = singers.FirstOrDefault().Value;
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
                    new OsuSpriteText
                    {
                        Text = $"# {index}"
                    },
                    new OsuSpriteText
                    {
                        Text = singer.Name
                    },
                };
            }
        }

        public class SingerInfoContainer : TableContainer
        {
            private readonly OsuTextBox nameTextBox;
            private readonly OsuTextBox englishNameTextBox;
            private readonly OsuTextBox romajiNameTextBox;

            public SingerInfoContainer()
            {
                AutoSizeAxes = Axes.Y;
                RowSize = new Dimension(GridSizeMode.AutoSize);
                Content = new Drawable[,]
                {
                    {
                        new OsuSpriteText
                        {
                            Text = "Singer name :"
                        },
                        nameTextBox = new OsuTextBox
                        {
                            RelativeSizeAxes = Axes.X,
                        }
                    },
                    {
                        new OsuSpriteText
                        {
                            Text = "English name :"
                        },
                        englishNameTextBox = new OsuTextBox
                        {
                            RelativeSizeAxes = Axes.X,
                        }
                    },
                    {
                        new OsuSpriteText
                        {
                            Text = "Romaji name :"
                        },
                        romajiNameTextBox = new OsuTextBox
                        {
                            RelativeSizeAxes = Axes.X,
                        }
                    },
                };

                nameTextBox.Current.BindValueChanged(x => { Singer.Name = x.NewValue; });

                englishNameTextBox.Current.BindValueChanged(x => { Singer.EnglishName = x.NewValue; });

                romajiNameTextBox.Current.BindValueChanged(x => { Singer.RomajiName = x.NewValue; });
            }

            private Singer singer;

            public Singer Singer
            {
                get => singer;
                set
                {
                    singer = value;
                    nameTextBox.Text = singer.Name;
                    englishNameTextBox.Text = singer.EnglishName;
                    romajiNameTextBox.Text = singer.RomajiName;
                }
            }
        }

        public class KaraokeSingerEditorSkin : KaraokeLegacySkinTransformer
        {
            public KaraokeSingerEditorSkin()
                : base(null)
            {
            }
        }
    }
}
