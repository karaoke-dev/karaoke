// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
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
        bindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableEditStep.Value switch
    {
        TextEditStep.Typing => new Drawable[]
        {
            new TextEditStepSection(),
            new TextSwitchSpecialActionSection(),
        },
        TextEditStep.Split => new Drawable[]
        {
            new TextEditStepSection(),
            new TextSwitchSpecialActionSection(),
        },
        TextEditStep.Verify => new Drawable[]
        {
            new TextEditStepSection(),
            new TextIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
