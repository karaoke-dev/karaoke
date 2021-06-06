// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class LyricEditorModeMenu : EnumMenu<LyricEditorMode>
    {
        protected override KaraokeRulesetEditSetting Setting => KaraokeRulesetEditSetting.LyricEditorMode;

        public LyricEditorModeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(config, text)
        {
        }

        protected override string GetName(LyricEditorMode selection)
        {
            switch (selection)
            {
                case LyricEditorMode.View:
                    return "View";

                case LyricEditorMode.Manage:
                    return "Manage lyrics";

                case LyricEditorMode.Typing:
                    return "Typing";

                case LyricEditorMode.EditRubyRomaji:
                    return "Edit ruby / romaji";

                case LyricEditorMode.EditNote:
                    return "Edit note";

                case LyricEditorMode.RecordTimeTag:
                    return "Record";

                case LyricEditorMode.EditTimeTag:
                    return "Edit time tag";

                case LyricEditorMode.Layout:
                    return "Select layout";

                case LyricEditorMode.Singer:
                    return "Select singer";

                case LyricEditorMode.Language:
                    return "Select language";

                default:
                    throw new ArgumentOutOfRangeException(nameof(selection));
            }
        }
    }
}
