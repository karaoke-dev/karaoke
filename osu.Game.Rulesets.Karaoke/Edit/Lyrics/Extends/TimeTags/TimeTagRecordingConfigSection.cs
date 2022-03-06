// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagRecordingConfigSection : Section
    {
        protected override string Title => "Config";

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ITimeTagModeState timeTagModeState)
        {
            Children = new Drawable[]
            {
                new LabelledEnumDropdown<MovingTimeTagCaretMode>
                {
                    Label = "Record tag mode",
                    Description = "Only record time with start/end time-tag while recording.",
                    Current = lyricEditorConfigManager.GetBindable<MovingTimeTagCaretMode>(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode),
                },
                new LabelledSwitchButton
                {
                    Label = "Auto move to next tag",
                    Description = "Auto move to next time-tag if set time to current time-tag.",
                    Current = lyricEditorConfigManager.GetBindable<bool>(KaraokeRulesetLyricEditorSetting.RecordingAutoMoveToNextTimeTag),
                },
                new LabelledSwitchButton
                {
                    Label = "Change the time by time-tag.",
                    Description = "Change the track time if change the recording caret while pausing.",
                    Current = lyricEditorConfigManager.GetBindable<bool>(KaraokeRulesetLyricEditorSetting.RecordingChangeTimeWhileMovingTheCaret),
                },
                new LabelledRealTimeSliderBar<float>
                {
                    Label = "Time range",
                    Description = "Change time-range to zoom-in/zoom-out the recording area.",
                    Current = timeTagModeState.BindableRecordZoom
                },
                new LabelledOpacityAdjustment
                {
                    Label = "Waveform",
                    Description = "Show/hide or change the opacity of the waveform.",
                    Current = lyricEditorConfigManager.GetBindable<bool>(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowWaveform),
                    Opacity = lyricEditorConfigManager.GetBindable<float>(KaraokeRulesetLyricEditorSetting.RecordingTimeTagWaveformOpacity),
                },
                new LabelledOpacityAdjustment
                {
                    Label = "Ticks",
                    Description = "Show/hide or change the opacity of the ticks.",
                    Current = lyricEditorConfigManager.GetBindable<bool>(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowTick),
                    Opacity = lyricEditorConfigManager.GetBindable<float>(KaraokeRulesetLyricEditorSetting.RecordingTimeTagTickOpacity),
                }
            };
        }
    }
}
