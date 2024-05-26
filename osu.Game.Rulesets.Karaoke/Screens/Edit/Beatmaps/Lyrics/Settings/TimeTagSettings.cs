// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class TimeTagSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;
    public override float SettingsWidth => 300;

    private readonly Bindable<TimeTagEditStep> bindableEditStep = new();

    [BackgroundDependencyLoader]
    private void load(IEditTimeTagModeState editTimeTagModeState)
    {
        bindableEditStep.BindTo(editTimeTagModeState.BindableEditStep);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new TimeTagSettingsHeader
        {
            Current = bindableEditStep,
        };

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableEditStep.Value switch
    {
        TimeTagEditStep.Create => new EditorSection[]
        {
            new TimeTagAutoGenerateSection(),
            new CreateTimeTagActionSection(),
        },
        TimeTagEditStep.Recording => new EditorSection[]
        {
            new TimeTagRecordingToolSection(),
            new TimeTagRecordingConfigSection(),
        },
        TimeTagEditStep.Adjust => new EditorSection[]
        {
            new TimeTagAdjustConfigSection(),
            new TimeTagIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
