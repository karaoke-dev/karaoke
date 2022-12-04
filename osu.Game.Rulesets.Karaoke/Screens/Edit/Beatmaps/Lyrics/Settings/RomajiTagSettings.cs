// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings
{
    public class RomajiTagSettings : TextTagSettings<RomajiTagEditMode>
    {
        [BackgroundDependencyLoader]
        private void load(IEditRomajiModeState romajiModeState)
        {
            EditMode.BindTo(romajiModeState.BindableEditMode);
            EditMode.BindValueChanged(e =>
            {
                ReloadSections();
            }, true);
        }

        protected override IReadOnlyList<Drawable> CreateSections() => EditMode.Value switch
        {
            RomajiTagEditMode.Generate => new Drawable[]
            {
                new RomajiTagEditModeSection(),
                new RomajiTagAutoGenerateSection(),
            },
            RomajiTagEditMode.Edit => new Drawable[]
            {
                new RomajiTagEditModeSection(),
                new RomajiTagEditSection(),
            },
            RomajiTagEditMode.Verify => new Drawable[]
            {
                new RomajiTagEditModeSection(),
                new RomajiTagIssueSection(),
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
