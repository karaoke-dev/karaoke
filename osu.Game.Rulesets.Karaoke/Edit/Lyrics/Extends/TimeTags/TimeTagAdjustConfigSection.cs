// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagAdjustConfigSection : Section
    {
        protected override string Title => "Config";

        [BackgroundDependencyLoader]
        private void load(ITimeTagModeState timeTagModeState)
        {
            Children = new Drawable[]
            {
                new LabelledRealTimeSliderBar<float>
                {
                    Label = "Time range",
                    Description = "Change time-range to zoom-in/zoom-out the adjust area.",
                    Current = timeTagModeState.BindableAdjustZoom
                }
            };
        }
    }
}
