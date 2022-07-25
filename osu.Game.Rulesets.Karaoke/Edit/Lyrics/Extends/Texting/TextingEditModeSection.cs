// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Texting
{
    public class TextingEditModeSection : EditModeSection<TextingEditMode>
    {
        [Resolved]
        private ITextingModeState textingModeState { get; set; }

        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Red;

        protected override TextingEditMode DefaultMode()
            => textingModeState.EditMode;

        protected override Dictionary<TextingEditMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    TextingEditMode.Typing, new EditModeSelectionItem("Typing", "Edit the lyric text.")
                },
                {
                    TextingEditMode.Manage, new EditModeSelectionItem("Manage", "Create/delete or split/combine the lyric.")
                }
            };

        protected override Color4 GetColour(OsuColour colours, TextingEditMode mode, bool active)
        {
            return mode switch
            {
                TextingEditMode.Typing => active ? colours.Blue : colours.BlueDarker,
                TextingEditMode.Manage => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }

        internal override void UpdateEditMode(TextingEditMode mode)
        {
            textingModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }
    }
}
