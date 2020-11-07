// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Screens.Edit.Compose.Components.Timeline;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.Timeline
{
    public class SingerTimeline : Screens.Edit.Compose.Components.Timeline.Timeline
    {
        [Resolved(CanBeNull = true)]
        private SingerManager singerManager { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            MaxZoom = 50;
            Zoom = 20;
            MinZoom = 10;

            var centerRemarks = InternalChildren.OfType<CentreMarker>().ToList();
            foreach(var centerRemark in centerRemarks)
                RemoveInternal(centerRemark);

            singerManager?.BindableZoom.BindValueChanged(e =>
            {
                if (e.NewValue == Zoom)
                    return;

                Zoom = e.NewValue;
            });
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            var zoneChanged =  base.OnScroll(e);
            if (!zoneChanged)
                return false;

            // Update bindable to trigger zone changed.
            if(singerManager != null)
                singerManager.BindableZoom.Value = Zoom;

            return zoneChanged;
        }
    }
}
