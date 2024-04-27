// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.BottomEditor.RecordingTimeTags;

public partial class RecordingTimeTagPart : TimelinePart
{
    private readonly IBindable<Lyric?> bindableFocusedLyric = new Bindable<Lyric?>();

    public RecordingTimeTagPart()
    {
        RelativeSizeAxes = Axes.Both;
    }

    protected override void LoadBeatmap(EditorBeatmap beatmap)
    {
        base.LoadBeatmap(beatmap);

        bindableFocusedLyric.BindValueChanged(e =>
        {
            Clear();

            var lyric = e.NewValue;
            if (lyric == null)
                return;

            foreach (var timeTag in lyric.TimeTags)
            {
                Add(new RecordingTimeTagVisualization(lyric, timeTag));
            }

            Add(new CurrentRecordingTimeTagVisualization(lyric));
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);
    }

    private partial class CurrentRecordingTimeTagVisualization : CompositeDrawable
    {
        private IBindable<ICaretPosition?> position = null!;

        private readonly Lyric lyric;

        private readonly DrawableTimeTag drawableTimeTag;

        public CurrentRecordingTimeTagVisualization(Lyric lyric)
        {
            this.lyric = lyric;

            Anchor = Anchor.BottomLeft;
            RelativePositionAxes = Axes.X;
            Size = new Vector2(RecordingTimeTagScrollContainer.TIMELINE_HEIGHT / 2);

            InternalChild = drawableTimeTag = new DrawableTimeTag
            {
                Name = "Time tag triangle",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                TimeTagColourFunc = (timeTag, colours) => colours.GetRecordingTimeTagCaretColour(timeTag),
            };
        }

        [BackgroundDependencyLoader]
        private void load(RecordingTimeTagScrollContainer timeline, ILyricCaretState lyricCaretState)
        {
            position = lyricCaretState.BindableCaretPosition.GetBoundCopy();
            position.BindValueChanged(e =>
            {
                if (e.NewValue is not RecordingTimeTagCaretPosition recordingTimeTagCaretPosition)
                    return;

                if (recordingTimeTagCaretPosition.Lyric != lyric)
                {
                    Hide();
                    return;
                }

                var timeTag = recordingTimeTagCaretPosition.TimeTag;
                var textIndex = timeTag.Index;

                Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.BottomLeft, Anchor.BottomRight);
                drawableTimeTag.TimeTag = timeTag;

                if (timeTag.Time.HasValue)
                {
                    Show();
                    this.MoveToX((float)timeline.GetPreviewTime(timeTag), 100, Easing.OutCubic);
                }
                else
                {
                    Hide();
                }
            });
        }
    }

    private partial class RecordingTimeTagVisualization : CompositeDrawable, IHasContextMenu
    {
        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; } = null!;

        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

        private readonly Bindable<double?> bindableTime;

        private readonly Lyric lyric;
        private readonly TimeTag timeTag;

        public RecordingTimeTagVisualization(Lyric lyric, TimeTag timeTag)
        {
            this.lyric = lyric;
            this.timeTag = timeTag;

            var textIndex = timeTag.Index;

            Anchor = Anchor.CentreLeft;
            Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.CentreLeft, Anchor.CentreRight);

            RelativePositionAxes = Axes.X;
            Size = new Vector2(RecordingTimeTagScrollContainer.TIMELINE_HEIGHT);

            bindableTime = timeTag.TimeBindable.GetBoundCopy();
            InternalChildren = new Drawable[]
            {
                new DrawableTimeTag
                {
                    Name = "Time tag triangle",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    TimeTag = timeTag,
                },
                new OsuSpriteText
                {
                    Text = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag),
                    Anchor = TextIndexUtils.GetValueByState(textIndex, Anchor.BottomLeft, Anchor.BottomRight),
                    Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.TopLeft, Anchor.TopRight),
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(RecordingTimeTagScrollContainer timeline)
        {
            bindableTime.BindValueChanged(e =>
            {
                bool hasValue = e.NewValue.HasValue;
                Alpha = hasValue ? 1 : 0;

                if (!hasValue)
                    return;

                // should wait until all time-tag time has been modified.
                Schedule(() =>
                {
                    double previewTime = timeline.GetPreviewTime(timeTag);

                    // adjust position.
                    X = (float)previewTime;
                });
            }, true);
        }

        protected override bool OnClick(ClickEvent e)
        {
            lyricCaretState.MoveCaretToTargetPosition(lyric, timeTag);

            return base.OnClick(e);
        }

        public MenuItem[] ContextMenuItems =>
            new MenuItem[]
            {
                new OsuMenuItem("Clear time", MenuItemType.Destructive, () =>
                {
                    lyricTimeTagsChangeHandler.ClearTimeTagTime(timeTag);
                }),
            };
    }
}
