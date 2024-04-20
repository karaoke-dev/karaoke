// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Carets;

public partial class DrawableRecordingTimeTagCaret : DrawableCaret<RecordingTimeTagCaretPosition>
{
    private const float border_spacing = 5;
    private const float caret_move_time = 60;
    private const float caret_resize_time = 60;

    // should be list of indexse.
    private readonly FillFlowContainer<DrawableTextIndex> drawableTextIndexes;
    private readonly Box indicator;

    public DrawableRecordingTimeTagCaret(DrawableCaretType type)
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
            drawableTextIndexes = new FillFlowContainer<DrawableTextIndex>()
            {
                Y = -10,
                Spacing = new Vector2(5),
                Alpha = GetAlpha(type),
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
            },
            indicator = new Box
            {
                Width = border_spacing,
                RelativeSizeAxes = Axes.Y,
            },
        };
    }

    protected override void ApplyCaretPosition(RecordingTimeTagCaretPosition caret)
    {
        var timeTag = caret.TimeTag;
        var charRange = caret.GetLyricCharRange();
        var rect = RectangleF.Union(
            LyricPositionProvider.GetRectByCharIndex(charRange.Item1),
            LyricPositionProvider.GetRectByCharIndex(charRange.Item2));

        // to able to let user know now many indicator.
        updateDisplayListTextIndex(caret);

        // update the caret.
        changeTheSizeByRect(rect);
        drawableTextIndexes.Anchor = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.TopLeft, Anchor.TopRight);
        drawableTextIndexes.Origin = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.BottomLeft, Anchor.BottomRight);
        indicator.Colour = Colours.GetRecordingTimeTagCaretColour(timeTag);
        indicator.Anchor = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.CentreLeft, Anchor.CentreRight);
        indicator.Origin = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.CentreLeft, Anchor.CentreRight);
    }

    private void updateDisplayListTextIndex(RecordingTimeTagCaretPosition caret)
    {
        int paddingIndicator = caret.GetPaddingTextIndex();
        drawableTextIndexes.Clear();

        for (int i = 0; i <= paddingIndicator; i++)
        {
            bool isFirst = i == 0;
            drawableTextIndexes.Add(new DrawableTextIndex
            {
                Size = new Vector2(isFirst ? 15 : 12),
                State = caret.TimeTag.Index.State,
                Colour = Colours.GetRecordingTimeTagCaretColour(caret.TimeTag),
                Origin = Anchor.BottomLeft,
                Alpha = isFirst ? 1 : 0.4f,
            });
        }
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
