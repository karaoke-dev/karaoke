// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagExtend : TextTagExtend
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
            TextTagEditMode.Generate => new Drawable[]
            {
                new RubyTagEditModeSection(),
                new RubyTagAutoGenerateSection(),
            },
            TextTagEditMode.Edit => new Drawable[]
            {
                new RubyTagEditModeSection(),
                new RubyTagEditSection(),
            },
            TextTagEditMode.Verify => new Drawable[]
            {
                new RubyTagEditModeSection(),
                new RubyTagIssueSection(),
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
