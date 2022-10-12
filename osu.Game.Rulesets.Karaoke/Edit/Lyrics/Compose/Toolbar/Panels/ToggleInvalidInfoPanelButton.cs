// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar.Panels
{
    public class ToggleInvalidInfoPanelButton : LyricEditorConfigButton
    {
        protected override KaraokeRulesetLyricEditorSetting Setting => KaraokeRulesetLyricEditorSetting.ShowInvalidInfoInComposer;

        protected override IconUsage Icon => FontAwesome.Solid.ExclamationTriangle;
    }
}
