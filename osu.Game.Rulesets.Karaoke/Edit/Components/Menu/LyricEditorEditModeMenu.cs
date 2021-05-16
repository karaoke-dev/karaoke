// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class LyricEditorEditModeMenu : EnumMenu<Mode>
    {
        protected override KaraokeRulesetEditSetting Setting => KaraokeRulesetEditSetting.LyricEditorMode;

        public LyricEditorEditModeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(config, text)
        {
        }

        protected override string GetName(Mode selection)
        {
            switch (selection)
            {
                case Mode.ViewMode:
                    return "View";

                case Mode.EditMode:
                    return "Edit";

                case Mode.TypingMode:
                    return "Typing";

                case Mode.RubyRomajiMode:
                    return "Edit ruby / romaji";

                case Mode.EditNoteMode:
                    return "Edit note";

                case Mode.RecordMode:
                    return "Record";

                case Mode.TimeTagEditMode:
                    return "Edit time tag";

                default:
                    throw new ArgumentOutOfRangeException(nameof(selection));
            }
        }
    }
}
