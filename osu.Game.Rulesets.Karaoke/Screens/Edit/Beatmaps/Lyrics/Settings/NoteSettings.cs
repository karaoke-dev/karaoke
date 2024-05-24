// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Notes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class NoteSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 300;

    private readonly IBindable<NoteEditStep> bindableEditStep = new Bindable<NoteEditStep>();

    [BackgroundDependencyLoader]
    private void load(IEditNoteModeState editNoteModeState)
    {
        bindableEditStep.BindTo(editNoteModeState.BindableEditStep);
        bindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableEditStep.Value switch
    {
        NoteEditStep.Generate => new Drawable[]
        {
            new NoteSettingsHeader(),
            new NoteConfigSection(),
            new NoteSwitchSpecialActionSection(),
        },
        NoteEditStep.Edit => new Drawable[]
        {
            new NoteSettingsHeader(),
            new NoteEditPropertyModeSection(),
            new NoteEditPropertySection(),
        },
        NoteEditStep.Verify => new Drawable[]
        {
            new NoteSettingsHeader(),
            new NoteIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
