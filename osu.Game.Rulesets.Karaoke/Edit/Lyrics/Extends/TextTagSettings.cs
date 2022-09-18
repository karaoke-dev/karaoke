// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public abstract class TextTagSettings : LyricEditorSettings
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 350;

        protected readonly IBindable<TextTagEditMode> EditMode = new Bindable<TextTagEditMode>();
    }
}
