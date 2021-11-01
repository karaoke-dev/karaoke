// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagCreateConfigSection : Section
    {
        protected override string Title => "Config";

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            Children = new[]
            {
                new LabelledEnumDropdown<MovingTimeTagCaretMode>
                {
                    Label = "Create tag mode",
                    Description = "Only create start/end time-tag or both.",
                    Current = lyricEditorConfigManager.GetBindable<MovingTimeTagCaretMode>(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode),
                }
            };
        }
    }
}
