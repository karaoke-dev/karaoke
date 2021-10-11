// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteConfigSection : Section
    {
        protected override string Title => "Edit mode";

        [BackgroundDependencyLoader]
        private void load(IScrollingInfo scrollingInfo)
        {
            if (scrollingInfo.TimeRange is not BindableDouble bindableDouble)
                return;

            Children = new[]
            {
                new LabelledRealTimeSliderBar<double>
                {
                    Label = "Time range",
                    Description = "Change time-range to zoom-in/zoom-out the notes.",
                    Current = bindableDouble
                }
            };
        }
    }
}
