// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class ReferenceSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 300;

    private readonly IBindable<ReferenceLyricEditMode> bindableMode = new Bindable<ReferenceLyricEditMode>();

    [BackgroundDependencyLoader]
    private void load(IEditReferenceLyricModeState editReferenceLyricModeState)
    {
        bindableMode.BindTo(editReferenceLyricModeState.BindableEditMode);
        bindableMode.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableMode.Value switch
    {
        ReferenceLyricEditMode.Edit => new Drawable[]
        {
            new ReferenceLyricEditModeSection(),
            new ReferenceLyricAutoGenerateSection(),
            new ReferenceLyricSection(),
            new ReferenceLyricConfigSection(),
        },
        ReferenceLyricEditMode.Verify => new Drawable[]
        {
            new ReferenceLyricEditModeSection(),
            new ReferenceLyricIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
