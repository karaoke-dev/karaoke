// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Notes
{
    public class NoteEditModeSection : EditModeSection<IEditNoteModeState, NoteEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Blue;

        protected override LocalisableString GetSelectionText(NoteEditMode mode) =>
            mode switch
            {
                NoteEditMode.Generate => "Generate",
                NoteEditMode.Edit => "Edit",
                NoteEditMode.Verify => "Verify",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override Color4 GetSelectionColour(OsuColour colours, NoteEditMode mode, bool active) =>
            mode switch
            {
                NoteEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
                NoteEditMode.Edit => active ? colours.Red : colours.RedDarker,
                NoteEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };

        protected override DescriptionFormat GetSelectionDescription(NoteEditMode mode) =>
            mode switch
            {
                NoteEditMode.Generate => "Using time-tag to create default notes.",
                NoteEditMode.Edit => "Batch edit note property in here.",
                NoteEditMode.Verify => "Check invalid notes in here.",
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
    }
}
