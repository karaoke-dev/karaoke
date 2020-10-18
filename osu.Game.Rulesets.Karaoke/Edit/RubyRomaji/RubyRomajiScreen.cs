// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.RubyRomaji
{
    public class RubyRomajiScreen : EditorScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        public RubyRomajiScreen()
            : base(EditorScreenMode.SongSetup)
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Orange);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(50),
                Child = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 10,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = colours.GreySeafoamDark,
                            RelativeSizeAxes = Axes.Both,
                        },
                        new FixedSectionsContainer<Container>
                        {
                            FixedHeader = new RubyRomajiScreenHeader(),
                            RelativeSizeAxes = Axes.Both,
                            Children = new Container[]
                            {
                                new RubyRomajiEditSection(EditorBeatmap)
                                {
                                    RelativeSizeAxes = Axes.Both,
                                },
                            }
                        },
                    }
                }
            };
        }

        internal class FixedSectionsContainer<T> : SectionsContainer<T> where T : Drawable
        {
            private readonly Container<T> content;

            protected override Container<T> Content => content;

            public FixedSectionsContainer()
            {
                AddInternal(content = new Container<T>
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Top = 55 }
                });
            }
        }

        internal class RubyRomajiScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new RubyRomajiScreenTitle();

            private class RubyRomajiScreenTitle : OverlayTitle
            {
                public RubyRomajiScreenTitle()
                {
                    Title = "ruby/romaji";
                    Description = "create ruby/romaji of your beatmap";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}
