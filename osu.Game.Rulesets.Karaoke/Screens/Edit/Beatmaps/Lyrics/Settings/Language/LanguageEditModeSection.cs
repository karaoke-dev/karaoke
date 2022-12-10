// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language
{
    public partial class LanguageEditModeSection : LyricEditorEditModeSection<ILanguageModeState, LanguageEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;

        protected override Selection CreateSelection(LanguageEditMode mode) =>
            mode switch
            {
                LanguageEditMode.Generate => new Selection(),
                LanguageEditMode.Verify => new LanguageVerifySelection(),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override LocalisableString GetSelectionText(LanguageEditMode mode) =>
            mode switch
            {
                LanguageEditMode.Generate => "Generate",
                LanguageEditMode.Verify => "Verify",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override Color4 GetSelectionColour(OsuColour colours, LanguageEditMode mode, bool active) =>
            mode switch
            {
                LanguageEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
                LanguageEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        protected override DescriptionFormat GetSelectionDescription(LanguageEditMode mode) =>
            mode switch
            {
                LanguageEditMode.Generate => "Auto-generate language with just a click.",
                LanguageEditMode.Verify => "Check if have lyric with no language.",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        private partial class LanguageVerifySelection : LyricEditorVerifySelection
        {
            protected override LyricEditorMode EditMode => LyricEditorMode.Language;
        }
    }
}
