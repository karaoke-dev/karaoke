// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class ReferenceSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 300;

    private readonly IBindable<ReferenceLyricEditStep> bindableEditStep = new Bindable<ReferenceLyricEditStep>();

    [BackgroundDependencyLoader]
    private void load(IEditReferenceLyricModeState editReferenceLyricModeState)
    {
        bindableEditStep.BindTo(editReferenceLyricModeState.BindableEditStep);
        bindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new ReferenceLyricSettingsHeader();

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableEditStep.Value switch
    {
        ReferenceLyricEditStep.Edit => new EditorSection[]
        {
            new ReferenceLyricAutoGenerateSection(),
            new ReferenceLyricSection(),
            new ReferenceLyricConfigSection(),
        },
        ReferenceLyricEditStep.Verify => new EditorSection[]
        {
            new ReferenceLyricIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
