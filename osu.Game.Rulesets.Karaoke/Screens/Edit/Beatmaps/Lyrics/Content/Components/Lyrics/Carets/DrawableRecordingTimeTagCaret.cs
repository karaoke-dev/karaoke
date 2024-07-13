// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;

public partial class DrawableRecordingTimeTagCaret : DrawableCaret<RecordingTimeTagCaretPosition>
{
    private const float border_spacing = 5;
    private const float caret_move_time = 60;
    private const float caret_resize_time = 60;

    // should be list of indexes.
    private readonly TextIndexInfo textIndexInfo;
    private readonly Box indicator;

    public DrawableRecordingTimeTagCaret(DrawableCaretState state)
        : base(state)
    {
        InternalChildren = new Drawable[]
        {
            new Container
            {
                Masking = true,
                BorderThickness = border_spacing,
                BorderColour = Colour4.White,
                RelativeSizeAxes = Axes.Both,
                Alpha = GetAlpha(state),
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.White,
                    Alpha = 0.1f,
                },
            },
            textIndexInfo = new TextIndexInfo
            {
                Y = -10,
                Alpha = GetAlpha(state),
            },
            indicator = new Box
            {
                Width = border_spacing,
                RelativeSizeAxes = Axes.Y,
                Alpha = GetAlpha(state),
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

        // update the caret.
        changeTheSizeByRect(rect);
        textIndexInfo.Anchor = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.TopLeft, Anchor.TopRight);
        textIndexInfo.Origin = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.BottomLeft, Anchor.BottomRight);
        textIndexInfo.X = TextIndexUtils.GetValueByState(timeTag.Index, -10, 10);
        textIndexInfo.UpdateCaret(caret);
        indicator.Colour = Colours.GetRecordingTimeTagCaretColour(timeTag);
        indicator.Anchor = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.CentreLeft, Anchor.CentreRight);
        indicator.Origin = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.CentreLeft, Anchor.CentreRight);
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

    private partial class TextIndexInfo : CompositeDrawable
    {
        private const int border_radius = 5;

        private DrawableTimeTag currentTextTag = null!;
        private FillFlowContainer<DrawableTimeTag> pendingTimeTags = null!;

        [Resolved]
        private OsuColour colours { get; set; } = null!;

        public TextIndexInfo()
        {
            Masking = true;
            CornerRadius = border_radius;

            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background2(state.Mode),
                },
                new Container
                {
                    Name = "Background for pending text indexes",
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding
                    {
                        Horizontal = 5,
                        Vertical = 7,
                    },
                    Child = new Container
                    {
                        Masking = true,
                        CornerRadius = border_radius,
                        RelativeSizeAxes = Axes.Both,
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background3(state.Mode),
                        },
                    },
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Margin = new MarginPadding(5),
                    Direction = FillDirection.Horizontal,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Masking = true,
                            CornerRadius = border_radius,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = colourProvider.Background5(state.Mode),
                                },
                                currentTextTag = new DrawableTimeTag
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Margin = new MarginPadding(5),
                                    Size = new Vector2(15),
                                },
                            },
                        },
                        pendingTimeTags = new FillFlowContainer<DrawableTimeTag>
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(5),
                            Direction = FillDirection.Horizontal,
                        },
                    },
                },
            };
        }

        public void UpdateCaret(RecordingTimeTagCaretPosition caret)
        {
            currentTextTag.TimeTag = caret.TimeTag;

            int paddingIndicator = caret.GetPaddingTextIndex();
            pendingTimeTags.Clear();

            for (int i = 0; i < paddingIndicator; i++)
            {
                bool isFirst = i == 0;
                bool isLast = i == paddingIndicator - 1;

                pendingTimeTags.Add(new DrawableTimeTag
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(12),
                    Margin = new MarginPadding(5)
                    {
                        Left = isFirst ? 5 : 0,
                        Right = isLast ? 5 : 0,
                    },
                    TimeTag = caret.TimeTag,
                    Alpha = 0.5f,
                });
            }
        }
    }
}
