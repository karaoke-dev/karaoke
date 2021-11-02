// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditModeSection : EditModeSection<NoteEditMode>
    {
        [Resolved]
        private Bindable<NoteEditMode> bindableEditMode { get; set; }

        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Blue;

        protected override NoteEditMode DefaultMode()
            => bindableEditMode.Value;

        protected override Dictionary<NoteEditMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    NoteEditMode.Generate, new EditModeSelectionItem("Generate", "Using time-tag to create default notes.")
                },
                {
                    NoteEditMode.Edit, new EditModeSelectionItem("Edit", "Batch edit note property in here.")
                },
                {
                    NoteEditMode.Verify, new EditModeSelectionItem("Verify", "Check invalid notes in here.")
                }
            };

        protected override Color4 GetColour(OsuColour colour, NoteEditMode mode, bool active) =>
            mode switch
            {
                NoteEditMode.Generate => active ? colour.Blue : colour.BlueDarker,
                NoteEditMode.Edit => active ? colour.Red : colour.RedDarker,
                NoteEditMode.Verify => active ? colour.Yellow : colour.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };

        protected override void UpdateEditMode(NoteEditMode mode)
        {
            bindableEditMode.Value = mode;

            base.UpdateEditMode(mode);
        }
    }
}
