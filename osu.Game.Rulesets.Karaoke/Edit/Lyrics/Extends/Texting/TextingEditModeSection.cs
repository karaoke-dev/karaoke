// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Texting
{
    public class TextingEditModeSection : EditModeSection<ITextingModeState, TextingEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Red;

        protected override Dictionary<TextingEditMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    TextingEditMode.Typing, new EditModeSelectionItem("Typing", "Edit the lyric text.")
                },
                {
                    TextingEditMode.Split, new EditModeSelectionItem("Split", "Create/delete or split/combine the lyric.")
                }
            };

        protected override Color4 GetColour(OsuColour colours, TextingEditMode mode, bool active)
        {
            return mode switch
            {
                TextingEditMode.Typing => active ? colours.Blue : colours.BlueDarker,
                TextingEditMode.Split => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }
    }
}
