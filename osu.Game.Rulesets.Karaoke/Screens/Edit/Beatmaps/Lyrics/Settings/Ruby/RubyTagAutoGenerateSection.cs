// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Ruby;

public partial class RubyTagAutoGenerateSection : AutoGenerateSection
{
    protected override AutoGenerateSubsection CreateAutoGenerateSubsection()
        => new RubyTagAutoGenerateSubsection();

    private partial class RubyTagAutoGenerateSubsection : LyricEditorAutoGenerateSubsection
    {
        private const string language_mode = "LANGUAGE_MODE";

        public RubyTagAutoGenerateSubsection()
            : base(AutoGenerateType.AutoGenerateRubyTags)
        {
        }

        protected override DescriptionFormat CreateInvalidDescriptionFormat()
            => new()
            {
                Text = $"Seems some lyric missing language, go to [{DescriptionFormat.LINK_KEY_ACTION}]({language_mode}) to fill the language.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        language_mode, new SwitchModeDescriptionAction
                        {
                            Text = "edit language mode",
                            Mode = LyricEditorMode.EditLanguage,
                        }
                    },
                },
            };

        protected override ConfigButton CreateConfigButton()
            => new RubyTagAutoGenerateConfigButton();

        protected partial class RubyTagAutoGenerateConfigButton : MultiConfigButton
        {
            protected override IEnumerable<KaraokeRulesetEditGeneratorSetting> AvailableSettings => new[]
            {
                KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig,
            };

            protected override string GetDisplayName(KaraokeRulesetEditGeneratorSetting setting) =>
                setting switch
                {
                    KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig => "Japanese",
                    _ => throw new ArgumentOutOfRangeException(nameof(setting)),
                };
        }
    }
}
