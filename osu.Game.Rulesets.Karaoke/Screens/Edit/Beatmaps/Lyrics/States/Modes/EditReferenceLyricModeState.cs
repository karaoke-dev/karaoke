// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditReferenceLyricModeState : Component, IEditReferenceLyricModeState
{
    private readonly Bindable<ReferenceLyricEditMode> bindableEditMode = new();

    public IBindable<ReferenceLyricEditMode> BindableEditMode => bindableEditMode;

    public void ChangeEditMode(ReferenceLyricEditMode mode)
        => bindableEditMode.Value = mode;
}
