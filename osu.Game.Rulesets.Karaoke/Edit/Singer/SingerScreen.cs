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

namespace osu.Game.Rulesets.Karaoke.Edit.Singer
{
    public class SingerScreen : EditorScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly SingerManager SingerManager;

        public SingerScreen()
            : base(EditorScreenMode.SongSetup)
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Purple);
            Content.Add(SingerManager = new SingerManager());
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
                        new SectionsContainer<Container>
                        {
                            FixedHeader = new SingerScreenHeader(),
                            RelativeSizeAxes = Axes.Both,
                            Children = new Container[]
                            {
                                new SingerEditSection()
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                },
                            }
                        },
                    }
                }
            };
        }

        internal class SingerScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new TranslateScreenTitle();

            private class TranslateScreenTitle : OverlayTitle
            {
                public TranslateScreenTitle()
                {
                    Title = "singer";
                    Description = "create singer of your beatmap";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}
