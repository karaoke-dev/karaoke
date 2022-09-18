// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings
{
    public class RomajiTagSettings : TextTagSettings
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
            TextTagEditMode.Generate => new Drawable[]
            {
                new RomajiTagEditModeSection(),
                new RomajiTagAutoGenerateSection(),
            },
            TextTagEditMode.Edit => new Drawable[]
            {
                new RomajiTagEditModeSection(),
                new RomajiTagEditSection(),
            },
            TextTagEditMode.Verify => new Drawable[]
            {
                new RomajiTagEditModeSection(),
                new RomajiTagIssueSection(),
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
