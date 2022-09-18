// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RubyTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji
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
            public RubyTagAutoGenerateSubsection()
                : base(LyricAutoGenerateProperty.AutoGenerateRubyTags)
            {
            }

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
