// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class LanguageSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 300;

    private readonly Bindable<LanguageEditStep> bindableEditStep = new();

    [BackgroundDependencyLoader]
    private void load(IEditLanguageModeState editLanguageModeState)
    {
        bindableEditStep.BindTo(editLanguageModeState.BindableEditStep);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new LanguageSettingsHeader
        {
            Current = bindableEditStep,
        };

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableEditStep.Value switch
    {
        LanguageEditStep.Generate => new[]
        {
            new LanguageSwitchSpecialActionSection(),
        },
        LanguageEditStep.Verify => new[]
        {
            new LanguageIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
