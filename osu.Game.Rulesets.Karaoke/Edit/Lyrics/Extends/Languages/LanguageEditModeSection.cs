// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageEditModeSection : EditModeSection<LanguageEditMode>
    {
        [Resolved]
        private ILanguageModeState languageModeState { get; set; }

        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;

        protected override LanguageEditMode DefaultMode()
            => languageModeState.EditMode;

        protected override Dictionary<LanguageEditMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    LanguageEditMode.Generate, new EditModeSelectionItem("Generate", "Auto-generate language with just a click.")
                },
                {
                    LanguageEditMode.Verify, new EditModeSelectionItem("Verify", "Check if have lyric with no language.")
                }
            };

        protected override Color4 GetColour(OsuColour colours, LanguageEditMode mode, bool active) =>
            mode switch
            {
                LanguageEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
                LanguageEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };

        internal override void UpdateEditMode(LanguageEditMode mode)
        {
            languageModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }
    }
}
