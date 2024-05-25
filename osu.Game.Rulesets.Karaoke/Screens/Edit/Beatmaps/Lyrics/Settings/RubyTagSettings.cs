// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Ruby;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class RubyTagSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 350;

    private readonly IBindable<RubyTagEditStep> bindableEditStep = new Bindable<RubyTagEditStep>();

    [BackgroundDependencyLoader]
    private void load(IEditRubyModeState editRubyModeState)
    {
        bindableEditStep.BindTo(editRubyModeState.BindableEditStep);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new RubyTagSettingsHeader();

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableEditStep.Value switch
    {
        RubyTagEditStep.Generate => new EditorSection[]
        {
            new RubyTagConfigToolSection(),
            new RubyTagAutoGenerateSection(),
        },
        RubyTagEditStep.Edit => new EditorSection[]
        {
            new RubyTagConfigToolSection(),
            new RubyTagEditSection(),
        },
        RubyTagEditStep.Verify => new EditorSection[]
        {
            new RubyTagConfigToolSection(),
            new RubyTagIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
