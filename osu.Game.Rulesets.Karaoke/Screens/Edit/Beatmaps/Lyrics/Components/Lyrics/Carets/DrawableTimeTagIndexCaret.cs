// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public partial class DrawableTimeTagIndexCaret : DrawableCaret<TimeTagIndexCaretPosition>
{
    private const float triangle_width = 8;

    private readonly DrawableTextIndex drawableTextIndex;

    public DrawableTimeTagIndexCaret(DrawableCaretType type)
        : base(type)
    {
        AutoSizeAxes = Axes.Both;

        InternalChild = drawableTextIndex = new DrawableTextIndex
        {
            Name = "Text index",
            Size = new Vector2(triangle_width),
            Alpha = GetAlpha(type),
        };
    }

    protected override void ApplyCaretPosition(IPreviewLyricPositionProvider positionProvider, OsuColour colour, TimeTagIndexCaretPosition caret)
    {
        var textIndex = caret.Index;

        Position = positionProvider.GetTextIndexPosition(textIndex);
        Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.BottomLeft, Anchor.BottomRight);

        drawableTextIndex.State = textIndex.State;
        drawableTextIndex.Colour = colour.GetEditTimeTagCaretColour();
    }

    protected override void TriggerDisallowEditEffect(OsuColour colour)
    {
        this.FlashColour(colour.Red, 200);
    }
}
