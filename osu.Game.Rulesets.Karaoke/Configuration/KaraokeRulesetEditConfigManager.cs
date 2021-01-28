// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetEditConfigManager : InMemoryConfigManager<KaraokeRulesetEditSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // Edit mode
            Set(KaraokeRulesetEditSetting.EditMode, EditMode.LyricEditor);

            // Lyric editor
            Set(KaraokeRulesetEditSetting.LyricEditorFontSize, 28);
            Set(KaraokeRulesetEditSetting.LyricEditorMode, Mode.ViewMode);
            Set(KaraokeRulesetEditSetting.LyricEditorFastEditMode, LyricFastEditMode.None);
            Set(KaraokeRulesetEditSetting.RecordingMovingCursorMode, RecordingMovingCursorMode.None);

            Set(KaraokeRulesetEditSetting.DisplayRuby, true);
            Set(KaraokeRulesetEditSetting.DisplayRomaji, true);
            Set(KaraokeRulesetEditSetting.DisplayTranslate, true);
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
        RecordingMovingCursorMode,

        // Note editor
        DisplayRuby,
        DisplayRomaji,
        DisplayTranslate,
    }
}
