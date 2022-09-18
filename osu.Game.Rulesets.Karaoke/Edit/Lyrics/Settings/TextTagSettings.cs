// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings
{
    public abstract class TextTagSettings : LyricEditorSettings
    {
        public override SettingsDirection Direction => SettingsDirection.Right;

        public override float SettingsWidth => 350;

        protected readonly IBindable<TextTagEditMode> EditMode = new Bindable<TextTagEditMode>();
    }
}
