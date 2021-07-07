// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.RecordingTimeTags
{
    public class RecordingTimeTagEditor : TimeTagZoomableScrollContainer
    {
        private const float timeline_height = 28;

        public RecordingTimeTagEditor(Lyric lyric)
            : base(lyric)
        {
            Padding = new MarginPadding { Top = 10 };
            Height = timeline_height;
        }

        private Container mainContent;

        private CurrentTimeMarker currentTimeMarker;

        private TimelineTickDisplay ticks;

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            AddInternal(new Box
            {
                Name = "Background",
                Depth = 1,
                RelativeSizeAxes = Axes.X,
                Height = timeline_height,
                Colour = colour.Gray3,
            });
            AddRange(new Drawable[]
            {
                currentTimeMarker = new CurrentTimeMarker(),
                mainContent = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = timeline_height,
                    Depth = float.MaxValue,
                    Children = new Drawable[]
                    {
                        ticks = new TimelineTickDisplay(),
                    }
                },
            });
        }
    }
}
