// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components
{
    public abstract class TimeTagEditorScrollContainer : EditorScrollContainer
    {
        private readonly IBindableList<TimeTag> timeTagsBindable = new BindableList<TimeTag>();
        private readonly IBindable<WorkingBeatmap> beatmap = new Bindable<WorkingBeatmap>();

        protected readonly IBindable<bool> ShowWaveformGraph = new BindableBool();
        protected readonly IBindable<float> WaveformOpacity = new BindableFloat();
        protected readonly IBindable<bool> ShowTick = new BindableBool();
        protected readonly IBindable<float> TickOpacity = new BindableFloat();

        protected double StartTime { get; private set; }

        protected double EndTime { get; private set; }

        protected Track Track { get; private set; }

        public readonly Lyric HitObject;

        protected TimeTagEditorScrollContainer(Lyric lyric)
        {
            HitObject = lyric;
            RelativeSizeAxes = Axes.X;

            timeTagsBindable.BindCollectionChanged((_, args) =>
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

            timeTagsBindable.BindTo(lyric.TimeTagsBindable);

            updateTimeRange();
        }

        private void updateTimeRange()
        {
            var fistTimeTag = timeTagsBindable.FirstOrDefault();
            var lastTimeTag = timeTagsBindable.LastOrDefault();

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

        private WaveformGraph waveform;

        private TimelineTickDisplay ticks;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, IBindable<WorkingBeatmap> beatmap)
        {
            this.beatmap.BindTo(beatmap);

            Container container;

            Add(container = new Container
            {
                RelativeSizeAxes = Axes.X,
                Depth = float.MaxValue,
                Children = new Drawable[]
                {
                    waveform = new WaveformGraph
                    {
                        RelativeSizeAxes = Axes.Both,
                        BaseColour = colours.Blue.Opacity(0.2f),
                        LowColour = colours.BlueLighter,
                        MidColour = colours.BlueDark,
                        HighColour = colours.BlueDarker,
                    },
                    ticks = new TimelineTickDisplay(),
                }
            });

            PostProcessContent(container);

            this.beatmap.BindValueChanged(b =>
            {
                waveform.Waveform = b.NewValue.Waveform;
                Track = b.NewValue.Track;
            }, true);

            ShowWaveformGraph.BindValueChanged(e => updateWaveformOpacity());
            WaveformOpacity.BindValueChanged(e => updateWaveformOpacity());
            ShowTick.BindValueChanged(e => updateTickOpacity());
            TickOpacity.BindValueChanged(e => updateTickOpacity());
        }

        private void updateWaveformOpacity() =>
            waveform.FadeTo(ShowWaveformGraph.Value ? WaveformOpacity.Value : 0, 200, Easing.OutQuint);

        private void updateTickOpacity() =>
            ticks.FadeTo(ShowTick.Value ? TickOpacity.Value : 0, 200, Easing.OutQuint);

        protected abstract void PostProcessContent(Container content);

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
