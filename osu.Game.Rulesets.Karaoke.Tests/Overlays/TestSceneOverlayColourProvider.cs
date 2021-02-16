// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Tests.Visual;
using osu.Game.Rulesets.Karaoke.Extensions;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays
{
    [TestFixture]
    public class TestSceneOverlayColourProvider : OsuTestScene
    {
        [Test]
        public void ShowWithNoFetch()
        {
            var providers = new []
            {
                OverlayColourProvider.Red,
                OverlayColourProvider.Pink,
                OverlayColourProvider.Orange,
                OverlayColourProvider.Green,
                OverlayColourProvider.Purple,
                OverlayColourProvider.Blue
            };
            var colourName = new[]
            {
                "Colour1",
                "Colour2",
                "Colour3",
                "Colour4",
                "Highlight1",
                "Content1",
                "Content2",
                "Light1",
                "Light2",
                "Light3",
                "Light4",
                "Dark1",
                "Dark2",
                "Dark3",
                "Dark4",
                "Dark5",
                "Dark6",
                "Foreground1",
                "Background1",
                "Background2",
                "Background3",
                "Background4",
                "Background5",
                "Background6",
            };

            Child = new OsuScrollContainer(Direction.Horizontal)
            {
                RelativeSizeAxes = Axes.Both,
                Child = new TableContainer
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Columns = colourName.Select(c => new TitleTableColumn(c)).ToArray(),
                    Content = providers.Select(p =>
                    {
                        return colourName.Select(c =>
                        {
                            var colour = (Color4)p.GetType().GetProperty(c).GetValue(p);
                            return new PreviewColourDrawable(colour);
                        });
                    }).To2DArray(),
                }
            };
        }

        private class TitleTableColumn : TableColumn
        {
            public TitleTableColumn(string title)
                : base(title, Anchor.Centre, new Dimension(GridSizeMode.Absolute, 120))
            {
            }
        }

        private class PreviewColourDrawable : CompositeDrawable
        {
            public PreviewColourDrawable(Color4 color)
            {
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
        }
    }
}
