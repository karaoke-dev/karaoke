// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components
{
    public abstract class TimeTagZoomableScrollContainer : ZoomableScrollContainer
    {
        protected readonly IBindable<TimeTag[]> TimeTagsBindable = new Bindable<TimeTag[]>();

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
    }
}
