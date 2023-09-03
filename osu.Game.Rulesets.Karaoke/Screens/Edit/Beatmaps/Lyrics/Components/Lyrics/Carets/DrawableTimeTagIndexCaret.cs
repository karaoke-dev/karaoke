// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public partial class DrawableTimeTagIndexCaret : DrawableCaret<TimeTagIndexCaretPosition>
{
    private const float border_spacing = 5;

    public DrawableTimeTagIndexCaret(DrawableCaretType type)
        : base(type)
    {
        InternalChild = new Container
        {
            Masking = true,
            BorderThickness = border_spacing,
            BorderColour = Colour4.White,
            RelativeSizeAxes = Axes.Both,
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White,
                Alpha = 0.1f,
            },
        };
    }

    protected override void ApplyCaretPosition(TimeTagIndexCaretPosition caret)
    {
        var rect = LyricPositionProvider.GetRectByCharIndex(caret.CharIndex);

        Position = rect.TopLeft - new Vector2(border_spacing);
        Size = rect.Size + new Vector2(border_spacing * 2);
    }

    protected override void TriggerDisallowEditEffect(OsuColour colour)
    {
        this.FlashColour(colour.Red, 200);
    }
}
