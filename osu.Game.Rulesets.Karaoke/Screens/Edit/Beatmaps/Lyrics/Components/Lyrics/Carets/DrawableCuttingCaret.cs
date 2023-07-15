// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public partial class DrawableCuttingCaret : DrawableCaret<CuttingCaretPosition>
{
    private readonly Container splitter;
    private readonly SpriteIcon splitIcon;

    public DrawableCuttingCaret(DrawableCaretType type)
        : base(type)
    {
        Width = 10;
        Origin = Anchor.TopCentre;

        InternalChildren = new Drawable[]
        {
            splitIcon = new SpriteIcon
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.BottomCentre,
                Icon = FontAwesome.Solid.HandScissors,
                X = 7,
                Y = -5,
                Size = new Vector2(10),
            },
            splitter = new Container
            {
                RelativeSizeAxes = Axes.Y,
                AutoSizeAxes = Axes.X,
                Alpha = GetAlpha(type),
                Children = new Drawable[]
                {
                    new Triangle
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.BottomCentre,
                        Scale = new Vector2(1, -1),
                        Size = new Vector2(10, 5),
                    },
                    new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Y,
                        Width = 2,
                        EdgeSmoothness = new Vector2(1, 0),
                    },
                },
            },
        };

        switch (type)
        {
            case DrawableCaretType.HoverCaret:
                splitIcon.Show();
                break;

            case DrawableCaretType.Caret:
                splitIcon.Hide();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        splitter.Colour = colours.Red;
        splitIcon.Colour = colours.Yellow;
    }

    protected override void ApplyCaretPosition(CuttingCaretPosition caret)
    {
        var rect = LyricPositionProvider.GetRectByCharIndicator(caret.CharGap);

        Position = rect.TopLeft;
        Height = rect.Height;
    }

    protected override void TriggerDisallowEditEffect(OsuColour colour)
    {
        this.FlashColour(colour.Red, 200);
    }
}
