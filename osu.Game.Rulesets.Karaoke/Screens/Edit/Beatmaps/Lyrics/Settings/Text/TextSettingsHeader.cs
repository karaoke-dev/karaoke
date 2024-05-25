// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Text;

public partial class TextSettingsHeader : LyricEditorSettingsHeader<IEditTextModeState, TextEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Red;

    protected override EditStepTabControl CreateTabControl()
        => new TextEditStepTabControl();

    protected override DescriptionFormat GetSelectionDescription(TextEditStep step) =>
        step switch
        {
            TextEditStep.Typing => "Edit the lyric text.",
            TextEditStep.Split => "Create/delete or split/combine the lyric.",
            TextEditStep.Verify => "Check if have lyric with no text.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class TextEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, TextEditStep value)
        {
            return value switch
            {
                TextEditStep.Typing => new StepTabButton(value)
                {
                    Text = "Typing",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                TextEditStep.Split => new StepTabButton(value)
                {
                    Text = "Split",
                    SelectedColour = colours.Red,
                    UnSelectedColour = colours.RedDarker,
                },
                TextEditStep.Verify => new VerifyStepTabButton(value)
                {
                    Text = "Verify",
                    EditMode = LyricEditorMode.EditText,
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
