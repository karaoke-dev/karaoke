// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class RomajiTagSettings : TextTagSettings<RomajiTagEditStep>
{
    [BackgroundDependencyLoader]
    private void load(IEditRomajiModeState romajiModeState)
    {
        BindableEditStep.BindTo(romajiModeState.BindableEditStep);
        BindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => BindableEditStep.Value switch
    {
        RomajiTagEditStep.Generate => new Drawable[]
        {
            new RomajiTagEditStepSection(),
            new RomajiTagAutoGenerateSection(),
        },
        RomajiTagEditStep.Edit => new Drawable[]
        {
            new RomajiTagEditStepSection(),
            new RomajiTagEditSection(),
        },
        RomajiTagEditStep.Verify => new Drawable[]
        {
            new RomajiTagEditStepSection(),
            new RomajiTagIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
