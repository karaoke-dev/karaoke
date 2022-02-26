// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Containers
{
    public abstract class EditorScrollContainer : ZoomableScrollContainer
    {
        public readonly BindableFloat BindableZoom = new();
        public readonly BindableFloat BindableCurrent = new();

        protected EditorScrollContainer()
        {
            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;

            BindableZoom.MaxValueChanged += (v) =>
            {
                MaxZoom = v;
            };

            BindableZoom.MinValueChanged += (v) =>
            {
                MinZoom = v;
            };

            BindableZoom.BindValueChanged(e =>
            {
                if (e.NewValue == Zoom)
                    return;

                Zoom = e.NewValue;
            }, true);

            BindableCurrent.BindValueChanged(e =>
            {
                ScrollTo(e.NewValue);
            }, true);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            bool zoneChanged = base.OnScroll(e);
            if (!zoneChanged)
                return false;

            if (e.AltPressed)
            {
                // todo : this event not working while zooming, because zooming will also call scroll to.
                // bindableCurrent.Value = getCurrentPosition();

                // Update zoom to target, ignore easing value.
                BindableZoom.Value = Zoom;
            }

            return true;

            /*
            float getCurrentPosition()
            {
                // params
                var zoomedContent = Content;
                var focusPoint = zoomedContent.ToLocalSpace(e.ScreenSpaceMousePosition).X;
                var contentSize = zoomedContent.DrawWidth;
                var scrollOffset = Current;

                // calculation
                float focusOffset = focusPoint - scrollOffset;
                float expectedWidth = DrawWidth * Zoom;
                float targetOffset = expectedWidth * (focusPoint / contentSize) - focusOffset;

                return targetOffset;
            }
            */
        }

        protected override void OnUserScroll(float value, bool animated = true, double? distanceDecay = null)
        {
            base.OnUserScroll(value, animated, distanceDecay);

            // update current value if user scroll to.
            BindableCurrent.Value = value;
        }
    }
}
