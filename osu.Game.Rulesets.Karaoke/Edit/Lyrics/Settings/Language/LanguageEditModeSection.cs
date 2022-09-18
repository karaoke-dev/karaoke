﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Language
{
    public class LanguageEditModeSection : EditModeSection<ILanguageModeState, LanguageEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;

        protected override EditModeSelectionItem CreateSelectionItem(LanguageEditMode editMode) =>
            editMode switch
            {
                LanguageEditMode.Generate => new EditModeSelectionItem("Generate", "Auto-generate language with just a click."),
                LanguageEditMode.Verify => new EditModeSelectionItem("Verify", "Check if have lyric with no language."),
                _ => throw new ArgumentOutOfRangeException(nameof(editMode), editMode, null)
            };

        protected override Color4 GetColour(OsuColour colours, LanguageEditMode mode, bool active) =>
            mode switch
            {
                LanguageEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
                LanguageEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
    }
}
