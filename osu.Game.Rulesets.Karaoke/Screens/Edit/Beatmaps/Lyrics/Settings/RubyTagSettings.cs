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
        BindableEditStep.BindTo(editRubyModeState.BindableEditStep);
        BindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => BindableEditStep.Value switch
    {
        RubyTagEditStep.Generate => new Drawable[]
        {
            new RubyTagEditStepSection(),
            new RubyTagAutoGenerateSection(),
        },
        RubyTagEditStep.Edit => new Drawable[]
        {
            new RubyTagEditStepSection(),
            new RubyTagEditSection(),
        },
        RubyTagEditStep.Verify => new Drawable[]
        {
            new RubyTagEditStepSection(),
            new RubyTagIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
