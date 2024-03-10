// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class TimeTagCreateConfigSection : EditorSection
{
    protected override LocalisableString Title => "Config Tool";

    [BackgroundDependencyLoader]
    private void load(IEditTimeTagModeState editTimeTagModeState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        Children = new Drawable[]
        {
            new TimeTagCreateConfigSubsection
            {
                Current = editTimeTagModeState.BindableCreateTimeTagEditMode,
            },
            new LabelledEnumDropdown<MovingTimeTagCaretMode>
            {
                Label = "Create tag mode",
                Description = "Only create start/end time-tag or both.",
                Current = lyricEditorConfigManager.GetBindable<MovingTimeTagCaretMode>(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode),
            },
        };
    }
}
