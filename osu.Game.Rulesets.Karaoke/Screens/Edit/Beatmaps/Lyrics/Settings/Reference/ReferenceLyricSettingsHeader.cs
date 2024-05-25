// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference;

public partial class ReferenceLyricSettingsHeader : LyricEditorSettingsHeader<ReferenceLyricEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Pink;

    protected override EditStepTabControl CreateTabControl()
        => new ReferenceLyricEditStepTabControl();

    protected override DescriptionFormat GetSelectionDescription(ReferenceLyricEditStep step) =>
        step switch
        {
            ReferenceLyricEditStep.Edit => "Assign the reference lyrics.",
            ReferenceLyricEditStep.Verify => "Check any invalid reference lyric issue.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class ReferenceLyricEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, ReferenceLyricEditStep value)
        {
            return value switch
            {
                ReferenceLyricEditStep.Edit => new StepTabButton(value)
                {
                    Text = "Edit",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                ReferenceLyricEditStep.Verify => new VerifyStepTabButton(value)
                {
                    Text = "Verify",
                    EditMode = LyricEditorMode.EditReferenceLyric,
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
