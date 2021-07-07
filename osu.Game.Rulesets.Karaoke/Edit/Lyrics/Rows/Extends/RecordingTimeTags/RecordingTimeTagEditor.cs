// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.RecordingTimeTags
{
    public class RecordingTimeTagEditor : TimeTagZoomableScrollContainer
    {
        private const float timeline_height = 20;

        [Resolved]
        private EditorClock editorClock { get; set; }

        /// <summary>
        /// The timeline's scroll position in the last frame.
        /// </summary>
        private float lastScrollPosition;

        /// <summary>
        /// The track time in the last frame.
        /// </summary>
        private double lastTrackTime;

        /// <summary>
        /// Whether the user is currently dragging the timeline.
        /// </summary>
        private bool handlingDragInput;

        /// <summary>
        /// Whether the track was playing before a user drag event.
        /// </summary>
        private bool trackWasPlaying;

        private Track track;

        public RecordingTimeTagEditor(Lyric lyric)
            : base(lyric)
        {
            Height = timeline_height;
        }

        private CurrentTimeMarker currentTimeMarker;

        private OsuSpriteText trackTimer;

        private Container mainContent;

        private WaveformGraph waveform;

        private TimelineTickDisplay ticks;

        private Container userContent;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    Depth = 1,
                    Y = 10,
                    RelativeSizeAxes = Axes.X,
                    Height = timeline_height,
                    Colour = colours.Gray3,
                },
                currentTimeMarker = new CurrentTimeMarker
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                trackTimer = new OsuSpriteText
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomLeft,
                    Colour = colours.Red,
                    X = 5,
                    Font = OsuFont.GetFont(size: 16, fixedWidth: true),
                }
            });
            AddRange(new Drawable[]
            {
                mainContent = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = timeline_height,
                    Y = 10,
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
                        userContent = new Container(),
                    }
                },
            });

            Beatmap.BindValueChanged(b =>
            {
                waveform.Waveform = b.NewValue.Waveform;
                track = b.NewValue.Track;
            }, true);
        }

        protected override void Update()
        {
            base.Update();

            trackTimer.Text = editorClock.CurrentTime.ToEditorFormattedString();

            // This needs to happen after transforms are updated, but before the scroll position is updated in base.UpdateAfterChildren
            if (editorClock.IsRunning)
                scrollToTrackTime();
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            if (handlingDragInput)
                seekTrackToCurrent();
            else if (!editorClock.IsRunning)
            {
                // The track isn't running. There are three cases we have to be wary of:
                // 1) The user flick-drags on this timeline and we are applying an interpolated seek on the clock, until interrupted by 2 or 3.
                // 2) The user changes the track time through some other means (scrolling in the editor or overview timeline; clicking a hitobject etc.). We want the timeline to track the clock's time.
                // 3) An ongoing seek transform is running from an external seek. We want the timeline to track the clock's time.

                // The simplest way to cover the first two cases is by checking whether the scroll position has changed and the audio hasn't been changed externally
                // Checking IsSeeking covers the third case, where the transform may not have been applied yet.
                if (Current != lastScrollPosition && editorClock.CurrentTime == lastTrackTime && !editorClock.IsSeeking)
                    seekTrackToCurrent();
                else
                    scrollToTrackTime();
            }

            lastScrollPosition = Current;
            lastTrackTime = editorClock.CurrentTime;
        }

        private void seekTrackToCurrent()
        {
            if (!track.IsLoaded)
                return;

            double target = Current / Content.DrawWidth * track.Length;
            editorClock.Seek(Math.Min(track.Length, target));
        }

        private void scrollToTrackTime()
        {
            if (!track.IsLoaded || track.Length == 0)
                return;

            // covers the case where the user starts playback after a drag is in progress.
            // we want to ensure the clock is always stopped during drags to avoid weird audio playback.
            if (handlingDragInput)
                editorClock.Stop();

            ScrollTo((float)(editorClock.CurrentTime / editorClock.TrackLength) * Content.DrawWidth, false);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (base.OnMouseDown(e))
            {
                beginUserDrag();
                return true;
            }

            return false;
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            endUserDrag();
            base.OnMouseUp(e);
        }

        private void beginUserDrag()
        {
            handlingDragInput = true;
            trackWasPlaying = editorClock.IsRunning;
            editorClock.Stop();
        }

        private void endUserDrag()
        {
            handlingDragInput = false;
            if (trackWasPlaying)
                editorClock.Start();
        }
    }
}
