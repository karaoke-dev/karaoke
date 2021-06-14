// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components
{
    [Cached]
    public class SingerLyricEditor : ZoomableScrollContainer
    {
        private const float timeline_height = 38;

        [Resolved]
        private EditorClock editorClock { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public Bindable<float> bindableZoom;
        public Bindable<float> bindableCurrent;

        public readonly Singer Singer;

        public SingerLyricEditor(Singer singer)
        {
            Singer = singer;

            RelativeSizeAxes = Axes.Both;

            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;
        }

        private Box background;

        private Container mainContent;

        [BackgroundDependencyLoader]
        private void load(SingerManager singerManager, OsuColour colour)
        {
            AddInternal(background = new Box
            {
                Name = "Background",
                Depth = 1,
                RelativeSizeAxes = Axes.X,
                Height = timeline_height,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Colour = colour.Gray3,
            });
            AddRange(new Drawable[]
            {
                mainContent = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = timeline_height,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Depth = float.MaxValue,
                    Children = new Drawable[]
                    {
                        new LyricBlueprintContainer(Singer),
                    }
                },
            });

            // initialize scroll zone.
            MaxZoom = getZoomLevelForVisibleMilliseconds(2000);
            MinZoom = getZoomLevelForVisibleMilliseconds(20000);
            Zoom = getZoomLevelForVisibleMilliseconds(5000);

            bindableZoom = singerManager.BindableZoom.GetBoundCopy();
            bindableCurrent = singerManager.BindableCurrent.GetBoundCopy();

            bindableZoom.BindValueChanged(e =>
            {
                if (e.NewValue == Zoom)
                    return;

                Zoom = e.NewValue;
            }, true);

            bindableCurrent.BindValueChanged(e =>
            {
                ScrollTo(e.NewValue);
            }, true);
        }

        private float getZoomLevelForVisibleMilliseconds(double milliseconds) => Math.Max(1, (float)(editorClock.TrackLength / milliseconds));

        protected override bool OnScroll(ScrollEvent e)
        {
            var zoneChanged = base.OnScroll(e);
            if (!zoneChanged)
                return false;

            if (e.AltPressed)
            {
                // todo : this event not working while zooming, because zooming will also call scrollto.
                // bindableCurrent.Value = getCurrentPosition();

                // Update zoom to target, ignore easing value.
                bindableZoom.Value = Zoom;
            }

            return true;

            float getCurrentPosition()
            {
                // params
                var zoomedContent = Content;
                var focusPoint = zoomedContent.ToLocalSpace(e.ScreenSpaceMousePosition).X;
                var contentSize = zoomedContent.DrawWidth;
                var scrollOffset = Current;

                // calculation
                float focusOffset = focusPoint - scrollOffset;
                float expectedWidth = DrawWidth * Zoom;
                float targetOffset = expectedWidth * (focusPoint / contentSize) - focusOffset;

                return targetOffset;
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            const float preempt_time = 1000;

            var firstLyric = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault(x => x.LyricStartTime > 0);
            if (firstLyric == null)
                return;

            var position = getPositionFromTime(firstLyric.LyricStartTime - preempt_time);
            ScrollTo(position, false);
        }

        protected override void OnUserScroll(float value, bool animated = true, double? distanceDecay = null)
        {
            base.OnUserScroll(value, animated, distanceDecay);

            // update current value if user scroll to.
            bindableCurrent.Value = value;
        }

        private float getPositionFromTime(double time)
            => (float)(time / editorClock.TrackLength) * Content.DrawWidth;
    }
}
