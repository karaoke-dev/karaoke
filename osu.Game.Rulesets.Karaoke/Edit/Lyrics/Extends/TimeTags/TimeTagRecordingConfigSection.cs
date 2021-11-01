// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagRecordingConfigSection : Section
    {
        protected override string Title => "Config";

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            Children = new Drawable[]
            {
                new LabelledEnumDropdown<MovingTimeTagCaretMode>
                {
                    Label = "Record tag mode",
                    Description = "Only record time with start/end time-tag while recording.",
                    Current = lyricEditorConfigManager.GetBindable<MovingTimeTagCaretMode>(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode),
                },
                new LabelledSwitchButton
                {
                    Label = "Auto move to next tag",
                    Description = "Auto move to next time-tag if set time to current time-tag.",
                    Current = lyricEditorConfigManager.GetBindable<bool>(KaraokeRulesetLyricEditorSetting.RecordingAutoMoveToNextTimeTag),
                }
            };
        }
    }
}
