// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetLyricEditorConfigManager : InMemoryConfigManager<KaraokeRulesetLyricEditorSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // General
            SetDefault(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, 28f);
            SetDefault(KaraokeRulesetLyricEditorSetting.LyricEditorMode, LyricEditorMode.View);
            SetDefault(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyric, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyricSkipRows, 1, 0, 4);
            SetDefault(KaraokeRulesetLyricEditorSetting.ClickToLockLyricState, LockState.Partial);

            // Create time-tag.
            SetDefault(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode, MovingTimeTagCaretMode.None);

            // Recording
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode, MovingTimeTagCaretMode.None);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingAutoMoveToNextTimeTag, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowWaveform, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagWaveformOpacity, 0.5f, 0, 1, 0.01f);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagShowTick, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingTimeTagTickOpacity, 0.5f, 0, 1, 0.01f);

            // Adjust
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowWaveform, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagWaveformOpacity, 0.5f, 0, 1, 0.01f);
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowTick, true);
            SetDefault(KaraokeRulesetLyricEditorSetting.AdjustTimeTagTickOpacity, 0.5f, 0, 1, 0.01f);
        }
    }

    public enum KaraokeRulesetLyricEditorSetting
    {
        // General
        LyricEditorFontSize,
        LyricEditorMode,
        AutoFocusToEditLyric,
        AutoFocusToEditLyricSkipRows,
        ClickToLockLyricState,

        // Create time-tag.
        CreateTimeTagMovingCaretMode,

        // Recording
        RecordingTimeTagMovingCaretMode,
        RecordingAutoMoveToNextTimeTag,
        RecordingTimeTagShowWaveform,
        RecordingTimeTagWaveformOpacity,
        RecordingTimeTagShowTick,
        RecordingTimeTagTickOpacity,

        // Adjust
        AdjustTimeTagShowWaveform,
        AdjustTimeTagWaveformOpacity,
        AdjustTimeTagShowTick,
        AdjustTimeTagTickOpacity,
    }
}
