// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class LyricEditorMovingCursorModeMenu : EnumMenu<RecordingMovingCaretMode>
    {
        protected override KaraokeRulesetEditSetting Setting => KaraokeRulesetEditSetting.RecordingMovingCursorMode;

        public LyricEditorMovingCursorModeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(config, text)
        {
        }

        protected override string GetName(RecordingMovingCaretMode selection)
        {
            switch (selection)
            {
                case RecordingMovingCaretMode.None:
                    return "None";

                case RecordingMovingCaretMode.OnlyStartTag:
                    return "Skip start tag";

                case RecordingMovingCaretMode.OnlyEndTag:
                    return "Skip end tag";

                default:
                    throw new ArgumentOutOfRangeException(nameof(selection));
            }
        }
    }
}
