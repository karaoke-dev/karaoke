// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagAutoGenerateSection : TextTagAutoGenerateSection
    {
        public RubyTagAutoGenerateSection()
        {
            Children = new[]
            {
                new RubyTagAutoGenerateSubsection()
            };
        }

        private class RubyTagAutoGenerateSubsection : TextTagAutoGenerateSubsection
        {
            [Resolved]
            private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; }

            protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(IEnumerable<Lyric> lyrics)
                => lyrics.Where(x => x.Language == null)
                         .ToDictionary(k => k, _ => "Before generate ruby-tag, need to assign language first.");

            protected override void Apply()
                => rubyTagsChangeHandler.AutoGenerate();

            protected override ConfigButton CreateConfigButton()
                => new RubyTagAutoGenerateConfigButton();

            protected class RubyTagAutoGenerateConfigButton : MultiConfigButton
            {
                protected override IEnumerable<KaraokeRulesetEditGeneratorSetting> AvailableSettings => new[]
                {
                    KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig,
                };

                protected override string GetDisplayName(KaraokeRulesetEditGeneratorSetting setting) =>
                    setting switch
                    {
                        KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig => "Japanese",
                        _ => throw new ArgumentOutOfRangeException(nameof(setting))
                    };

                protected override Popover GetPopoverBySettingType(KaraokeRulesetEditGeneratorSetting setting) =>
                    setting switch
                    {
                        KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig => new JaRubyTagGeneratorConfigPopover(),
                        _ => throw new ArgumentOutOfRangeException(nameof(setting))
                    };
            }
        }
    }
}
