// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagAutoGenerateSection : Section
    {
        protected override string Title => "Auto generate";

        public TimeTagAutoGenerateSection()
        {
            Children = new[]
            {
                new TimeTageAutoGenerateSubsection()
            };
        }

        private class TimeTageAutoGenerateSubsection : AutoGenerateSubsection
        {
            public TimeTageAutoGenerateSubsection()
                : base(LyricAutoGenerateProperty.AutoGenerateTimeTags)
            {
            }

            protected override InvalidLyricAlertTextContainer CreateInvalidLyricAlertTextContainer()
                => new InvalidLyricLanguageAlertTextContainer();

            protected override ConfigButton CreateConfigButton()
                => new TimeTagAutoGenerateConfigButton();

            protected class InvalidLyricLanguageAlertTextContainer : InvalidLyricAlertTextContainer
            {
                private const string language_mode = "LANGUAGE_MODE";

                public InvalidLyricLanguageAlertTextContainer()
                {
                    SwitchToEditorMode(language_mode, "edit language mode", LyricEditorMode.Language);
                    Text = $"Seems some lyric missing language, go to [{language_mode}] to fill the language.";
                }
            }

            protected class TimeTagAutoGenerateConfigButton : MultiConfigButton
            {
                protected override IEnumerable<KaraokeRulesetEditGeneratorSetting> AvailableSettings => new[]
                {
                    KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig,
                    KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig,
                };

                protected override string GetDisplayName(KaraokeRulesetEditGeneratorSetting setting) =>
                    setting switch
                    {
                        KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig => "Japanese",
                        KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig => "Chinese",
                        _ => throw new ArgumentOutOfRangeException(nameof(setting))
                    };

                protected override Popover GetPopoverBySettingType(KaraokeRulesetEditGeneratorSetting setting) =>
                    setting switch
                    {
                        KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig => new JaTimeTagGeneratorConfigPopover(),
                        KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig => new ZhTimeTagGeneratorConfigPopover(),
                        _ => throw new ArgumentOutOfRangeException(nameof(setting))
                    };
            }
        }
    }
}
