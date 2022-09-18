// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Language
{
    public class LanguageExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        private readonly IBindable<LanguageEditMode> bindableMode = new Bindable<LanguageEditMode>();

        [BackgroundDependencyLoader]
        private void load(ILanguageModeState languageModeState)
        {
            bindableMode.BindTo(languageModeState.BindableEditMode);
            bindableMode.BindValueChanged(e =>
            {
                ReloadSections();
            }, true);
        }

        protected override IReadOnlyList<Drawable> CreateSections() => bindableMode.Value switch
        {
            LanguageEditMode.Generate => new Drawable[]
            {
                new LanguageEditModeSection(),
                new LanguageSwitchSpecialActionSection(),
            },
            LanguageEditMode.Verify => new Drawable[]
            {
                new LanguageEditModeSection(),
                new LanguageMissingSection(),
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
