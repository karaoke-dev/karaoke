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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;

public partial class DrawableCuttingCaret : DrawableCaret<CuttingCaretPosition>
{
    private const float caret_width = 10;

    private readonly Container splitter;
    private readonly SpriteIcon splitIcon;

    public DrawableCuttingCaret(DrawableCaretState state)
        : base(state)
    {
        Width = caret_width;
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
                Alpha = GetAlpha(state),
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

        switch (state)
        {
            case DrawableCaretState.Idle:
                splitIcon.Hide();
                break;

            case DrawableCaretState.Hover:
                splitIcon.Show();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
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

        Position = rect.TopLeft + new Vector2(rect.Width / 2 - caret_width / 2, 0);
        Height = rect.Height;
    }

    protected override void TriggerDisallowEditEffect(OsuColour colour)
    {
        this.FlashColour(colour.Red, 200);
    }
}
