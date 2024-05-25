// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Text;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class TextSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 300;

    private readonly IBindable<TextEditStep> bindableEditStep = new Bindable<TextEditStep>();

    [BackgroundDependencyLoader]
    private void load(IEditTextModeState editTextModeState)
    {
        bindableEditStep.BindTo(editTextModeState.BindableEditStep);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new TextSettingsHeader();

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableEditStep.Value switch
    {
        TextEditStep.Typing => new[]
        {
            new TextSwitchSpecialActionSection(),
        },
        TextEditStep.Split => new[]
        {
            new TextSwitchSpecialActionSection(),
        },
        TextEditStep.Verify => new[]
        {
            new TextIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
