// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class EditModeMenu : EnumMenu<KaraokeRulesetEditSetting, EditMode>
    {
        protected override KaraokeRulesetEditSetting Setting => KaraokeRulesetEditSetting.EditMode;

        public EditModeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(config, text)
        {
        }

        protected override string GetName(EditMode selection) =>
            selection switch
            {
                EditMode.LyricEditor => "Lyric",
                EditMode.Note => "Note",
                _ => throw new ArgumentOutOfRangeException(nameof(selection))
            };
    }
}
