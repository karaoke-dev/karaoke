// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference;

public partial class ReferenceLyricEditModeSection : LyricEditorEditModeSection<IEditReferenceLyricModeState, ReferenceLyricEditMode>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Pink;

    protected override Selection CreateSelection(ReferenceLyricEditMode mode) =>
        mode switch
        {
            ReferenceLyricEditMode.Edit => new Selection(),
            ReferenceLyricEditMode.Verify => new ReferenceLyricVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override LocalisableString GetSelectionText(ReferenceLyricEditMode mode) =>
        mode switch
        {
            ReferenceLyricEditMode.Edit => "Edit",
            ReferenceLyricEditMode.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override Color4 GetSelectionColour(OsuColour colours, ReferenceLyricEditMode mode, bool active) =>
        mode switch
        {
            ReferenceLyricEditMode.Edit => active ? colours.Blue : colours.BlueDarker,
            ReferenceLyricEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override DescriptionFormat GetSelectionDescription(ReferenceLyricEditMode mode) =>
        mode switch
        {
            ReferenceLyricEditMode.Edit => "Assign the reference lyrics.",
            ReferenceLyricEditMode.Verify => "Check any invalid reference lyric issue.",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    private partial class ReferenceLyricVerifySelection : VerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.Reference;
    }
}
