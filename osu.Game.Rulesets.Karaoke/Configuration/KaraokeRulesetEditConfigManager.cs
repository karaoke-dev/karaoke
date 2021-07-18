// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetEditConfigManager : InMemoryConfigManager<KaraokeRulesetEditSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // Edit mode
            SetDefault(KaraokeRulesetEditSetting.EditMode, EditMode.LyricEditor);

            // Note editor
            SetDefault(KaraokeRulesetEditSetting.DisplayRuby, true);
            SetDefault(KaraokeRulesetEditSetting.DisplayRomaji, true);
            SetDefault(KaraokeRulesetEditSetting.DisplayTranslate, true);
        }
    }

    public enum KaraokeRulesetEditSetting
    {
        // Edit mode
        EditMode,

        // Note editor
        DisplayRuby,
        DisplayRomaji,
        DisplayTranslate,
    }
}
