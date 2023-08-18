// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Texting;

public partial class TextingEditStepSection : LyricEditorEditStepSection<ITextingModeState, TextingEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Red;

    protected override Selection CreateSelection(TextingEditStep step) =>
        step switch
        {
            TextingEditStep.Typing => new Selection(),
            TextingEditStep.Split => new Selection(),
            TextingEditStep.Verify => new TextingVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(TextingEditStep step) =>
        step switch
        {
            TextingEditStep.Typing => "Typing",
            TextingEditStep.Split => "Split",
            TextingEditStep.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, TextingEditStep step, bool active) =>
        step switch
        {
            TextingEditStep.Typing => active ? colours.Blue : colours.BlueDarker,
            TextingEditStep.Split => active ? colours.Red : colours.RedDarker,
            TextingEditStep.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(TextingEditStep step) =>
        step switch
        {
            TextingEditStep.Typing => "Edit the lyric text.",
            TextingEditStep.Split => "Create/delete or split/combine the lyric.",
            TextingEditStep.Verify => "Check if have lyric with no text.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class TextingVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.Texting;
    }
}
