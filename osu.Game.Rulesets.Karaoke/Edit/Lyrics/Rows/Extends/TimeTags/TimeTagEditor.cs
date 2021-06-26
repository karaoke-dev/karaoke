// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags
{
    [Cached(typeof(IPositionSnapProvider))]
    [Cached]
    public class TimeTagEditor : ZoomableScrollContainer, IPositionSnapProvider
    {
        private const float timeline_height = 38;

        public readonly IBindable<TimeTag[]> TimeTagsBindable = new Bindable<TimeTag[]>();

        [Resolved]
        private EditorClock editorClock { get; set; }

        public readonly Lyric HitObject;

        public double StartTime { get; private set; }

        public double EndTime { get; private set; }

        public TimeTagEditor(Lyric lyric)
        {
            HitObject = lyric;

            RelativeSizeAxes = Axes.X;
            Padding = new MarginPadding { Top = 10 };
            Height = timeline_height;

            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;

            TimeTagsBindable.BindArrayChanged(addItems =>
            {
                foreach (var obj in addItems)
                {
                    obj.TimeBindable.BindValueChanged(e =>
                    {
                        updateTimeRange();
                    });
                }
            }, removedItems =>
            {
                foreach (var obj in removedItems)
                {
                    obj.TimeBindable.UnbindEvents();
                }
            });

            TimeTagsBindable.BindTo(lyric.TimeTagsBindable);

            updateTimeRange();
        }

        private void updateTimeRange()
        {
            var fistTimeTag = TimeTagsBindable.Value.FirstOrDefault();
            var lastTimeTag = TimeTagsBindable.Value.LastOrDefault();

            if (fistTimeTag != null && lastTimeTag != null)
            {
                StartTime = GetPreviewTime(fistTimeTag) - 500;
                EndTime = GetPreviewTime(lastTimeTag) + 500;
            }
            else
            {
                StartTime = 0;
                EndTime = 0;
            }
        }

        private Box background;

        private Container mainContent;

        private CurrentTimeMarker currentTimeMarker;

        private TimelineTickDisplay ticks;

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            AddInternal(background = new Box
            {
                Name = "Background",
                Depth = 1,
                RelativeSizeAxes = Axes.X,
                Height = timeline_height,
                Colour = colour.Gray3,
            });
            AddRange(new Drawable[]
            {
                mainContent = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = timeline_height,
                    Depth = float.MaxValue,
                    Children = new Drawable[]
                    {
                        ticks = new TimelineTickDisplay(),
                        new TimeTagEditorBlueprintContainer(HitObject),
                        currentTimeMarker = new CurrentTimeMarker(),
                    }
                },
            });

            // initialize scroll zone.
            MaxZoom = getZoomLevelForVisibleMilliseconds(500);
            MinZoom = getZoomLevelForVisibleMilliseconds(10000);
            Zoom = getZoomLevelForVisibleMilliseconds(3000);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            const float preempt_time = 200;
            var position = getPositionFromTime(HitObject.LyricStartTime - preempt_time);
            ScrollTo(position, false);
        }

        private float getZoomLevelForVisibleMilliseconds(double milliseconds) => Math.Max(1, (float)(editorClock.TrackLength / milliseconds));

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            var position = getPositionFromTime(editorClock.CurrentTime);
            currentTimeMarker.MoveToX(position);
        }

        protected override void OnUserScroll(float value, bool animated = true, double? distanceDecay = null)
        {
            const float preempt_time = 1000;
            var zoomMillionSecond = editorClock.TrackLength / CurrentZoom;
            var position = getTimeFromPosition(new Vector2(value));

            // should prevent dragging or moving is out of time-tag range.
            if (position < StartTime - preempt_time)
                value = getPositionFromTime(StartTime - preempt_time);

            if (position > EndTime - zoomMillionSecond + preempt_time)
                value = getPositionFromTime(EndTime - zoomMillionSecond + preempt_time);

            base.OnUserScroll(value, animated, distanceDecay);
        }

        public double GetPreviewTime(TimeTag timeTag)
        {
            var time = timeTag.Time;

            if (time != null)
                return time.Value;

            var timeTags = HitObject.TimeTags;
            var index = timeTags.IndexOf(timeTag);

            const float preempt_time = 200;
            var previousTimeTagWithTime = timeTags.GetPreviousMatch(timeTag, x => x.Time.HasValue);
            var nextTimeTagWithTime = timeTags.GetNextMatch(timeTag, x => x.Time.HasValue);

            if (previousTimeTagWithTime?.Time != null)
            {
                var diffIndex = timeTags.IndexOf(previousTimeTagWithTime) - index;
                return previousTimeTagWithTime.Time.Value - preempt_time * diffIndex;
            }

            if (nextTimeTagWithTime?.Time != null)
            {
                var diffIndex = timeTags.IndexOf(nextTimeTagWithTime) - index;
                return nextTimeTagWithTime.Time.Value - preempt_time * diffIndex;
            }

            // will goes in here if all time-tag are no time.
            return index * preempt_time;
        }

        public SnapResult SnapScreenSpacePositionToValidPosition(Vector2 screenSpacePosition) =>
            new SnapResult(screenSpacePosition, null);

        public SnapResult SnapScreenSpacePositionToValidTime(Vector2 screenSpacePosition) =>
            new SnapResult(screenSpacePosition, getTimeFromPosition(Content.ToLocalSpace(screenSpacePosition)));

        private double getTimeFromPosition(Vector2 localPosition) =>
            localPosition.X / Content.DrawWidth * editorClock.TrackLength;

        private float getPositionFromTime(double time)
            => (float)(time / editorClock.TrackLength) * Content.DrawWidth;

        public float GetBeatSnapDistanceAt(double referenceTime) => throw new NotImplementedException();

        public float DurationToDistance(double referenceTime, double duration) => throw new NotImplementedException();

        public double DistanceToDuration(double referenceTime, float distance) => throw new NotImplementedException();

        public double GetSnappedDurationFromDistance(double referenceTime, float distance) => throw new NotImplementedException();

        public float GetSnappedDistanceFromDistance(double referenceTime, float distance) => throw new NotImplementedException();
    }
}
