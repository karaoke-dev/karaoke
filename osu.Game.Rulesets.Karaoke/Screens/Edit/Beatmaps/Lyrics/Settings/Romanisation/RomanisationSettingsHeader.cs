// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;

public partial class RomanisationSettingsHeader : LyricEditorSettingsHeader<RomanisationTagEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Orange;

    protected override EditStepTabControl CreateTabControl()
        => new RomanisationEditStepTabControl();

    protected override DescriptionFormat GetSelectionDescription(RomanisationTagEditStep step) =>
        step switch
        {
            RomanisationTagEditStep.Generate => "Auto-generate romanisation in the lyric.",
            RomanisationTagEditStep.Edit => new DescriptionFormat
            {
                Text = "Create / delete and edit lyric rubies in here.\n"
                       + $"Click [{DescriptionFormat.LINK_KEY_ACTION}](directions) to select the target lyric.\n"
                       + "Press `Tab` to switch between the romanised syllable tags.\n"
                       + $"Than, press [{DescriptionFormat.LINK_KEY_ACTION}](adjust_text_tag_index) or button to adjust romanised syllable index after hover to edit index area.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "directions", new InputKeyDescriptionAction
                        {
                            Text = "Up or down",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.MoveToPreviousLyric,
                                KaraokeEditAction.MoveToNextLyric,
                            },
                        }
                    },
                    {
                        "adjust_text_tag_index", new InputKeyDescriptionAction
                        {
                            Text = "Keys",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.EditRubyTagReduceStartIndex,
                                KaraokeEditAction.EditRubyTagIncreaseStartIndex,
                                KaraokeEditAction.EditRubyTagReduceEndIndex,
                                KaraokeEditAction.EditRubyTagIncreaseEndIndex,
                            },
                        }
                    },
                },
            },
            RomanisationTagEditStep.Verify => "Check invalid romanisation in here.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class RomanisationEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, RomanisationTagEditStep value)
        {
            return value switch
            {
                RomanisationTagEditStep.Generate => new StepTabButton(value)
                {
                    Text = "Generate",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                RomanisationTagEditStep.Edit => new StepTabButton(value)
                {
                    Text = "Edit",
                    SelectedColour = colours.Red,
                    UnSelectedColour = colours.RedDarker,
                },
                RomanisationTagEditStep.Verify => new VerifyStepTabButton(value)
                {
                    Text = "Verify",
                    EditMode = LyricEditorMode.EditRomanisation,
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
