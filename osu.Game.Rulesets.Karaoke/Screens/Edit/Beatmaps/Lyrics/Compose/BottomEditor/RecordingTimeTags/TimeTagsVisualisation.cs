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
        private readonly IBindable<double?> bindableTime = new Bindable<double?>();

        private readonly Box background;
        private readonly DrawableTextIndex drawableTextIndex;
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
                drawableTextIndex = new DrawableTextIndex
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

            drawableTextIndex.State = timeTag.Index.State;
            timeTagText.Text = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag);

            bindableTime.UnbindBindings();
            bindableTime.BindTo(timeTag.TimeBindable);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray6;

            bindableTime.BindValueChanged(x =>
            {
                if (timeTag == null)
                    return;

                drawableTextIndex.Colour = colours.GetTimeTagColour(timeTag);
            });
        }
    }

    public partial class PendingTimeTagsArea : CompositeDrawable
    {
        private readonly Box background;

        private readonly FillFlowContainer drawableTimeTags;

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
                drawableTimeTags = new FillFlowContainer
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
                drawableTimeTags.Add(new TimeTagDisplay(lyric, timeTag));
            }
        }

        private partial class TimeTagDisplay : CompositeDrawable
        {
            private readonly IBindable<double?> bindableTime;

            private readonly TimeTag timeTag;
            private readonly DrawableTextIndex drawableTextIndex;

            public TimeTagDisplay(Lyric lyric, TimeTag timeTag)
            {
                this.timeTag = timeTag;
                bindableTime = timeTag.TimeBindable.GetBoundCopy();

                Width = 12;
                Height = 12;
                InternalChildren = new Drawable[]
                {
                    drawableTextIndex = new DrawableTextIndex
                    {
                        State = timeTag.Index.State,
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

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                bindableTime.BindValueChanged(x =>
                {
                    drawableTextIndex.Colour = colours.GetTimeTagColour(timeTag);
                }, true);
            }
        }
    }
}
