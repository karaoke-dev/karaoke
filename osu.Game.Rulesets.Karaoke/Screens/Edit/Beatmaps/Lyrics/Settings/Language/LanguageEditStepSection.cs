// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language;

public partial class LanguageEditStepSection : LyricEditorEditStepSection<ILanguageModeState, LanguageEditStep>
{
    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Pink;

    protected override Selection CreateSelection(LanguageEditStep step) =>
        step switch
        {
            LanguageEditStep.Generate => new Selection(),
            LanguageEditStep.Verify => new LanguageVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(LanguageEditStep step) =>
        step switch
        {
            LanguageEditStep.Generate => "Generate",
            LanguageEditStep.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, LanguageEditStep step, bool active) =>
        step switch
        {
            LanguageEditStep.Generate => active ? colours.Blue : colours.BlueDarker,
            LanguageEditStep.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(LanguageEditStep step) =>
        step switch
        {
            LanguageEditStep.Generate => "Auto-generate language with just a click.",
            LanguageEditStep.Verify => "Check if have lyric with no language.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class LanguageVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditLanguage;
    }
}
