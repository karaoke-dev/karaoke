// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Texting;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class TextingModeState : Component, ITextingModeState
{
    private readonly Bindable<TextingEditStep> bindableEditMode = new();

    public IBindable<TextingEditStep> BindableEditMode => bindableEditMode;

    public void ChangeEditMode(TextingEditStep step)
        => bindableEditMode.Value = step;

    public Bindable<TextingEditModeSpecialAction> BindableSpecialAction { get; } = new();
}
