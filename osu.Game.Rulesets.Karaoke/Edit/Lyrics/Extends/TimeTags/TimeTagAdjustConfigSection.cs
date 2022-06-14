// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagAdjustConfigSection : Section
    {
        protected override LocalisableString Title => "Config";

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ITimeTagModeState timeTagModeState)
        {
            Children = new Drawable[]
            {
                new LabelledRealTimeSliderBar<float>
                {
                    Label = "Time range",
                    Description = "Change time-range to zoom-in/zoom-out the adjust area.",
                    Current = timeTagModeState.BindableAdjustZoom
                },
                new LabelledOpacityAdjustment
                {
                    Label = "Waveform",
                    Description = "Show/hide or change the opacity of the waveform.",
                    Current = lyricEditorConfigManager.GetBindable<bool>(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowWaveform),
                    Opacity = lyricEditorConfigManager.GetBindable<float>(KaraokeRulesetLyricEditorSetting.AdjustTimeTagWaveformOpacity),
                },
                new LabelledOpacityAdjustment
                {
                    Label = "Ticks",
                    Description = "Show/hide or change the opacity of the ticks.",
                    Current = lyricEditorConfigManager.GetBindable<bool>(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowTick),
                    Opacity = lyricEditorConfigManager.GetBindable<float>(KaraokeRulesetLyricEditorSetting.AdjustTimeTagTickOpacity),
                }
            };
        }
    }
}
