// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.SingerLyricEditor
{
    [Cached]
    public class SingerLyricEditor : ZoomableScrollContainer
    {
        private const float timeline_height = 38;

        [Resolved(CanBeNull = true)]
        private SingerManager singerManager { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

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
        private void load(OsuColour colour)
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
            MaxZoom = getZoomLevelForVisibleMilliseconds(5000);
            MinZoom = getZoomLevelForVisibleMilliseconds(100000);
            Zoom = getZoomLevelForVisibleMilliseconds(20000);

            // todo : might need better way to sync the zoom.
            singerManager?.BindableZoom.BindValueChanged(e =>
            {
                if (e.NewValue == Zoom)
                    return;

                Zoom = e.NewValue;
            });
        }

        private float getZoomLevelForVisibleMilliseconds(double milliseconds) => Math.Max(1, (float)(editorClock.TrackLength / milliseconds));

        protected override bool OnScroll(ScrollEvent e)
        {
            var zoneChanged = base.OnScroll(e);
            if (!zoneChanged)
                return false;

            // Update bindable to trigger zone changed.
            if (singerManager != null)
                singerManager.BindableZoom.Value = Zoom;

            return true;
        }
    }
}
