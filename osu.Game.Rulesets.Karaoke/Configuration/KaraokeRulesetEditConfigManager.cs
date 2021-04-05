// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetEditConfigManager : InMemoryConfigManager<KaraokeRulesetEditSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // Edit mode
            SetDefault(KaraokeRulesetEditSetting.EditMode, EditMode.LyricEditor);

            // Lyric editor
            SetDefault(KaraokeRulesetEditSetting.LyricEditorFontSize, 28f);
            SetDefault(KaraokeRulesetEditSetting.LyricEditorMode, Mode.ViewMode);
            SetDefault(KaraokeRulesetEditSetting.LyricEditorFastEditMode, LyricFastEditMode.None);
            SetDefault(KaraokeRulesetEditSetting.RecordingMovingCaretMode, RecordingMovingCaretMode.None);
            SetDefault(KaraokeRulesetEditSetting.AutoFocusToEditLyric, true);
            SetDefault(KaraokeRulesetEditSetting.AutoFocusToEditLyricSkipRows, 1, 0, 4);

            SetDefault(KaraokeRulesetEditSetting.DisplayRuby, true);
            SetDefault(KaraokeRulesetEditSetting.DisplayRomaji, true);
            SetDefault(KaraokeRulesetEditSetting.DisplayTranslate, true);

            // Lock
            SetDefault(KaraokeRulesetEditSetting.ClickToLockLyricState, LockState.Partial);
        }
    }

    public enum KaraokeRulesetEditSetting
    {
        // Edit mode
        EditMode,

        // Lyric editor
        LyricEditorFontSize,
        LyricEditorMode,
        LyricEditorFastEditMode,
        RecordingMovingCaretMode,
        AutoFocusToEditLyric,
        AutoFocusToEditLyricSkipRows,

        // Note editor
        DisplayRuby,
        DisplayRomaji,
        DisplayTranslate,

        // Lock
        ClickToLockLyricState,
    }
}
