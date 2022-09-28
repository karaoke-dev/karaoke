// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus
{
    public class LyricEditorPreferLayoutMenu : EnumMenu<LyricEditorLayout>
    {
        public LyricEditorPreferLayoutMenu(KaraokeRulesetLyricEditorConfigManager config, string text)
            : base(config.GetBindable<LyricEditorLayout>(KaraokeRulesetLyricEditorSetting.LyricEditorPreferLayout), text)
        {
        }
    }
}
