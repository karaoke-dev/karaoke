// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romaji;

public partial class RomajiAutoGenerateSection : AutoGenerateSection
{
    protected override AutoGenerateSubsection CreateAutoGenerateSubsection()
        => new RomajiTagAutoGenerateSubsection();

    private partial class RomajiTagAutoGenerateSubsection : LyricEditorAutoGenerateSubsection
    {
        private const string language_mode = "LANGUAGE_MODE";
        private const string time_tag_mode = "TIME_TAG_MODE";

        public RomajiTagAutoGenerateSubsection()
            : base(AutoGenerateType.AutoGenerateTimeTagRomaji)
        {
        }

        protected override DescriptionFormat CreateInvalidDescriptionFormat()
            => new()
            {
                Text = $"Seems some lyric missing language or time-tag, go to [{DescriptionFormat.LINK_KEY_ACTION}]({language_mode}) to fill the language, or [{DescriptionFormat.LINK_KEY_ACTION}]({time_tag_mode}) to fill the time-tag.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        language_mode,
                        new SwitchModeDescriptionAction
                        {
                            Text = "edit language mode",
                            Mode = LyricEditorMode.EditLanguage,
                        }
                    },
                    {
                        time_tag_mode,
                        new SwitchModeDescriptionAction
                        {
                            Text = "edit time-tag mode",
                            Mode = LyricEditorMode.EditTimeTag,
                        }
                    },
                },
            };

        protected override ConfigButton CreateConfigButton()
            => new RomajiTagAutoGenerateConfigButton();

        protected partial class RomajiTagAutoGenerateConfigButton : MultiConfigButton
        {
            protected override IEnumerable<KaraokeRulesetEditGeneratorSetting> AvailableSettings => new[]
            {
                KaraokeRulesetEditGeneratorSetting.JaRomajiGeneratorConfig,
            };

            protected override string GetDisplayName(KaraokeRulesetEditGeneratorSetting setting) =>
                setting switch
                {
                    KaraokeRulesetEditGeneratorSetting.JaRomajiGeneratorConfig => "Japanese",
                    _ => throw new ArgumentOutOfRangeException(nameof(setting)),
                };
        }
    }
}
