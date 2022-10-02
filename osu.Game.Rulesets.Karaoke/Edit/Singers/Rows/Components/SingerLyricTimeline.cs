// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components
{
    [Cached]
    public class SingerLyricTimeline : BindableScrollContainer
    {
        private const float timeline_height = 38;

        [Resolved]
        private EditorClock editorClock { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public readonly Singer Singer;

        public SingerLyricTimeline(Singer singer)
        {
            Singer = singer;

            RelativeSizeAxes = Axes.Both;

            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;
        }

        [BackgroundDependencyLoader]
        private void load(ISingerScreenScrollingInfoProvider scrollingInfoProvider, OsuColour colour)
        {
            AddInternal(new Box
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
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = timeline_height,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Depth = float.MaxValue,
                    Children = new Drawable[]
                    {
                        new LyricBlueprintContainer(),
                    }
                },
            });

            BindableZoom.BindTo(scrollingInfoProvider.BindableZoom);
            BindableCurrent.BindTo(scrollingInfoProvider.BindableCurrent);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            const float preempt_time = 1000;

            var firstLyric = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault(x => x.LyricStartTime > 0);
            if (firstLyric == null)
                return;

            float position = getPositionFromTime(firstLyric.LyricStartTime - preempt_time);
            ScrollTo(position, false);
        }

        private float getPositionFromTime(double time)
            => (float)(time / editorClock.TrackLength) * Content.DrawWidth;
    }
}
