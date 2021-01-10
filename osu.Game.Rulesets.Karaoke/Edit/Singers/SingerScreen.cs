// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerScreen : EditorSubScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly SingerManager SingerManager;

        public SingerScreen()
            : base()
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
                        new FixedSectionsContainer<Drawable>
                        {
                            FixedHeader = new SingerScreenHeader(),
                            RelativeSizeAxes = Axes.Both,
                            Children = new[]
                            {
                                new SingerEditSection
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
