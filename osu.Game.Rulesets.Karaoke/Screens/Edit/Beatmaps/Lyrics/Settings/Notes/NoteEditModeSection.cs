// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Notes;

public partial class NoteEditModeSection : LyricEditorEditModeSection<IEditNoteModeState, NoteEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Blue;

    protected override Selection CreateSelection(NoteEditStep step) =>
        step switch
        {
            NoteEditStep.Generate => new Selection(),
            NoteEditStep.Edit => new Selection(),
            NoteEditStep.Verify => new NoteVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(NoteEditStep step) =>
        step switch
        {
            NoteEditStep.Generate => "Generate",
            NoteEditStep.Edit => "Edit",
            NoteEditStep.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, NoteEditStep step, bool active) =>
        step switch
        {
            NoteEditStep.Generate => active ? colours.Blue : colours.BlueDarker,
            NoteEditStep.Edit => active ? colours.Red : colours.RedDarker,
            NoteEditStep.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(NoteEditStep step) =>
        step switch
        {
            NoteEditStep.Generate => "Using time-tag to create default notes.",
            NoteEditStep.Edit => "Batch edit note property in here.",
            NoteEditStep.Verify => "Check invalid notes in here.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class NoteVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditNote;
    }
}
