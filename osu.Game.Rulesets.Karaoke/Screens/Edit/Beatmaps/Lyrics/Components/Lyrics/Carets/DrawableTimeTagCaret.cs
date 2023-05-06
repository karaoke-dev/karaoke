// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public partial class DrawableTimeTagCaret : DrawableCaret<TimeTagCaretPosition>
{
    private const float triangle_width = 8;

    [Resolved]
    private OsuColour colours { get; set; }

    [Resolved]
    private InteractableKaraokeSpriteText karaokeSpriteText { get; set; }

    private readonly DrawableTextIndex drawableTextIndex;

    public DrawableTimeTagCaret(DrawableCaretType type)
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

    protected override void Apply(TimeTagCaretPosition caret)
    {
        var timeTag = caret.TimeTag;
        var textIndex = timeTag.Index;
        this.MoveTo(karaokeSpriteText.GetPositionByTimeTag(timeTag), getMoveToDuration(Type), Easing.OutCubic);
        Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.BottomLeft, Anchor.BottomRight);

        drawableTextIndex.State = textIndex.State;
        drawableTextIndex.Colour = colours.GetRecordingTimeTagCaretColour(timeTag);

        static double getMoveToDuration(DrawableCaretType type) =>
            type switch
            {
                DrawableCaretType.Caret => 100,
                DrawableCaretType.HoverCaret => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }

    public override void TriggerDisallowEditEffect(LyricEditorMode editorMode)
    {
        this.FlashColour(colours.Red, 200);
    }
}
