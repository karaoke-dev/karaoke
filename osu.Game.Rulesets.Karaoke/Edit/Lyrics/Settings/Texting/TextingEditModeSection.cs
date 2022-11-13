// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Texting
{
    public class TextingEditModeSection : EditModeSection<ITextingModeState, TextingEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Red;

        protected override Selection CreateSelection(TextingEditMode mode) =>
            mode switch
            {
                TextingEditMode.Typing => new Selection(),
                TextingEditMode.Split => new Selection(),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override LocalisableString GetSelectionText(TextingEditMode mode) =>
            mode switch
            {
                TextingEditMode.Typing => "Typing",
                TextingEditMode.Split => "Split",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override Color4 GetSelectionColour(OsuColour colours, TextingEditMode mode, bool active) =>
            mode switch
            {
                TextingEditMode.Typing => active ? colours.Blue : colours.BlueDarker,
                TextingEditMode.Split => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override DescriptionFormat GetSelectionDescription(TextingEditMode mode) =>
            mode switch
            {
                TextingEditMode.Typing => "Edit the lyric text.",
                TextingEditMode.Split => "Create/delete or split/combine the lyric.",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
    }
}
