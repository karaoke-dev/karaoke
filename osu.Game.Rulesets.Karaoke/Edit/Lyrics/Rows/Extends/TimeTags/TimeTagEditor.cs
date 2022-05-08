// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags
{
    [Cached(typeof(IPositionSnapProvider))]
    [Cached]
    public class TimeTagEditor : TimeTagEditorScrollContainer, IPositionSnapProvider
    {
        private const float timeline_height = 38;

        [Resolved]
        private EditorClock editorClock { get; set; }

        private CurrentTimeMarker currentTimeMarker;

        public TimeTagEditor(Lyric lyric)
            : base(lyric)
        {
            Padding = new MarginPadding { Top = 10 };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ITimeTagModeState timeTagModeState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            BindableZoom.BindTo(timeTagModeState.BindableAdjustZoom);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowWaveform, ShowWaveformGraph);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagWaveformOpacity, WaveformOpacity);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowTick, ShowTick);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagTickOpacity, TickOpacity);

            AddInternal(new Box
            {
                Name = "Background",
                Depth = 1,
                RelativeSizeAxes = Axes.X,
                Height = timeline_height,
                Colour = colours.Gray3,
            });
        }

        protected override void PostProcessContent(Container content)
        {
            content.Height = timeline_height;
            content.AddRange(new Drawable[]
            {
                new TimeTagEditorBlueprintContainer(HitObject),
                currentTimeMarker = new CurrentTimeMarker(),
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            const float preempt_time = 200;
            float position = getPositionFromTime(HitObject.LyricStartTime - preempt_time);
            ScrollTo(position, false);
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            float position = getPositionFromTime(editorClock.CurrentTime);
            currentTimeMarker.MoveToX(position);
        }

        protected override void OnUserScroll(float value, bool animated = true, double? distanceDecay = null)
        {
            const float preempt_time = 1000;
            double zoomMillionSecond = editorClock.TrackLength / CurrentZoom;
            double position = getTimeFromPosition(new Vector2(value));

            // should prevent dragging or moving is out of time-tag range.
            if (position < StartTime - preempt_time)
                value = getPositionFromTime(StartTime - preempt_time);

            if (position > EndTime - zoomMillionSecond + preempt_time)
                value = getPositionFromTime(EndTime - zoomMillionSecond + preempt_time);

            base.OnUserScroll(value, animated, distanceDecay);
        }

        public SnapResult FindSnappedPosition(Vector2 screenSpacePosition) =>
            new(screenSpacePosition, null);

        public SnapResult FindSnappedPositionAndTime(Vector2 screenSpacePosition) =>
            new(screenSpacePosition, getTimeFromPosition(Content.ToLocalSpace(screenSpacePosition)));

        private double getTimeFromPosition(Vector2 localPosition) =>
            localPosition.X / Content.DrawWidth * editorClock.TrackLength;

        private float getPositionFromTime(double time)
            => (float)(time / editorClock.TrackLength) * Content.DrawWidth;

        public float GetBeatSnapDistanceAt(HitObject referenceObject) => throw new NotImplementedException();

        public float DurationToDistance(HitObject referenceObject, double duration) => throw new NotImplementedException();

        public double DistanceToDuration(HitObject referenceObject, float distance) => throw new NotImplementedException();

        public double GetSnappedDurationFromDistance(HitObject referenceObject, float distance) => throw new NotImplementedException();

        public float GetSnappedDistanceFromDistance(HitObject referenceObject, float distance) => throw new NotImplementedException();
    }
}
