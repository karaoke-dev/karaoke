// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Texting
{
    public partial class TextingEditModeSection : LyricEditorEditModeSection<ITextingModeState, TextingEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Red;

        protected override Selection CreateSelection(TextingEditMode mode) =>
            mode switch
            {
                TextingEditMode.Typing => new Selection(),
                TextingEditMode.Split => new Selection(),
                TextingEditMode.Verify => new TextingVerifySelection(),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override LocalisableString GetSelectionText(TextingEditMode mode) =>
            mode switch
            {
                TextingEditMode.Typing => "Typing",
                TextingEditMode.Split => "Split",
                TextingEditMode.Verify => "Verify",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override Color4 GetSelectionColour(OsuColour colours, TextingEditMode mode, bool active) =>
            mode switch
            {
                TextingEditMode.Typing => active ? colours.Blue : colours.BlueDarker,
                TextingEditMode.Split => active ? colours.Red : colours.RedDarker,
                TextingEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override DescriptionFormat GetSelectionDescription(TextingEditMode mode) =>
            mode switch
            {
                TextingEditMode.Typing => "Edit the lyric text.",
                TextingEditMode.Split => "Create/delete or split/combine the lyric.",
                TextingEditMode.Verify => "Check if have lyric with no text.",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        private partial class TextingVerifySelection : LyricEditorVerifySelection
        {
            protected override LyricEditorMode EditMode => LyricEditorMode.Texting;
        }
    }
}
