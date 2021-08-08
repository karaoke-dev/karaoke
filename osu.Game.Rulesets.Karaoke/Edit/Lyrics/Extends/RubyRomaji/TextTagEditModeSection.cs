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

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class TextTagEditModeSection : EditModeSection<TextTagEditMode>
    {
        [Resolved]
        private Bindable<TextTagEditMode> bindableEditMode { get; set; }

        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;

        protected override TextTagEditMode DefaultMode()
            => bindableEditMode.Value;

        protected override Dictionary<TextTagEditMode, EditModeSelectionItem> CreateSelections()
            => new Dictionary<TextTagEditMode, EditModeSelectionItem>
            {
                {
                    TextTagEditMode.Generate, new EditModeSelectionItem("Generate", "Auto-generate ruby/romaji tag.")
                },
                {
                    TextTagEditMode.Edit, new EditModeSelectionItem("Edit", "Create / delete and edit lyric text tag in here.")
                },
                {
                    TextTagEditMode.Verify, new EditModeSelectionItem("Verify", "Check invalid text tag in here.")
                }
            };

        protected override Color4 GetColour(OsuColour colour, TextTagEditMode mode, bool active) =>
            mode switch
            {
                TextTagEditMode.Generate => active ? colour.Blue : colour.BlueDarker,
                TextTagEditMode.Edit => active ? colour.Red : colour.RedDarker,
                TextTagEditMode.Verify => active ? colour.Yellow : colour.YellowDarker,
                _ => throw new IndexOutOfRangeException(nameof(mode))
            };

        protected override void UpdateEditMode(TextTagEditMode mode)
        {
            bindableEditMode.Value = mode;

            base.UpdateEditMode(mode);
        }
    }
}
