// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;

public partial class RomanisationEditStepSection : LyricEditorEditStepSection<IEditRomanisationModeState, RomanisationTagEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Orange;

    protected override Selection CreateSelection(RomanisationTagEditStep step) =>
        step switch
        {
            RomanisationTagEditStep.Generate => new Selection(),
            RomanisationTagEditStep.Edit => new Selection(),
            RomanisationTagEditStep.Verify => new RomanisationVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(RomanisationTagEditStep step) =>
        step switch
        {
            RomanisationTagEditStep.Generate => "Generate",
            RomanisationTagEditStep.Edit => "Edit",
            RomanisationTagEditStep.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, RomanisationTagEditStep step, bool active) =>
        step switch
        {
            RomanisationTagEditStep.Generate => active ? colours.Blue : colours.BlueDarker,
            RomanisationTagEditStep.Edit => active ? colours.Red : colours.RedDarker,
            RomanisationTagEditStep.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

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

    private partial class RomanisationVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditRomanisation;
    }
}
