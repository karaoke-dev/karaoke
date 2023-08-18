// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class TimeTagSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;
    public override float SettingsWidth => 300;

    private readonly IBindable<TimeTagEditStep> bindableEditStep = new Bindable<TimeTagEditStep>();

    [BackgroundDependencyLoader]
    private void load(ITimeTagModeState timeTagModeState)
    {
        bindableEditStep.BindTo(timeTagModeState.BindableEditStep);
        bindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableEditStep.Value switch
    {
        TimeTagEditStep.Create => new Drawable[]
        {
            new TimeTagEditStepSection(),
            new TimeTagAutoGenerateSection(),
            new TimeTagCreateConfigSection(),
            new CreateTimeTagActionReceiver(),
        },
        TimeTagEditStep.Recording => new Drawable[]
        {
            new TimeTagEditStepSection(),
            new TimeTagRecordingConfigSection(),
            new RecordTimeTagActionReceiver(),
        },
        TimeTagEditStep.Adjust => new Drawable[]
        {
            new TimeTagEditStepSection(),
            new TimeTagAdjustConfigSection(),
            new TimeTagIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
