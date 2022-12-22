// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.TimeTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji
{
    public partial class RomajiTagAutoGenerateSection : TextTagAutoGenerateSection
    {
        protected override AutoGenerateSubsection CreateAutoGenerateSubsection()
            => new RomajiTagAutoGenerateSubsection();

        private partial class RomajiTagAutoGenerateSubsection : TextTagAutoGenerateSubsection
        {
            public RomajiTagAutoGenerateSubsection()
                : base(LyricAutoGenerateProperty.AutoGenerateRomajiTags)
            {
            }

            protected override ConfigButton CreateConfigButton()
                => new RomajiTagAutoGenerateConfigButton();

            protected partial class RomajiTagAutoGenerateConfigButton : MultiConfigButton
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
}
