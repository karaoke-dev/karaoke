// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.TimeTags
{
    public class TimeTagAutoGenerateSection : LyricEditorSection
    {
        protected override LocalisableString Title => "Auto generate";

        public TimeTagAutoGenerateSection()
        {
            Children = new[]
            {
                new TimeTageAutoGenerateSubsection()
            };
        }

        private class TimeTageAutoGenerateSubsection : AutoGenerateSubsection
        {
            private const string language_mode = "LANGUAGE_MODE";

            public TimeTageAutoGenerateSubsection()
                : base(LyricAutoGenerateProperty.AutoGenerateTimeTags)
            {
            }

            protected override DescriptionFormat CreateInvalidLyricDescriptionFormat()
                => new()
                {
                    Text = $"Seems some lyric missing language, go to [{DescriptionFormat.LINK_KEY_EDIT_MODE}]({language_mode}) to fill the language.",
                    EditModes = new Dictionary<string, SwitchMode>
                    {
                        {
                            language_mode, new SwitchMode
                            {
                                Text = "edit language mode",
                                Mode = LyricEditorMode.Language
                            }
                        }
                    }
                };

            protected override ConfigButton CreateConfigButton()
                => new TimeTagAutoGenerateConfigButton();

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
