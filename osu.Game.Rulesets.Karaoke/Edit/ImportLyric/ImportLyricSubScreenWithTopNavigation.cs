// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class ImportLyricSubScreenWithTopNavigation : ImportLyricSubScreen
    {
        public ImportLyricSubScreenWithTopNavigation()
        {
            Padding = new MarginPadding(10);
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 40),
                    new Dimension(GridSizeMode.Absolute, 10),
                    new Dimension(GridSizeMode.Distributed)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new TopNavigation
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    },
                    new Drawable[] { },
                    new Drawable[]
                    {
                        CreateContent(),
                    }
                }
            };
        }

        protected virtual Drawable CreateContent() => new Container();

        public class TopNavigation : Container
        {
            private readonly CornerBackground background;

            public TopNavigation()
            {
                AddInternal(background = new CornerBackground
                {
                    RelativeSizeAxes = Axes.Both,
                });
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray2;
            }
        }
    }
}
