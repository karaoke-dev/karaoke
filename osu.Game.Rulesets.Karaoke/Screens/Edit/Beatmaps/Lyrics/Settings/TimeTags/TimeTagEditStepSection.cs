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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class TimeTagEditStepSection : LyricEditorEditStepSection<IEditTimeTagModeState, TimeTagEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Orange;

    protected override Selection CreateSelection(TimeTagEditStep step) =>
        step switch
        {
            TimeTagEditStep.Create => new Selection(),
            TimeTagEditStep.Recording => new Selection(),
            TimeTagEditStep.Adjust => new TimeTagVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(TimeTagEditStep step) =>
        step switch
        {
            TimeTagEditStep.Create => "Create",
            TimeTagEditStep.Recording => "Recording",
            TimeTagEditStep.Adjust => "Adjust",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, TimeTagEditStep step, bool active) =>
        step switch
        {
            TimeTagEditStep.Create => active ? colours.Blue : colours.BlueDarker,
            TimeTagEditStep.Recording => active ? colours.Red : colours.RedDarker,
            TimeTagEditStep.Adjust => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

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

    private partial class TimeTagVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditTimeTag;
    }
}
