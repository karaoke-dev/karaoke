// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference;

public partial class ReferenceLyricEditStepSection : LyricEditorEditStepSection<IEditReferenceLyricModeState, ReferenceLyricEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Pink;

    protected override Selection CreateSelection(ReferenceLyricEditStep step) =>
        step switch
        {
            ReferenceLyricEditStep.Edit => new Selection(),
            ReferenceLyricEditStep.Verify => new ReferenceLyricVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(ReferenceLyricEditStep step) =>
        step switch
        {
            ReferenceLyricEditStep.Edit => "Edit",
            ReferenceLyricEditStep.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, ReferenceLyricEditStep step, bool active) =>
        step switch
        {
            ReferenceLyricEditStep.Edit => active ? colours.Blue : colours.BlueDarker,
            ReferenceLyricEditStep.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(ReferenceLyricEditStep step) =>
        step switch
        {
            ReferenceLyricEditStep.Edit => "Assign the reference lyrics.",
            ReferenceLyricEditStep.Verify => "Check any invalid reference lyric issue.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class ReferenceLyricVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.Reference;
    }
}
