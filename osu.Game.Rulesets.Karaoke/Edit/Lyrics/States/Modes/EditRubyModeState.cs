// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public class EditRubyModeState : Component, IEditRubyModeState
    {
        private readonly Bindable<TextTagEditMode> bindableEditMode = new();

        public IBindable<TextTagEditMode> BindableEditMode => bindableEditMode;

        public void ChangeEditMode(TextTagEditMode mode)
            => bindableEditMode.Value = mode;

        public BindableList<RubyTag> SelectedItems { get; } = new();
    }
}
