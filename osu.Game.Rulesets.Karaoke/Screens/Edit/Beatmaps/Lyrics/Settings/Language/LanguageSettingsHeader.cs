// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language;

public partial class LanguageSettingsHeader : LyricEditorSettingsHeader<LanguageEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Pink;

    protected override EditStepTabControl CreateTabControl()
        => new LanguageEditStepTabControl();

    protected override DescriptionFormat GetSelectionDescription(LanguageEditStep step) =>
        step switch
        {
            LanguageEditStep.Generate => "Auto-generate language with just a click.",
            LanguageEditStep.Verify => "Check if have lyric with no language.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class LanguageEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, LanguageEditStep value)
        {
            return value switch
            {
                LanguageEditStep.Generate => new StepTabButton(value)
                {
                    Text = "Generate",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                LanguageEditStep.Verify => new VerifyStepTabButton(value)
                {
                    Text = "Verify",
                    EditMode = LyricEditorMode.EditLanguage,
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
