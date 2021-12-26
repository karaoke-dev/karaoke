// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagAutoGenerateSection : TextTagAutoGenerateSection
    {
        [Resolved]
        private ILyricRomajiChangeHandler romajiChangeHandler { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(IEnumerable<Lyric> lyrics)
            => lyrics.Where(x => x.Language == null)
                     .ToDictionary(k => k, _ => "Before generate romaji-tag, need to assign language first.");

        protected override void Apply()
            => romajiChangeHandler.AutoGenerate();

        protected override ConfigButton CreateConfigButton()
            => new RomajiTagAutoGenerateConfigButton();

        protected class RomajiTagAutoGenerateConfigButton : MultiConfigButton
        {
            protected override IEnumerable<KaraokeRulesetEditGeneratorSetting> AvailableSettings => new[]
            {
                KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig,
            };

            protected override string GetDisplayName(KaraokeRulesetEditGeneratorSetting setting) =>
                setting switch
                {
                    KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig => "Japanese",
                    _ => throw new ArgumentOutOfRangeException(nameof(setting))
                };

            protected override Popover GetPopoverBySettingType(KaraokeRulesetEditGeneratorSetting setting) =>
                setting switch
                {
                    KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig => new JaTimeTagGeneratorConfigPopover(),
                    _ => throw new ArgumentOutOfRangeException(nameof(setting))
                };
        }
    }
}
