// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.RecordingTimeTags
{
    public class RecordingTimeTagPart : TimelinePart
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

        private class CurrentRecordingTimeTagVisualization : CompositeDrawable
        {
            private IBindable<ICaretPosition?> position = null!;

            private readonly Lyric lyric;

            private readonly DrawableTextIndex drawableTextIndex;

            public CurrentRecordingTimeTagVisualization(Lyric lyric)
            {
                this.lyric = lyric;

                Anchor = Anchor.BottomLeft;
                RelativePositionAxes = Axes.X;
                Size = new Vector2(RecordingTimeTagScrollContainer.TIMELINE_HEIGHT / 2);

                InternalChild = drawableTextIndex = new DrawableTextIndex
                {
                    Name = "Time tag triangle",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours, RecordingTimeTagScrollContainer timeline, ILyricCaretState lyricCaretState)
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
                    var textIndex = timeTag.Index;
                    var state = timeTag.Index.State;

                    Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.BottomLeft, Anchor.BottomRight);
                    drawableTextIndex.Colour = colours.GetRecordingTimeTagCaretColour(timeTag);
                    drawableTextIndex.State = state;

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
            [Resolved, AllowNull]
            private ILyricCaretState lyricCaretState { get; set; }

            [Resolved, AllowNull]
            private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

            private readonly Bindable<double?> bindableTime;

            private readonly TextIndexPiece textIndexPiece;

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
                    textIndexPiece = new TextIndexPiece
                    {
                        Name = "Time tag triangle",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        State = textIndex.State
                    },
                    new OsuSpriteText
                    {
                        Text = LyricUtils.GetTimeTagDisplayRubyText(lyric, timeTag),
                        Anchor = TextIndexUtils.GetValueByState(textIndex, Anchor.BottomLeft, Anchor.BottomRight),
                        Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.TopLeft, Anchor.TopRight),
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(EditorClock clock, OsuColour colours, RecordingTimeTagScrollContainer timeline)
            {
                textIndexPiece.Clock = clock;
                textIndexPiece.Colour = colours.GetTimeTagColour(timeTag);

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

                        // make tickle effect.
                        textIndexPiece.ClearTransforms();

                        using (textIndexPiece.BeginAbsoluteSequence(previewTime))
                        {
                            textIndexPiece.Colour = colours.GetTimeTagColour(timeTag);
                            textIndexPiece.FlashColour(colours.RedDark, 750, Easing.OutQuint);
                        }
                    });
                }, true);
            }

            protected override bool OnClick(ClickEvent e)
            {
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

        private class TextIndexPiece : DrawableTextIndex
        {
            public override bool RemoveCompletedTransforms => false;
        }
    }
}
