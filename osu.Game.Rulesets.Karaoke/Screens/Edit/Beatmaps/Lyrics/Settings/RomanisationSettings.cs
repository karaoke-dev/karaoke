// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class RomanisationSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 350;

    private readonly IBindable<RomanisationTagEditStep> bindableEditStep = new Bindable<RomanisationTagEditStep>();

    [BackgroundDependencyLoader]
    private void load(IEditRomanisationModeState romanisationModeState)
    {
        bindableEditStep.BindTo(romanisationModeState.BindableEditStep);
        bindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new RomanisationSettingsHeader();

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableEditStep.Value switch
    {
        RomanisationTagEditStep.Generate => new[]
        {
            new RomanisationAutoGenerateSection(),
        },
        RomanisationTagEditStep.Edit => new[]
        {
            new RomanisationEditSection(),
        },
        RomanisationTagEditStep.Verify => new[]
        {
            new RomanisationIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
