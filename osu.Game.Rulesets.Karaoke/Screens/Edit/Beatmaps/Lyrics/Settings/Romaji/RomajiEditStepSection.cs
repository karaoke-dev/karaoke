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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romaji;

public partial class RomajiEditStepSection : LyricEditorEditStepSection<IEditRomajiModeState, RomajiTagEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Orange;

    protected override Selection CreateSelection(RomajiTagEditStep step) =>
        step switch
        {
            RomajiTagEditStep.Generate => new Selection(),
            RomajiTagEditStep.Edit => new Selection(),
            RomajiTagEditStep.Verify => new RomajiTagVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(RomajiTagEditStep step) =>
        step switch
        {
            RomajiTagEditStep.Generate => "Generate",
            RomajiTagEditStep.Edit => "Edit",
            RomajiTagEditStep.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, RomajiTagEditStep step, bool active) =>
        step switch
        {
            RomajiTagEditStep.Generate => active ? colours.Blue : colours.BlueDarker,
            RomajiTagEditStep.Edit => active ? colours.Red : colours.RedDarker,
            RomajiTagEditStep.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(RomajiTagEditStep step) =>
        step switch
        {
            RomajiTagEditStep.Generate => "Auto-generate romajies in the lyric.",
            RomajiTagEditStep.Edit => new DescriptionFormat
            {
                Text = "Create / delete and edit lyric rubies in here.\n"
                       + $"Click [{DescriptionFormat.LINK_KEY_ACTION}](directions) to select the target lyric.\n"
                       + "Press `Tab` to switch between the romaji tags.\n"
                       + $"Than, press [{DescriptionFormat.LINK_KEY_ACTION}](adjust_text_tag_index) or button to adjust romaji index after hover to edit index area.",
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
            RomajiTagEditStep.Verify => "Check invalid romajies in here.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class RomajiTagVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditRomaji;
    }
}
