// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Texting;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings
{
    public partial class TextingSettings : LyricEditorSettings
    {
        public override SettingsDirection Direction => SettingsDirection.Right;

        public override float SettingsWidth => 300;

        private readonly IBindable<TextingEditMode> bindableMode = new Bindable<TextingEditMode>();

        [BackgroundDependencyLoader]
        private void load(ITextingModeState textingModeState)
        {
            bindableMode.BindTo(textingModeState.BindableEditMode);
            bindableMode.BindValueChanged(e =>
            {
                ReloadSections();
            }, true);
        }

        protected override IReadOnlyList<Drawable> CreateSections() => bindableMode.Value switch
        {
            TextingEditMode.Typing => new Drawable[]
            {
                new TextingEditModeSection(),
                new TextingSwitchSpecialActionSection(),
            },
            TextingEditMode.Split => new Drawable[]
            {
                new TextingEditModeSection(),
                new TextingSwitchSpecialActionSection(),
            },
            TextingEditMode.Verify => new Drawable[]
            {
                new TextingEditModeSection(),
                new TextingIssueSection(),
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
