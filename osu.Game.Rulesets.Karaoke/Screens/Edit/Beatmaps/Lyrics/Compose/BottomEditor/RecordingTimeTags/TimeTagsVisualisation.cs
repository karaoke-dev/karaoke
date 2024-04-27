// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.BottomEditor.RecordingTimeTags;

/// <summary>
/// Display all time-tags in the lyric, and current time-tag.
/// </summary>
public partial class TimeTagsVisualisation : CompositeDrawable
{
    private readonly IBindable<ICaretPosition?> bindableCaret = new Bindable<ICaretPosition?>();

    public TimeTagsVisualisation()
    {
        AutoSizeAxes = Axes.Both;

        FocusedTimeTagArea focusedTimeTagArea;
        PendingTimeTagsArea leftPendingTimeTagsArea;
        PendingTimeTagsArea rightPendingTimeTagsArea;

        InternalChildren = new Drawable[]
        {
            focusedTimeTagArea = new FocusedTimeTagArea
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            },
            leftPendingTimeTagsArea = new PendingTimeTagsArea
            {
                X = -5,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreRight,
            },
            rightPendingTimeTagsArea = new PendingTimeTagsArea
            {
                X = 5,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreLeft,
            },
        };

        bindableCaret.BindValueChanged(x =>
        {
            if (x.NewValue is not RecordingTimeTagCaretPosition newCaret)
                return;

            focusedTimeTagArea.UpdateDisplayTimeTag(newCaret.Lyric, newCaret.TimeTag);
            leftPendingTimeTagsArea.UpdateDisplayTimeTags(newCaret.Lyric, newCaret.GetRecordedTimeTags());
            rightPendingTimeTagsArea.UpdateDisplayTimeTags(newCaret.Lyric, newCaret.GetPendingTimeTags());
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableCaret.BindTo(lyricCaretState.BindableCaretPosition);
    }

    private partial class FocusedTimeTagArea : CompositeDrawable
    {
        private readonly Box background;
        private readonly DrawableTimeTag drawableTimeTag;
        private readonly OsuSpriteText timeTagText;

        public FocusedTimeTagArea()
        {
            Size = new Vector2(30);

            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Child = background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                },
                drawableTimeTag = new DrawableTimeTag
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(20),
                },
                timeTagText = new OsuSpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.TopLeft,
                    X = 5,
                },
            };
        }

        private TimeTag? timeTag;

        public void UpdateDisplayTimeTag(Lyric lyric, TimeTag timeTag)
        {
            this.timeTag = timeTag;

            drawableTimeTag.TimeTag = timeTag;
            timeTagText.Text = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag);
        }
    }

    public partial class PendingTimeTagsArea : CompositeDrawable
    {
        private readonly Box background;

        private readonly FillFlowContainer<PendingTimeTag> drawableTimeTags;

        [Resolved]
        private OsuColour colours { get; set; } = null!;

        public PendingTimeTagsArea()
        {
            AutoSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Child = background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                },
                drawableTimeTags = new FillFlowContainer<PendingTimeTag>
                {
                    Margin = new MarginPadding(5),
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(25),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray4;
        }

        public void UpdateDisplayTimeTags(Lyric lyric, TimeTag[] timeTags)
        {
            drawableTimeTags.Clear();

            Alpha = timeTags.Length > 0 ? 1 : 0;

            foreach (var timeTag in timeTags)
            {
                drawableTimeTags.Add(new PendingTimeTag(lyric, timeTag)
                {
                    Size = new Vector2(12),
                });
            }
        }

        private partial class PendingTimeTag : CompositeDrawable
        {
            public PendingTimeTag(Lyric lyric, TimeTag timeTag)
            {
                InternalChildren = new Drawable[]
                {
                    new DrawableTimeTag
                    {
                        TimeTag = timeTag,
                        Size = new Vector2(12),
                    },
                    new OsuSpriteText
                    {
                        Font = OsuFont.Default.With(size: 12),
                        Text = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag),
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.TopLeft,
                        Y = 10,
                    },
                };
            }
        }
    }
}
