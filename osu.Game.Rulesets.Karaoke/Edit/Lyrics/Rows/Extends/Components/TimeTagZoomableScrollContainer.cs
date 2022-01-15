// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components
{
    public abstract class TimeTagZoomableScrollContainer : ZoomableScrollContainer
    {
        protected readonly IBindableList<TimeTag> TimeTagsBindable = new BindableList<TimeTag>();

        protected readonly IBindable<WorkingBeatmap> Beatmap = new Bindable<WorkingBeatmap>();

        [Resolved]
        private EditorClock editorClock { get; set; }

        public readonly Lyric HitObject;

        protected double StartTime { get; private set; }

        protected double EndTime { get; private set; }

        protected TimeTagZoomableScrollContainer(Lyric lyric)
        {
            HitObject = lyric;

            RelativeSizeAxes = Axes.X;

            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;

            TimeTagsBindable.BindCollectionChanged((_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var obj in args.NewItems.OfType<TimeTag>())
                        {
                            obj.TimeBindable.BindValueChanged(_ =>
                            {
                                updateTimeRange();
                            });
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (var obj in args.OldItems.OfType<TimeTag>())
                        {
                            obj.TimeBindable.UnbindEvents();
                        }

                        break;
                }
            });

            TimeTagsBindable.BindTo(lyric.TimeTagsBindable);

            updateTimeRange();
        }

        private void updateTimeRange()
        {
            var fistTimeTag = TimeTagsBindable.FirstOrDefault();
            var lastTimeTag = TimeTagsBindable.LastOrDefault();

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

        [BackgroundDependencyLoader]
        private void load(IBindable<WorkingBeatmap> beatmap)
        {
            Beatmap.BindTo(beatmap);

            // initialize scroll zone.
            MaxZoom = getZoomLevelForVisibleMilliseconds(500);
            MinZoom = getZoomLevelForVisibleMilliseconds(10000);
            Zoom = getZoomLevelForVisibleMilliseconds(3000);
        }

        private float getZoomLevelForVisibleMilliseconds(double milliseconds) => Math.Max(1, (float)(editorClock.TrackLength / milliseconds));

        public double GetPreviewTime(TimeTag timeTag)
        {
            double? time = timeTag.Time;

            if (time != null)
                return time.Value;

            var timeTags = HitObject.TimeTags;
            int index = timeTags.IndexOf(timeTag);

            const float preempt_time = 200;
            var previousTimeTagWithTime = timeTags.GetPreviousMatch(timeTag, x => x.Time.HasValue);
            var nextTimeTagWithTime = timeTags.GetNextMatch(timeTag, x => x.Time.HasValue);

            if (previousTimeTagWithTime?.Time != null)
            {
                int diffIndex = timeTags.IndexOf(previousTimeTagWithTime) - index;
                return previousTimeTagWithTime.Time.Value - preempt_time * diffIndex;
            }

            if (nextTimeTagWithTime?.Time != null)
            {
                int diffIndex = timeTags.IndexOf(nextTimeTagWithTime) - index;
                return nextTimeTagWithTime.Time.Value - preempt_time * diffIndex;
            }

            // will goes in here if all time-tag are no time.
            return index * preempt_time;
        }
    }
}
