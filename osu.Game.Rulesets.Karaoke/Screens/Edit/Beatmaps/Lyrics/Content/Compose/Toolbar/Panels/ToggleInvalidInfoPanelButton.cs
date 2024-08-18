// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.Panels;

public partial class ToggleInvalidInfoPanelButton : ToggleButton
{
    public ToggleInvalidInfoPanelButton()
    {
        SetIcon(FontAwesome.Solid.ExclamationTriangle);
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.ShowInvalidInfoInComposer, Active);
    }
}
