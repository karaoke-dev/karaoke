// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Notes;

public partial class NoteSettingsHeader : LyricEditorSettingsHeader<IEditNoteModeState, NoteEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Blue;

    protected override EditStepTabControl CreateTabControl()
        => new NoteEditStepTabControl();

    protected override DescriptionFormat GetSelectionDescription(NoteEditStep step) =>
        step switch
        {
            NoteEditStep.Generate => "Using time-tag to create default notes.",
            NoteEditStep.Edit => "Batch edit note property in here.",
            NoteEditStep.Verify => "Check invalid notes in here.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class NoteEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, NoteEditStep value)
        {
            return value switch
            {
                NoteEditStep.Generate => new StepTabButton(value)
                {
                    Text = "Generate",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                NoteEditStep.Edit => new StepTabButton(value)
                {
                    Text = "Edit",
                    SelectedColour = colours.Red,
                    UnSelectedColour = colours.RedDarker,
                },
                NoteEditStep.Verify => new VerifyStepTabButton(value)
                {
                    Text = "Verify",
                    EditMode = LyricEditorMode.EditNote,
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
