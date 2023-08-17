// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class RubyTagSettings : TextTagSettings<RubyTagEditStep>
{
    [BackgroundDependencyLoader]
    private void load(IEditRubyModeState editRubyModeState)
    {
        EditMode.BindTo(editRubyModeState.BindableEditMode);
        EditMode.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => EditMode.Value switch
    {
        RubyTagEditStep.Generate => new Drawable[]
        {
            new RubyTagEditModeSection(),
            new RubyTagAutoGenerateSection(),
        },
        RubyTagEditStep.Edit => new Drawable[]
        {
            new RubyTagEditModeSection(),
            new RubyTagEditSection(),
        },
        RubyTagEditStep.Verify => new Drawable[]
        {
            new RubyTagEditModeSection(),
            new RubyTagIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
