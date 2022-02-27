// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagEditModeSection : LyricEditorEditModeSection
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Orange;

        protected override Dictionary<LyricEditorMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    LyricEditorMode.CreateTimeTag,
                    new EditModeSelectionItem("Create", "Use keyboard to control caret position, press `N` to create new time-tag and press `D` to delete exist time-tag.")
                },
                {
                    LyricEditorMode.RecordTimeTag, new EditModeSelectionItem("Recording", "Press spacing button at the right time to set current time to time-tag.")
                },
                {
                    LyricEditorMode.AdjustTimeTag, new EditModeSelectionItem("Adjust", "Drag to adjust time-tag time precisely.")
                }
            };

        protected override Color4 GetColour(OsuColour colours, LyricEditorMode mode, bool active)
        {
            return mode switch
            {
                LyricEditorMode.CreateTimeTag => active ? colours.Blue : colours.BlueDarker,
                LyricEditorMode.RecordTimeTag => active ? colours.Red : colours.RedDarker,
                LyricEditorMode.AdjustTimeTag => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }
    }
}
