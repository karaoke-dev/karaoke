// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.AdjustTimeTags
{
    [Cached]
    public class AdjustTimeTagScrollContainer : TimeTagScrollContainer, IPositionSnapProvider
    {
        private const float timeline_height = 38;

        [Resolved]
        private EditorClock editorClock { get; set; }

        private CurrentTimeMarker currentTimeMarker;

        public AdjustTimeTagScrollContainer()
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
                new AdjustTimeTagBlueprintContainer(),
                currentTimeMarker = new CurrentTimeMarker(),
            });
        }

        protected override void OnLyricChanged(Lyric newLyric)
        {
            // add the little bit delay to make sure that content width is not zero.
            this.FadeOut(1).OnComplete(x =>
            {
                const float preempt_time = 200;
                float position = getPositionFromTime(newLyric.LyricStartTime - preempt_time);
                ScrollTo(position, false);

                this.FadeIn(100);
            });
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            float position = getPositionFromTime(editorClock.CurrentTime);
            currentTimeMarker.MoveToX(position);
        }

        public SnapResult FindSnappedPosition(Vector2 screenSpacePosition) =>
            new(screenSpacePosition, null);

        public SnapResult FindSnappedPositionAndTime(Vector2 screenSpacePosition, SnapType snapType = SnapType.All) =>
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
