// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;

public partial class DrawableCreateRubyTagCaret : DrawableRangeCaret<CreateRubyTagCaretPosition>, IHasPopover
{
    private const float border_spacing = 5;
    private const float caret_move_time = 60;
    private const float caret_resize_time = 60;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    private readonly SpriteIcon icon;

    public DrawableCreateRubyTagCaret(DrawableCaretState state)
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
            icon = new SpriteIcon
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.BottomLeft,
                Icon = FontAwesome.Solid.PlusCircle,
                Size = new Vector2(15),
                Alpha = GetAlpha(state),
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        icon.Colour = colours.Green;
    }

    private int startCharIndex;
    private int endCharIndex;

    protected override void ApplyCaretPosition(CreateRubyTagCaretPosition caret)
    {
        // should not show the hover caret if already contains the selected range.
        if (State == DrawableCaretState.Hover)
        {
            bool isClickToThisCaret = lyricCaretState.CaretPosition?.Lyric == caret.Lyric;
            bool isDraggingToThisCaret = lyricCaretState.RangeCaretPosition?.IsInRange(caret.Lyric) ?? false;

            if (isClickToThisCaret || isDraggingToThisCaret)
            {
                Hide();
                return;
            }
        }

        startCharIndex = caret.CharIndex;
        endCharIndex = caret.CharIndex;

        var rect = LyricPositionProvider.GetRectByCharIndex(caret.CharIndex);
        changeTheSizeByRect(rect);

        // should not continuous showing the caret position if move the caret by keyboard.
        if (State == DrawableCaretState.Idle)
        {
            // todo: should wait until layer is attached to the parent.
            // use quick way to fix this because it will cause crash if open the
            Schedule(this.HidePopover);
        }
    }

    protected override bool OnClick(ClickEvent e)
    {
        if (State == DrawableCaretState.Hover)
            return false;

        this.ShowPopover();
        return true;
    }

    protected override void ApplyRangeCaretPosition(RangeCaretPosition<CreateRubyTagCaretPosition> caret)
    {
        startCharIndex = caret.GetRangeCaretPosition().Item1.CharIndex;
        endCharIndex = caret.GetRangeCaretPosition().Item2.CharIndex;

        var rect = RectangleF.Union(LyricPositionProvider.GetRectByCharIndex(startCharIndex), LyricPositionProvider.GetRectByCharIndex(endCharIndex));
        changeTheSizeByRect(rect);

        if (State == DrawableCaretState.Idle && caret.DraggingState == RangeCaretDraggingState.EndDrag)
            this.ShowPopover();
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

    public Popover GetPopover() => new CreateRubyPopover(startCharIndex, endCharIndex);

    private partial class CreateRubyPopover : OsuPopover
    {
        [Resolved]
        private ILyricRubyTagsChangeHandler lyricRubyTagsChangeHandler { get; set; } = null!;

        private readonly int startCharIndex;
        private readonly int endCharIndex;

        private readonly LabelledTextBox labelledRubyTextBox;

        public CreateRubyPopover(int startCharIndex, int endCharIndex)
        {
            this.startCharIndex = startCharIndex;
            this.endCharIndex = endCharIndex;

            AllowableAnchors = new[] { Anchor.TopCentre, Anchor.BottomCentre };

            Child = new FillFlowContainer
            {
                Width = 200,
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Y,
                Spacing = new Vector2(0, 10),
                Children = new Drawable[]
                {
                    labelledRubyTextBox = new CreateRubyLabelledTextBox
                    {
                        Label = "Ruby",
                        PlaceholderText = "Ruby text",
                    },
                    new CreateRubyButton
                    {
                        Text = "Create",
                        Action = addRubyText,
                    },
                },
            };

            labelledRubyTextBox.OnCommit += (_, _) =>
            {
                addRubyText();
            };
        }

        private void addRubyText()
        {
            string? rubyText = labelledRubyTextBox.Text;

            if (string.IsNullOrEmpty(rubyText))
            {
                labelledRubyTextBox.Description = "Please enter the ruby text";
                GetContainingFocusManager().ChangeFocus(labelledRubyTextBox);
                return;
            }

            lyricRubyTagsChangeHandler.Add(new RubyTag
            {
                StartIndex = startCharIndex,
                EndIndex = endCharIndex,
                Text = labelledRubyTextBox.Text,
            });

            this.HidePopover();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            ScheduleAfterChildren(() => GetContainingFocusManager().ChangeFocus(labelledRubyTextBox));
        }

        private partial class CreateRubyLabelledTextBox : LabelledTextBox
        {
            protected override OsuTextBox CreateComponent()
            {
                return base.CreateComponent().With(x =>
                {
                    x.CommitOnFocusLost = false;
                });
            }
        }

        private partial class CreateRubyButton : EditorSectionButton
        {
        }
    }
}
