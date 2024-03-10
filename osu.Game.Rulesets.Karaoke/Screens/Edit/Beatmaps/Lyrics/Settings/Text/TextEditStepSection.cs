// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Text;

public partial class TextEditStepSection : LyricEditorEditStepSection<IEditTextModeState, TextEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Red;

    protected override Selection CreateSelection(TextEditStep step) =>
        step switch
        {
            TextEditStep.Typing => new Selection(),
            TextEditStep.Split => new Selection(),
            TextEditStep.Verify => new TextVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(TextEditStep step) =>
        step switch
        {
            TextEditStep.Typing => "Typing",
            TextEditStep.Split => "Split",
            TextEditStep.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, TextEditStep step, bool active) =>
        step switch
        {
            TextEditStep.Typing => active ? colours.Blue : colours.BlueDarker,
            TextEditStep.Split => active ? colours.Red : colours.RedDarker,
            TextEditStep.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(TextEditStep step) =>
        step switch
        {
            TextEditStep.Typing => "Edit the lyric text.",
            TextEditStep.Split => "Create/delete or split/combine the lyric.",
            TextEditStep.Verify => "Check if have lyric with no text.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class TextVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditText;
    }
}
