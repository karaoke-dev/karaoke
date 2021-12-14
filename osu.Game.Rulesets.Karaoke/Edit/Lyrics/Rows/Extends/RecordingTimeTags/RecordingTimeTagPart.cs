// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.RecordingTimeTags
{
    public class RecordingTimeTagPart : TimelinePart
    {
        private readonly Lyric lyric;

        public RecordingTimeTagPart(Lyric lyric)
        {
            this.lyric = lyric;

            RelativeSizeAxes = Axes.Both;
        }

        protected override void LoadBeatmap(EditorBeatmap beatmap)
        {
            base.LoadBeatmap(beatmap);

            if (lyric.TimeTags == null)
                return;

            foreach (var timeTag in lyric.TimeTags)
            {
                Add(new RecordingTimeTagVisualization(lyric, timeTag));
            }

            Add(new CurrentRecordingTimeTagVisualization(lyric));
        }

        private class CurrentRecordingTimeTagVisualization : CompositeDrawable
        {
            private IBindable<ICaretPosition> position;

            private readonly Lyric lyric;

            public CurrentRecordingTimeTagVisualization(Lyric lyric)
            {
                this.lyric = lyric;

                Anchor = Anchor.BottomLeft;
                RelativePositionAxes = Axes.X;
                Size = new Vector2(RecordingTimeTagEditor.TIMELINE_HEIGHT / 2);

                InternalChild = new RightTriangle
                {
                    Name = "Time tag triangle",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours, RecordingTimeTagEditor timeline, ILyricCaretState lyricCaretState)
            {
                position = lyricCaretState.BindableCaretPosition.GetBoundCopy();
                position.BindValueChanged(e =>
                {
                    if (e.NewValue is not TimeTagCaretPosition timeTagCaretPosition)
                        return;

                    if (timeTagCaretPosition.Lyric != lyric)
                    {
                        Hide();
                        return;
                    }

                    var timeTag = timeTagCaretPosition.TimeTag;
                    bool start = timeTag.Index.State == TextIndex.IndexState.Start;

                    Origin = start ? Anchor.BottomLeft : Anchor.BottomRight;
                    InternalChild.Colour = colours.GetRecordingTimeTagCaretColour(timeTag);
                    InternalChild.Scale = new Vector2(start ? 1 : -1, 1);

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

        private class RecordingTimeTagVisualization : CompositeDrawable, IHasCustomTooltip<TimeTag>, IHasContextMenu
        {
            [Resolved]
            private EditorClock editorClock { get; set; }

            [Resolved]
            private ILyricCaretState lyricCaretState { get; set; }

            [Resolved]
            private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

            private readonly Bindable<double?> bindableTIme;

            private readonly RightTriangle timeTagTriangle;

            private readonly Lyric lyric;
            private readonly TimeTag timeTag;

            public RecordingTimeTagVisualization(Lyric lyric, TimeTag timeTag)
            {
                this.lyric = lyric;
                this.timeTag = timeTag;
                bool start = timeTag.Index.State == TextIndex.IndexState.Start;

                Anchor = Anchor.CentreLeft;
                Origin = start ? Anchor.CentreLeft : Anchor.CentreRight;
                RelativePositionAxes = Axes.X;
                Size = new Vector2(RecordingTimeTagEditor.TIMELINE_HEIGHT);

                bindableTIme = timeTag.TimeBindable.GetBoundCopy();
                InternalChildren = new Drawable[]
                {
                    timeTagTriangle = new RightTriangle
                    {
                        Name = "Time tag triangle",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(start ? 1 : -1, 1)
                    },
                    new OsuSpriteText
                    {
                        Text = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag),
                        Anchor = start ? Anchor.BottomLeft : Anchor.BottomRight,
                        Origin = start ? Anchor.TopLeft : Anchor.TopRight,
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours, RecordingTimeTagEditor timeline)
            {
                timeTagTriangle.Colour = colours.GetTimeTagColour(timeTag);

                bindableTIme.BindValueChanged(e =>
                {
                    bool hasValue = e.NewValue.HasValue;
                    Alpha = hasValue ? 1 : 0;

                    if (!hasValue)
                        return;

                    X = (float)timeline.GetPreviewTime(timeTag);
                }, true);
            }

            protected override bool OnClick(ClickEvent e)
            {
                // navigation to target time
                double? time = timeTag.Time;
                if (time != null)
                    editorClock.SeekSmoothlyTo(time.Value);

                lyricCaretState.MoveCaretToTargetPosition(new TimeTagCaretPosition(lyric, timeTag));

                return base.OnClick(e);
            }

            public ITooltip<TimeTag> GetCustomTooltip() => new TimeTagTooltip();

            public TimeTag TooltipContent => timeTag;

            public MenuItem[] ContextMenuItems =>
                new MenuItem[]
                {
                    new OsuMenuItem("Clear time", MenuItemType.Destructive, () =>
                    {
                        lyricTimeTagsChangeHandler.ClearTimeTagTime(timeTag);
                    })
                };
        }
    }
}
