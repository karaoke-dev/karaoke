// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Ruby;

public partial class RubyTagSettingsHeader : LyricEditorSettingsHeader<RubyTagEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Pink;

    protected override EditStepTabControl CreateTabControl()
        => new RubyTagEditStepTabControl();

    protected override DescriptionFormat GetSelectionDescription(RubyTagEditStep step) =>
        step switch
        {
            RubyTagEditStep.Generate => "Auto-generate rubies in the lyric.",
            RubyTagEditStep.Edit => new DescriptionFormat
            {
                Text = "Create / delete and edit lyric rubies in here.\n"
                       + $"Click [{DescriptionFormat.LINK_KEY_ACTION}](directions) to select the target lyric.\n"
                       + "Press `Tab` to switch between the ruby tags.\n"
                       + $"Than, press [{DescriptionFormat.LINK_KEY_ACTION}](adjust_text_tag_index) or button to adjust ruby index after hover to edit index area.",
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
            RubyTagEditStep.Verify => "Check invalid rubies in here",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class RubyTagEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, RubyTagEditStep value)
        {
            return value switch
            {
                RubyTagEditStep.Generate => new StepTabButton(value)
                {
                    Text = "Generate",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                RubyTagEditStep.Edit => new StepTabButton(value)
                {
                    Text = "Edit",
                    SelectedColour = colours.Red,
                    UnSelectedColour = colours.RedDarker,
                },
                RubyTagEditStep.Verify => new VerifyStepTabButton(value)
                {
                    Text = "Verify",
                    EditMode = LyricEditorMode.EditRuby,
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
