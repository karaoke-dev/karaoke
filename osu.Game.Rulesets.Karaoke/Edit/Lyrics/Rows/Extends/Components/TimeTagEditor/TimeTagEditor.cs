// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.TimeTagEditor
{
    [Cached(typeof(IPositionSnapProvider))]
    [Cached]
    public class TimeTagEditor : ZoomableScrollContainer, IPositionSnapProvider
    {
        private const float timeline_height = 38;

        [Resolved]
        private EditorClock editorClock { get; set; }

        public readonly Lyric HitObject;

        private double startPosition;

        private double endPosition;

        public TimeTagEditor(Lyric lyric)
        {
            HitObject = lyric;
            lyric.TimeTagsBindable.GetBoundCopy().BindValueChanged(e =>
            {
                // todo : change time mignt not call time-tag changed.
                startPosition = e.NewValue?.Where(x => x.Time != null).FirstOrDefault()?.Time ?? 0 - 500;
                endPosition = e.NewValue?.Where(x => x.Time != null).LastOrDefault()?.Time ?? 1000000;
            }, true);

            RelativeSizeAxes = Axes.X;
            Padding = new MarginPadding { Top = 10 };
            Height = timeline_height;

            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;
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
            if (position < startPosition - preempt_time)
                value = getPositionFromTime(startPosition - preempt_time);

            if (position > endPosition - zoomMillionSecond + preempt_time)
                value = getPositionFromTime(endPosition - zoomMillionSecond + preempt_time);

            base.OnUserScroll(value, animated, distanceDecay);
        }

        public SnapResult SnapScreenSpacePositionToValidPosition(Vector2 screenSpacePosition) =>
            new SnapResult(screenSpacePosition, null);

        public SnapResult SnapScreenSpacePositionToValidTime(Vector2 screenSpacePosition) =>
            new SnapResult(screenSpacePosition, getTimeFromPosition(Content.ToLocalSpace(screenSpacePosition)));

        private double getTimeFromPosition(Vector2 localPosition) =>
            (localPosition.X / Content.DrawWidth) * editorClock.TrackLength;

        private float getPositionFromTime(double time)
            => (float)(time / editorClock.TrackLength) * Content.DrawWidth;

        public float GetBeatSnapDistanceAt(double referenceTime) => throw new NotImplementedException();

        public float DurationToDistance(double referenceTime, double duration) => throw new NotImplementedException();

        public double DistanceToDuration(double referenceTime, float distance) => throw new NotImplementedException();

        public double GetSnappedDurationFromDistance(double referenceTime, float distance) => throw new NotImplementedException();

        public float GetSnappedDistanceFromDistance(double referenceTime, float distance) => throw new NotImplementedException();
    }
}
