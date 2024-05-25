// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class TimeTagSettingsHeader : LyricEditorSettingsHeader<TimeTagEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Orange;

    protected override EditStepTabControl CreateTabControl()
        => new RubyTagEditStepTabControl();

    protected override DescriptionFormat GetSelectionDescription(TimeTagEditStep step) =>
        step switch
        {
            TimeTagEditStep.Create => "Create the time-tag or adjust the position.",
            TimeTagEditStep.Recording => new DescriptionFormat
            {
                Text =
                    $"Press [{DescriptionFormat.LINK_KEY_ACTION}](set_time_tag_time) at the right time to set current time to time-tag. Press [{DescriptionFormat.LINK_KEY_ACTION}](clear_time_tag_time) to clear the time-tag time.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "set_time_tag_time", new InputKeyDescriptionAction
                        {
                            AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.SetTime },
                        }
                    },
                    {
                        "clear_time_tag_time", new InputKeyDescriptionAction
                        {
                            AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.ClearTime },
                        }
                    },
                },
            },
            TimeTagEditStep.Adjust => "Drag to adjust time-tag time precisely.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class RubyTagEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, TimeTagEditStep value)
        {
            return value switch
            {
                TimeTagEditStep.Create => new StepTabButton(value)
                {
                    Text = "Create",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                TimeTagEditStep.Recording => new StepTabButton(value)
                {
                    Text = "Recording",
                    SelectedColour = colours.Red,
                    UnSelectedColour = colours.RedDarker,
                },
                TimeTagEditStep.Adjust => new VerifyStepTabButton(value)
                {
                    Text = "Adjust",
                    EditMode = LyricEditorMode.EditTimeTag,
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
