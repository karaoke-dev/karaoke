// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class LyricEditorLeftSideModeMenu : EnumMenu<LyricFastEditMode>
    {
        protected override KaraokeRulesetEditSetting Setting => KaraokeRulesetEditSetting.LyricEditorFastEditMode;

        public LyricEditorLeftSideModeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(config, text)
        {
        }

        protected override string GetName(LyricFastEditMode selection)
        {
            switch (selection)
            {
                case LyricFastEditMode.None:
                    return "None";

                case LyricFastEditMode.Layout:
                    return "Layout selection";

                case LyricFastEditMode.Singer:
                    return "Singer selection";

                case LyricFastEditMode.Language:
                    return "Language selection";

                case LyricFastEditMode.TimeTag:
                    return "Time tag display";

                case LyricFastEditMode.Order:
                    return "Lyric order";

                default:
                    throw new ArgumentOutOfRangeException(nameof(selection));
            }
        }
    }
}
