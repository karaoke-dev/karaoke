// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class LanguageSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 300;

    private readonly IBindable<LanguageEditStep> bindableMode = new Bindable<LanguageEditStep>();

    [BackgroundDependencyLoader]
    private void load(ILanguageModeState languageModeState)
    {
        bindableMode.BindTo(languageModeState.BindableEditMode);
        bindableMode.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableMode.Value switch
    {
        LanguageEditStep.Generate => new Drawable[]
        {
            new LanguageEditModeSection(),
            new LanguageSwitchSpecialActionSection(),
        },
        LanguageEditStep.Verify => new Drawable[]
        {
            new LanguageEditModeSection(),
            new LanguageIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
