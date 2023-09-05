// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public partial class DrawableCreateRubyTagCaret : DrawableRangeCaret<CreateRubyTagCaretPosition>
{
    private const float border_spacing = 5;
    private const float caret_move_time = 60;
    private const float caret_resize_time = 60;

    [Resolved]
    private ILyricRubyTagsChangeHandler lyricRubyTagsChangeHandler { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    private readonly IconButton icon;

    public DrawableCreateRubyTagCaret(DrawableCaretType type)
        : base(type)
    {
        InternalChildren = new Drawable[]
        {
            new Container
            {
                Masking = true,
                BorderThickness = border_spacing,
                BorderColour = Colour4.White,
                RelativeSizeAxes = Axes.Both,
                Alpha = GetAlpha(type),
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.White,
                    Alpha = 0.1f,
                },
            },
            icon = new IconButton
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.BottomLeft,
                Icon = FontAwesome.Solid.PlusCircle,
                Size = new Vector2(15),
                Alpha = GetAlpha(type),
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        icon.IconColour = colours.Green;
        icon.IconHoverColour = colours.GreenLight;
    }

    protected override void ApplyCaretPosition(CreateRubyTagCaretPosition caret)
    {
        // should not show the hover caret if already contains the selected range.
        if (Type == DrawableCaretType.HoverCaret && lyricCaretState.CaretPosition?.Lyric == caret.Lyric)
        {
            Hide();
            return;
        }

        var rect = LyricPositionProvider.GetRectByCharIndex(caret.CharIndex);
        changeTheSizeByRect(rect);

        icon.Action = () =>
        {
            lyricRubyTagsChangeHandler.Add(new RubyTag
            {
                StartIndex = caret.CharIndex,
                EndIndex = caret.CharIndex,
                Text = "Ruby",
            });
        };
    }

    protected override void ApplyRangeCaretPosition(RangeCaretPosition<CreateRubyTagCaretPosition> caret)
    {
        int minIndex = caret.GetRangeCaretPosition().Item1.CharIndex;
        int maxIndex = caret.GetRangeCaretPosition().Item2.CharIndex;

        Logger.Log($"{minIndex}, {maxIndex}");

        var rect = RectangleF.Union(LyricPositionProvider.GetRectByCharIndex(minIndex), LyricPositionProvider.GetRectByCharIndex(maxIndex));
        changeTheSizeByRect(rect);

        icon.Action = () =>
        {
            lyricRubyTagsChangeHandler.Add(new RubyTag
            {
                StartIndex = minIndex,
                EndIndex = maxIndex,
                Text = "Ruby",
            });
        };
    }

    private void changeTheSizeByRect(RectangleF rect)
    {
        var position = rect.TopLeft - new Vector2(border_spacing);
        float width = rect.Width + border_spacing * 2;

        this.MoveTo(position, caret_move_time, Easing.Out);
        this.ResizeWidthTo(width, caret_resize_time, Easing.Out);
        Height = rect.Height + border_spacing * 2;
    }

    protected override void TriggerDisallowEditEffect(OsuColour colour)
    {
        this.FlashColour(colour.Red, 200);
    }
}
