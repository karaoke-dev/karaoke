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
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagAutoGenerateSection : AutoGenerateSection
    {
        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        protected override Dictionary<Lyric, string> GetDisableSelectingLyrics(IEnumerable<Lyric> lyrics)
            => lyrics.Where(x => x.Language == null)
                     .ToDictionary(k => k, _ => "Before generate time-tag, need to assign language first.");

        protected override void Apply()
            => lyricTimeTagsChangeHandler.AutoGenerate();

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
