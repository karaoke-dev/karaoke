﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
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

    private readonly IBindable<LanguageEditStep> bindableEditStep = new Bindable<LanguageEditStep>();

    [BackgroundDependencyLoader]
    private void load(IEditLanguageModeState editLanguageModeState)
    {
        bindableEditStep.BindTo(editLanguageModeState.BindableEditStep);
        bindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableEditStep.Value switch
    {
        LanguageEditStep.Generate => new Drawable[]
        {
            new LanguageEditStepSection(),
            new LanguageSwitchSpecialActionSection(),
        },
        LanguageEditStep.Verify => new Drawable[]
        {
            new LanguageEditStepSection(),
            new LanguageIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
