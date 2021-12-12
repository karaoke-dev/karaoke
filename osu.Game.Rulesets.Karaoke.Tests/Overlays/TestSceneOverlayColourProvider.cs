// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays
{
    [TestFixture]
    public class TestSceneOverlayColourProvider : OsuTestScene
    {
        [Test]
        public void ShowWithNoFetch()
        {
            var providers = new[]
            {
                OverlayColourProvider.Red,
                OverlayColourProvider.Pink,
                OverlayColourProvider.Orange,
                OverlayColourProvider.Green,
                OverlayColourProvider.Purple,
                OverlayColourProvider.Blue
            };
            string[] colourName = new[]
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

            Schedule(() =>
            {
                var columns = colourName.Select(c => new TitleTableColumn(c)).OfType<TableColumn>().ToArray();
                var content = providers.Select(provider =>
                {
                    if (provider == null)
                        throw new ArgumentNullException(nameof(provider));

                    return colourName.Select(c =>
                    {
                        var value = provider.GetType().GetProperty(c)?.GetValue(provider);
                        if (value == null)
                            throw new ArgumentNullException(nameof(value));

                        var colour = (Color4)value;
                        return new PreviewColourDrawable(colour);
                    }).OfType<Drawable>();
                }).To2DArray();

                Child = new OsuScrollContainer(Direction.Horizontal)
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = new TableContainer
                    {
                        RelativeSizeAxes = Axes.Y,
                        AutoSizeAxes = Axes.X,
                        Columns = columns,
                        Content = content,
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

        private class PreviewColourDrawable : CompositeDrawable
        {
            [Resolved]
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
                host.GetClipboard().SetText(color.ToHex());
                return base.OnClick(e);
            }
        }
    }
}
