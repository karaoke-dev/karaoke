// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Text;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class TextModeState : Component, ITextModeState
{
    private readonly Bindable<TextEditStep> bindableEditMode = new();

    public IBindable<TextEditStep> BindableEditStep => bindableEditMode;

    public void ChangeEditStep(TextEditStep step)
        => bindableEditMode.Value = step;

    public Bindable<TextEditModeSpecialAction> BindableSpecialAction { get; } = new();
}
