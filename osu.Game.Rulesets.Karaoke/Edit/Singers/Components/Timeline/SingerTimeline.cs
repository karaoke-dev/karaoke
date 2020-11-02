// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.


using osu.Framework.Allocation;
using osu.Game.Screens.Edit.Compose.Components.Timeline;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.Timeline
{
    public class SingerTimeline : Screens.Edit.Compose.Components.Timeline.Timeline
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            MaxZoom = 50;
            Zoom = 20;
            MinZoom = 10;

            var centerRemarks = InternalChildren.OfType<CentreMarker>().ToList();
            foreach(var centerRemark in centerRemarks)
                RemoveInternal(centerRemark);
        }
    }
}
