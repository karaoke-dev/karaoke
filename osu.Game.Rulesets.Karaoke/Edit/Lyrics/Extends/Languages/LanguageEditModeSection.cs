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

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageEditModeSection : EditModeSection<LanguageEditMode>
    {
        [Resolved]
        private Bindable<LanguageEditMode> bindableEditMode { get; set; }

        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;

        protected override LanguageEditMode DefaultMode()
            => bindableEditMode.Value;

        protected override Dictionary<LanguageEditMode, EditModeSelectionItem> CreateSelections()
            => new Dictionary<LanguageEditMode, EditModeSelectionItem>
            {
                {
                    LanguageEditMode.Generate, new EditModeSelectionItem("Generate", "Auto-generate language with just a click.")
                },
                {
                    LanguageEditMode.Verify, new EditModeSelectionItem("Verify", "Check if have lyric with no language.")
                }
            };

        protected override Color4 GetColour(OsuColour colour, LanguageEditMode mode, bool active)
        {
            switch (mode)
            {
                case LanguageEditMode.Generate:
                    return active ? colour.Blue : colour.BlueDarker;

                case LanguageEditMode.Verify:
                    return active ? colour.Yellow : colour.YellowDarker;

                default:
                    throw new IndexOutOfRangeException(nameof(mode));
            }
        }

        protected override void UpdateEditMode(LanguageEditMode mode)
        {
            bindableEditMode.Value = mode;

            base.UpdateEditMode(mode);
        }
    }
}
