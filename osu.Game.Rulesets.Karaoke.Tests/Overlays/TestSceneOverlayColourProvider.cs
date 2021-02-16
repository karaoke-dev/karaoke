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
            Child = new TableContainer
            {
                RelativeSizeAxes = Axes.Both,
                Columns = new TableColumn[]
                {
                    new TitleTableColumn("Colour1"),
                    new TitleTableColumn("Colour2"),
                    new TitleTableColumn("Colour3"),
                    new TitleTableColumn("Colour4"),
                    new TitleTableColumn("Highlight1"),
                    new TitleTableColumn("Content1"),
                    new TitleTableColumn("Content2"),
                    new TitleTableColumn("Light1"),
                    new TitleTableColumn("Light2"),
                    new TitleTableColumn("Light3"),
                    new TitleTableColumn("Light4"),
                    new TitleTableColumn("Dark1"),
                    new TitleTableColumn("Dark2"),
                    new TitleTableColumn("Dark3"),
                    new TitleTableColumn("Dark4"),
                    new TitleTableColumn("Dark5"),
                    new TitleTableColumn("Dark6"),
                    new TitleTableColumn("Foreground1"),
                    new TitleTableColumn("Background1"),
                    new TitleTableColumn("Background2"),
                    new TitleTableColumn("Background3"),
                    new TitleTableColumn("Background4"),
                    new TitleTableColumn("Background5"),
                    new TitleTableColumn("Background6"),
                },
                Content = providers.Select(x => new []
                {
                    new PreviewColourDrawable(x.Colour1),
                    new PreviewColourDrawable(x.Colour2),
                    new PreviewColourDrawable(x.Colour3),
                    new PreviewColourDrawable(x.Colour4),
                    new PreviewColourDrawable(x.Highlight1),
                    new PreviewColourDrawable(x.Content1),
                    new PreviewColourDrawable(x.Content2),
                    new PreviewColourDrawable(x.Light1),
                    new PreviewColourDrawable(x.Light2),
                    new PreviewColourDrawable(x.Light3),
                    new PreviewColourDrawable(x.Light4),
                    new PreviewColourDrawable(x.Dark1),
                    new PreviewColourDrawable(x.Dark2),
                    new PreviewColourDrawable(x.Dark3),
                    new PreviewColourDrawable(x.Dark4),
                    new PreviewColourDrawable(x.Dark5),
                    new PreviewColourDrawable(x.Dark6),
                    new PreviewColourDrawable(x.Foreground1),
                    new PreviewColourDrawable(x.Background1),
                    new PreviewColourDrawable(x.Background2),
                    new PreviewColourDrawable(x.Background3),
                    new PreviewColourDrawable(x.Background4),
                    new PreviewColourDrawable(x.Background5),
                    new PreviewColourDrawable(x.Background6),
                }).To2DArray(),
            };
        }

        private class TitleTableColumn : TableColumn
        {
            public TitleTableColumn(string title)
                : base(title, Anchor.Centre)
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
