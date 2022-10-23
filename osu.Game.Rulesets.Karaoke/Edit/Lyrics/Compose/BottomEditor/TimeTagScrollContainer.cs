// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor
{
    public abstract class TimeTagScrollContainer : BindableScrollContainer
    {
        private readonly IBindable<Lyric?> bindableFocusedLyric = new Bindable<Lyric?>();

        private readonly IBindable<int> timeTagsVersion = new Bindable<int>();

        [Cached]
        private readonly BindableList<TimeTag> timeTagsBindable = new();

        private readonly IBindable<WorkingBeatmap> beatmap = new Bindable<WorkingBeatmap>();

        protected readonly IBindable<bool> ShowWaveformGraph = new BindableBool();
        protected readonly IBindable<float> WaveformOpacity = new BindableFloat();
        protected readonly IBindable<bool> ShowTick = new BindableBool();
        protected readonly IBindable<float> TickOpacity = new BindableFloat();

        [Resolved, AllowNull]
        private EditorClock editorClock { get; set; }

        protected TimeTagScrollContainer()
        {
            RelativeSizeAxes = Axes.X;

            timeTagsVersion.BindValueChanged(_ => updateTimeRange());
            timeTagsBindable.BindCollectionChanged((_, _) => updateTimeRange());

            bindableFocusedLyric.BindValueChanged(e =>
            {
                timeTagsVersion.UnbindBindings();
                timeTagsBindable.UnbindBindings();

                var lyric = e.NewValue;
                if (lyric == null)
                    return;

                timeTagsVersion.BindTo(lyric.TimeTagsVersion);
                timeTagsBindable.BindTo(lyric.TimeTagsBindable);

                Schedule(() =>
                {
                    OnLyricChanged(lyric);
                });
            });

            updateTimeRange();
        }

        private void updateTimeRange()
        {
            var fistTimeTag = timeTagsBindable.FirstOrDefault();
            var lastTimeTag = timeTagsBindable.LastOrDefault();

            double startTime = fistTimeTag != null ? GetPreviewTime(fistTimeTag) : 0;
            double endTime = lastTimeTag != null ? GetPreviewTime(lastTimeTag) : 0;

            OnTimeRangeChanged(startTime, endTime);
        }

        protected abstract void OnLyricChanged(Lyric newLyric);

        protected virtual void OnTimeRangeChanged(double startTime, double endTime) { }

        private WaveformGraph waveform = null!;

        private TimelineTickDisplay ticks = null!;

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState, OsuColour colours, IBindable<WorkingBeatmap> beatmap)
        {
            bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);

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

            var timeTags = timeTagsBindable.ToArray();
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

        public double TimeAtPosition(float x)
        {
            return x / Content.DrawWidth * editorClock.TrackLength;
        }

        public float PositionAtTime(double time)
        {
            return (float)(time / editorClock.TrackLength * Content.DrawWidth);
        }
    }
}
