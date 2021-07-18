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
            SetDefault(KaraokeRulesetLyricEditorSetting.RecordingAutoMoveToNext, true);
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
        RecordingAutoMoveToNext,
    }
}
