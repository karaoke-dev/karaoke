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
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays;

[TestFixture]
public partial class TestSceneOverlayColourProvider : OsuTestScene
{
    [Test]
    public void TestShowWithNoFetch()
    {
        var providers = Enum.GetValues<OverlayColourScheme>()
                            .Select(x => new OverlayColourProvider(x));

        var colourProperties = typeof(OverlayColourProvider)
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(x => x.PropertyType == typeof(Color4)).ToArray();

        Schedule(() =>
        {
            var columns = colourProperties.Select(c => new TitleTableColumn(c.Name)).OfType<TableColumn>().ToArray();
            var content = providers.Select(provider =>
            {
                if (provider == null)
                    throw new ArgumentNullException(nameof(provider));

                return colourProperties.Select(c =>
                {
                    object? value = c.GetValue(provider);
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
                },
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
        private Clipboard clipboard { get; set; } = null!;

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
                },
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            clipboard.SetText(color.ToHex());
            return base.OnClick(e);
        }
    }
}
